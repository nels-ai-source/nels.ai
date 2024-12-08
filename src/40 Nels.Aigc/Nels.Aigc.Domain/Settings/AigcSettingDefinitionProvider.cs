using Volo.Abp.Settings;

namespace Nels.Aigc.Settings;

public class AigcSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(AigcSettings.MySetting1));
    }
}
