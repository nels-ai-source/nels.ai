using Nels.Abp.Ddd.Domain;
using Nels.Aigc.Entities;
using Nels.Aigc.Localization;
using Nels.Aigc.Settings;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.SettingManagement;

namespace Nels.Aigc.Handlers;

public class SpaceCreatedUserHandler : Handler, ILocalEventHandler<EntityCreatedEventData<IdentityUser>>, ITransientDependency
{
    private readonly IRepository<Space, Guid> _repository;
    private readonly IRepository<SpaceUser, Guid> _spaceUsrrepository;
    private readonly ISettingManager _settingManager;


    public SpaceCreatedUserHandler(IRepository<Space, Guid> repository,
    IRepository<SpaceUser, Guid> spaceUsrrepository,
    IGuidGenerator guidGenerator,
    ISettingManager settingManager,
    LocalizableStringSerializer localizableStringSerializer)
    {
        _repository = repository;
        _spaceUsrrepository = spaceUsrrepository;
        _settingManager = settingManager;

        LocalizationResource = typeof(AigcResource);
    }

    public async Task HandleEventAsync(EntityCreatedEventData<IdentityUser> eventData)
    {
        Space space = new(GuidGenerator.Create())
        {
            Name = GetDisplayName("Space:Personal"),
            SpaceType = Enums.SpaceType.Personal,
        };

        space.AddSpaceOwnerUser(GuidGenerator.Create(), eventData.Entity.Id);

        await _repository.InsertAsync(space);
        await _spaceUsrrepository.InsertManyAsync(space.SpaceUsers);

        await _settingManager.SetForCurrentUserAsync(AigcSettingConst.CurrentSpaceIdName, space.Id.ToString());
    }

    protected string GetDisplayName(string name)
    {
        name = name.Split(',').Last();
        var localizable = L[name];
        if (localizable == null) return name;

        return localizable.Value ?? string.Empty;
    }
}
