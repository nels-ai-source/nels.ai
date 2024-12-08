using System.Collections.Generic;
using Volo.Abp.Settings;

namespace Nels.Abp.SysMng.Settings;

public class SysMngSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        var settingDefinitions = new List<SettingDefinition>
        {
            new(SysMngSettingConst.CurrentAppIdName)
        };
        context.Add([.. settingDefinitions]);
    }
}
