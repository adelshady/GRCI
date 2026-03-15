using Volo.Abp.Settings;

namespace GRCi.Settings;

public class GRCiSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(GRCiSettings.MySetting1));
    }
}
