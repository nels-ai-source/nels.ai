using System.Collections.Generic;

namespace Nels.Abp.SysMng.FunctionPage;

public class AppDefinition(App app, List<BusinessUnit> businessUnits, List<Page> pages)
{
    public App App { get; } = app;
    public IList<BusinessUnit> BusinessUnits { get; } = businessUnits;
    public IList<Page> Page { get; } = pages;
}
