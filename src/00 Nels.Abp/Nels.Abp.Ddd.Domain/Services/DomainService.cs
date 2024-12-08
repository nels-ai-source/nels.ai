using Microsoft.Extensions.Localization;
using Volo.Abp;
using Volo.Abp.Localization;

namespace Nels.Abp.Ddd.Domain.Services;

public abstract class DomainService : Volo.Abp.Domain.Services.DomainService
{

    protected IStringLocalizerFactory StringLocalizerFactory => LazyServiceProvider.LazyGetRequiredService<IStringLocalizerFactory>();
    protected IStringLocalizer L
    {
        get
        {
            if (_localizer == null)
            {
                _localizer = CreateLocalizer();
            }

            return _localizer;
        }
    }
    private IStringLocalizer? _localizer;
    protected Type? LocalizationResource
    {
        get => _localizationResource;
        set
        {
            _localizationResource = value;
            _localizer = null;
        }
    }

    protected virtual IStringLocalizer CreateLocalizer()
    {
        if (LocalizationResource != null)
        {
            return StringLocalizerFactory.Create(LocalizationResource);
        }

        var localizer = StringLocalizerFactory.CreateDefaultOrNull();
        if (localizer == null)
        {
            throw new AbpException($"Set {nameof(LocalizationResource)} or define the default localization resource type (by configuring the {nameof(AbpLocalizationOptions)}.{nameof(AbpLocalizationOptions.DefaultResourceType)}) to be able to use the {nameof(L)} object!");
        }

        return localizer;
    }
    private Type? _localizationResource = typeof(DefaultResource);
}
