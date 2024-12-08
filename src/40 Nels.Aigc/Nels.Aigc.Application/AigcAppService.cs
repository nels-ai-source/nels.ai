using Nels.Aigc.Localization;
using Volo.Abp.Application.Services;

namespace Nels.Aigc;

/* Inherit your application services from this class.
 */
public abstract class AigcAppService : ApplicationService
{
    protected AigcAppService()
    {
        LocalizationResource = typeof(AigcResource);
    }
}
