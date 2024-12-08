using Volo.Abp.Collections;

namespace Nels.Abp.SysMng.FunctionPage;

public class FunctionPageOptions
{
    public ITypeList<IFunctionPageProvider> DefinitionProviders { get; }

    public FunctionPageOptions()
    {
        DefinitionProviders = new TypeList<IFunctionPageProvider>();
    }
}
