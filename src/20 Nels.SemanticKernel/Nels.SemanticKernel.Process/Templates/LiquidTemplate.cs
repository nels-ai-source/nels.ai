using Fluid;

namespace Nels.SemanticKernel.Process.Templates;

public static class LiquidTemplate
{
    private static readonly FluidParser s_parser = new();
    private static readonly TemplateOptions s_templateOptions = new()
    {
        MemberAccessStrategy = new UnsafeMemberAccessStrategy() { MemberNameStrategy = MemberNameStrategies.SnakeCase },
    };

    public static string Render(string template, Dictionary<string, object?> dictionary)
    {
        if (!s_parser.TryParse(template, out IFluidTemplate liquidTemplate, out string error))
        {
            throw new ArgumentException(error is not null ?
                $"The template could not be parsed:{Environment.NewLine}{error}" :
                 "The template could not be parsed.");
        }
        var variables = GetTemplateContext(dictionary);
        return liquidTemplate.Render(variables);
    }
    /// <summary>
    /// Gets the variables for the prompt template, including setting any default values from the prompt config.
    /// </summary>
    private static TemplateContext GetTemplateContext(Dictionary<string, object?> dictionary)
    {
        var ctx = new TemplateContext(s_templateOptions);
        if (dictionary == null || dictionary.Count == 0) { return ctx; }

        foreach (var item in dictionary)
        {
            if (item.Value is null || item.Value is string stringDefault && stringDefault.Length == 0)
            {
                continue;
            }
            ctx.SetValue(item.Key, item.Value);
        }

        return ctx;
    }
}
