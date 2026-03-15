using System;
using System.Collections.Generic;
using System.Text;
using GRCi.Localization;
using Volo.Abp.Application.Services;

namespace GRCi;

/* Inherit your application services from this class.
 */
public abstract class GRCiAppService : ApplicationService
{
    protected GRCiAppService()
    {
        LocalizationResource = typeof(GRCiResource);
    }
}
