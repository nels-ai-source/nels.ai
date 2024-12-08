using System.Collections.Generic;
using Volo.Abp.Settings;

namespace Nels.Aigc.Settings;

public class AigcSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        var settingDefinitions = new List<SettingDefinition>
        {
            new(AigcSettingConst.CurrentSpaceIdName)
        };
        context.Add([.. settingDefinitions]);
    }
}
