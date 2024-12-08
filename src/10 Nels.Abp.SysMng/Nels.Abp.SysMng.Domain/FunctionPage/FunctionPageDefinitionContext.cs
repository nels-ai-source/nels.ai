using System;
using System.Collections.Generic;
using Volo.Abp;

namespace Nels.Abp.SysMng.FunctionPage;

public class FunctionPageDefinitionContext(IServiceProvider serviceProvider) : IFunctionPageDefinitionContext
{
    public IServiceProvider ServiceProvider { get; } = serviceProvider;
    public Dictionary<string, AppDefinition> AppGroups { get; } = [];

    public virtual AppDefinition AddGroup(App app, List<BusinessUnit> businessUnits, List<Page> pages)
    {
        Check.NotNull(app, nameof(app));

        if (AppGroups.ContainsKey(app.Name))
        {
            throw new AbpException($"There is already an existing app group with name: {app.Name}");
        }
        return AppGroups[app.Name] = new AppDefinition(app, businessUnits, pages);
    }
    public virtual AppDefinition GetGroup(string name)
    {
        var group = GetGroupOrNull(name);

        if (group == null)
        {
            throw new AbpException($"Could not find a permission app group with the given name: {name}");
        }

        return group;
    }
    public virtual AppDefinition? GetGroupOrNull(string name)
    {
        Check.NotNull(name, nameof(name));

        if (!AppGroups.ContainsKey(name))
        {
            return null;
        }

        return AppGroups[name];
    }
}
