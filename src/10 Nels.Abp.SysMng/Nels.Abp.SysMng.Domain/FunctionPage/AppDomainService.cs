using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace Nels.Abp.SysMng.FunctionPage;

public class AppDomainService(IRepository<App, Guid> appRepository, IRepository<BusinessUnit, Guid> unitRepository, IRepository<Page, Guid> repository,
    IIdentityUserRepository identityUserRepository,
    IRepository<PermissionGrant> permissionGrantRepository) : DomainService
{
    private readonly IRepository<App, Guid> _appRepository = appRepository;
    private readonly IRepository<BusinessUnit, Guid> _unitRepository = unitRepository;
    private readonly IRepository<Page, Guid> _repository = repository;
    private readonly IIdentityUserRepository _userRepository = identityUserRepository;
    private readonly IRepository<PermissionGrant> _permissionGrantRepository = permissionGrantRepository;

    [UnitOfWork]
    public async Task InitAppPages(App app, List<BusinessUnit> businessUnits, List<Page> pages)
    {
        if (app != null)
        {
            await _appRepository.DeleteAsync(x => x.Id == app.Id);
            await _appRepository.InsertAsync(app);
        }
        if (businessUnits.Count != 0)
        {
            var businessIds = businessUnits.Select(x => x.Id).ToList();
            await _unitRepository.DeleteAsync(x => businessIds.Contains(x.Id));
            await _unitRepository.InsertManyAsync(businessUnits);
        }
        if (pages.Count != 0)
        {
            var pageIds = pages.Select(x => x.Id).ToList();
            await _repository.DeleteAsync(x => pageIds.Contains(x.Id));
            await _repository.InsertManyAsync(pages);
        }
    }
    public async Task<List<string>> GetUserPermissionGrants(Guid userId)
    {
        List<PermissionGrant> permissionGrants = [];
        var roleName = await _userRepository.GetRoleNamesAsync(userId);
        var query = await _permissionGrantRepository.GetQueryableAsync();
        if (roleName.Count != 0)
        {
            query.WhereIf(roleName.Count != 0, x => x.ProviderName == "R" && roleName.Contains(x.ProviderKey));
            permissionGrants.AddRange(await AsyncExecuter.ToListAsync(query));
        }
        permissionGrants.AddRange(await _permissionGrantRepository.GetListAsync(x => x.ProviderName == "U" && x.ProviderKey == userId.ToString()));

        return await Task.FromResult(permissionGrants.Select(x => x.Name).Distinct().ToList());
    }
    public async Task<(List<BusinessUnit> businesses, List<Page> pages)> GetAppMeuns(Guid appId, List<string> permissions)
    {
        var businesses = await _unitRepository.GetListAsync(x => x.ApplicationId.Equals(appId));
        var pageQueryable = await _repository.GetQueryableAsync();
        pageQueryable = pageQueryable.WhereIf(appId != Guid.Empty, x => x.ApplicationId.Equals(appId));
        pageQueryable = pageQueryable.WhereIf(permissions.Count != 0, x => permissions.Contains(x.Permission) || x.Permission == string.Empty);

        var pages = await AsyncExecuter.ToListAsync(pageQueryable);
        businesses = businesses?.OrderBy(x => x.Sort).ToList();
        pages = pages?.OrderBy(x => x.Sort).ToList();
        return await Task.FromResult((businesses, pages));
    }
}
