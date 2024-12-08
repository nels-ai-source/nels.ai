using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Nels.SemanticKernel.Interfaces;
using Nels.SemanticKernel.Process.Interfaces;
using Nels.SemanticKernel.Process.Services;
using Nels.SemanticKernel.Process.Steps;
using Nels.SemanticKernel.Services;

namespace Nels.SemanticKernel.Process;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKernelProcess(this IServiceCollection services,
        Action<KernelProcessOptions>? processAction = null)
    {
        if (services.Any(x => x.ServiceType == typeof(KernelProcessOptions)))
            throw new InvalidOperationException("KernelProcess services already registered");


        var processStateTypeResolver = new ProcessStateTypeResolver();
        var proceessSerializer = new ProceessSerializer();
        proceessSerializer.SerializerOptions.TypeInfoResolver = processStateTypeResolver;


        var options = new KernelProcessOptions(services, proceessSerializer, processStateTypeResolver);
        options.RegisterKernelProcessStepType<StartStep>();

        processAction?.Invoke(options);

        services.AddSingleton<IProceessSerializer>(proceessSerializer);
        services.AddSingleton<KernelProcessOptions>(options);

        return services;
    }
}
