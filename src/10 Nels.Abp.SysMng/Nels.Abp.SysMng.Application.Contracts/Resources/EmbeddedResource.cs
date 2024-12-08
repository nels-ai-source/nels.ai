using Microsoft.SemanticKernel;
using System.IO;
using System.Reflection;

namespace Nels.Abp.SysMng.Resources;

public static class EmbeddedResource
{
    private static readonly string? s_namespace = typeof(EmbeddedResource).Namespace;

    public static string Read(string name)
    {
        var assembly = typeof(EmbeddedResource).GetTypeInfo().Assembly ??
            throw new FileNotFoundException($"[{s_namespace}] {name} assembly not found");

        using Stream? resource = assembly.GetManifestResourceStream($"{s_namespace}." + name) ??
            throw new FileNotFoundException($"[{s_namespace}] {name} resource not found");

        using var reader = new StreamReader(resource);
        return reader.ReadToEnd();
    }

    public static PromptTemplateConfig ToPromptTemplateConfig(string name)
    {
        var text = Read(name);
        return KernelFunctionYaml.ToPromptTemplateConfig(text);
    }
}
