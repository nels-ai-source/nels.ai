using Nels.Aigc.Entities;

namespace Nels.Aigc.Providers;

public interface IPromptDefinitionContext
{
    void AddPrompt(Prompt prompt);
}
