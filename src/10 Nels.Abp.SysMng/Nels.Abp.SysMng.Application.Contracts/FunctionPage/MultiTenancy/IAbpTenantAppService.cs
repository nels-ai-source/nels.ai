using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Nels.Abp.SysMng.FunctionPage.MultiTenancy;

public interface IAbpTenantAppService : IApplicationService
{
    Task<FindTenantResultDto> FindTenantByNameAsync(string name);

    Task<FindTenantResultDto> FindTenantByIdAsync(Guid id);
}
