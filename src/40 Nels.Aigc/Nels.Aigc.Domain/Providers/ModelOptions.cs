using Volo.Abp.Collections;

namespace Nels.Aigc.Providers;

public class ModelOptions
{
    public ITypeList<IModelProvider> DefinitionProviders { get; }

    public ModelOptions()
    {
        DefinitionProviders = new TypeList<IModelProvider>();
    }
}
