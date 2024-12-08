using Nels.Abp.SysMng.Localization;
using Volo.Abp.Application.Services;

namespace Nels.Abp.SysMng;

public abstract class SysMngAppService : ApplicationService
{
    protected SysMngAppService()
    {
        LocalizationResource = typeof(SysMngResource);
        ObjectMapperContext = typeof(NelsAbpSysMngApplicationModule);
    }
}
