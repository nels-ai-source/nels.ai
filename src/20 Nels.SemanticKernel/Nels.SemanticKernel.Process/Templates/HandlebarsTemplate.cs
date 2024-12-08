using HandlebarsDotNet;

namespace Nels.SemanticKernel.Process.Templates;

public static class HandlebarsTemplate
{
    public static string Render(string template, Dictionary<string, object?> dictionary)
    {
        var handlebarsInstance = Handlebars.Create();
        var handlebarsTemplate = handlebarsInstance.Compile(template);

        return System.Net.WebUtility.HtmlDecode(handlebarsTemplate(dictionary).Trim());
    }
}
