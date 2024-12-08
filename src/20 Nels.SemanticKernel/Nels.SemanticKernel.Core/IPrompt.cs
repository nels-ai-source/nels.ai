using System;

namespace Nels.SemanticKernel;

public interface IPrompt
{
    public Guid Id { get; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Template { get; set; }
    public string TemplateFormat { get; set; }
}
