using GRCi.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace GRCi.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class GRCiController : AbpControllerBase
{
    protected GRCiController()
    {
        LocalizationResource = typeof(GRCiResource);
    }
}
