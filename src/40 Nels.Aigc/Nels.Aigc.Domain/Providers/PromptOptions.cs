using Volo.Abp.Collections;

namespace Nels.Aigc.Providers;

public class PromptOptions
{
    public ITypeList<IPromptProvider> DefinitionProviders { get; }

    public PromptOptions()
    {
        DefinitionProviders = new TypeList<IPromptProvider>();
    }
}
