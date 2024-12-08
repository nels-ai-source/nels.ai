using System.Collections.Generic;

namespace Nels.Abp.SysMng.FunctionPage;

public interface IFunctionPageDefinitionContext
{
    AppDefinition AddGroup(App app, List<BusinessUnit> businessUnits, List<Page> pages);
    AppDefinition GetGroup(string name);
    AppDefinition? GetGroupOrNull(string name);
}
