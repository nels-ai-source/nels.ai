using Microsoft.SemanticKernel;
using Nels.SemanticKernel.Process.Interfaces;
using Nels.SemanticKernel.Process.Steps;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json.Serialization;
using Nels.SemanticKernel.Process.Variables;

namespace Nels.SemanticKernel.Process.Services;

public class ProceessSerializer : IProceessSerializer
{
    public JsonSerializerOptions SerializerOptions { get; set; }

    private static readonly JsonSerializerOptions s_jsonOptions = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
    public ProceessSerializer()
    {
        SerializerOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    }

    public KernelProcess Deserialize(string serializedKernelProcess)
    {
        var daprProcessInfo = JsonSerializer.Deserialize<DaprProcessInfo>(serializedKernelProcess, SerializerOptions);
        return daprProcessInfo?.ToKernelProcess() ?? throw new InvalidOperationException("Failed to deserialize and convert DaprProcessInfo to KernelProcess.");
    }

    public string Serialize(KernelProcess kernelProcess)
    {
        var daprProcess = DaprProcessInfo.FromKernelProcess(kernelProcess);
        return JsonSerializer.Serialize(daprProcess, SerializerOptions);
    }
}
/// <summary>
/// An implementation of <see cref="JsonTypeInfoResolver"/> that resolves the type information for <see cref="KernelProcessStepState{T}"/>.
/// </summary>
public class ProcessStateTypeResolver : DefaultJsonTypeInfoResolver
{
    private static readonly Type s_genericType = typeof(KernelProcessStep<>);
    private readonly Dictionary<string, Type> _types = new() { { "process", typeof(KernelProcessState) }, };


    public void RegisterKernelProcessStepType<T>() where T : KernelProcessStep
    {
        // Load all types from the resources assembly that derive from KernelProcessStep
        var assembly = typeof(T).Assembly;
        var stepTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(KernelProcessStep)));

        foreach (var type in stepTypes)
        {
            if (TryGetSubtypeOfStatefulStep(type, out Type? genericStepType) && genericStepType is not null)
            {
                var userStateType = genericStepType.GetGenericArguments()[0];
                var stateType = typeof(KernelProcessStepState<>).MakeGenericType(userStateType);
                this._types.TryAdd(userStateType.Name, stateType);
            }
        }
    }


    /// <inheritdoc />
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

        Type baseType = typeof(KernelProcessStepState);
        if (jsonTypeInfo.Type == baseType)
        {
            var jsonDerivedTypes = this._types.Select(t => new JsonDerivedType(t.Value, t.Key)).ToList();

            jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
            {
                TypeDiscriminatorPropertyName = "$state-type",
                IgnoreUnrecognizedTypeDiscriminators = true,
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization
            };

            // Add the known derived types to the collection
            var derivedTypesCollection = jsonTypeInfo.PolymorphismOptions.DerivedTypes;
            if (derivedTypesCollection is List<JsonDerivedType> list)
            {
                list.AddRange(jsonDerivedTypes);
            }
            else
            {
                foreach (var item in jsonDerivedTypes!)
                {
                    derivedTypesCollection!.Add(item);
                }
            }
        }
        else if (jsonTypeInfo.Type == typeof(DaprStepInfo))
        {
            jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
            {
                TypeDiscriminatorPropertyName = "$state-type",
                IgnoreUnrecognizedTypeDiscriminators = true,
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                DerivedTypes =
                {
                    new JsonDerivedType(typeof(DaprProcessInfo), nameof(DaprProcessInfo)),
                }
            };
        }

        return jsonTypeInfo;
    }

    private static bool TryGetSubtypeOfStatefulStep(Type? type, out Type? genericStateType)
    {
        while (type != null && type != typeof(object))
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == s_genericType)
            {
                genericStateType = type;
                return true;
            }

            type = type.BaseType;
        }

        genericStateType = null;
        return false;
    }
}
