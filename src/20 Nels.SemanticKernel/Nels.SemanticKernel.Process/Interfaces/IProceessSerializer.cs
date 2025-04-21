using Microsoft.SemanticKernel;
using System.Text.Json;

namespace Nels.SemanticKernel.Process.Interfaces;

/// <summary>
/// Serializes and deserializes kernelProcess.
/// </summary>
public interface IProceessSerializer
{
    JsonSerializerOptions SerializerOptions { get; set; }

    /// <summary>
    /// Serializes the specified kernelProcess.
    /// </summary>
    /// <param name="kernelProcess">The KernelProcess.</param>
    /// <returns>A string representing the serialized kernelProcess.</returns>
    string Serialize(KernelProcess kernelProcess);

    /// <summary>
    /// Serializes the specified kernelProcess.
    /// </summary>
    /// <param name="serializedKernelProcess">The data representing the kernelProcess.</param>
    /// <returns>A deserialized kernelProcess.</returns>
    KernelProcess Deserialize(string serializedKernelProcess);
}
