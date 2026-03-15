using Microsoft.Extensions.Localization;
using GRCi.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace GRCi;

[Dependency(ReplaceServices = true)]
public class GRCiBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<GRCiResource> _localizer;

    public GRCiBrandingProvider(IStringLocalizer<GRCiResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
