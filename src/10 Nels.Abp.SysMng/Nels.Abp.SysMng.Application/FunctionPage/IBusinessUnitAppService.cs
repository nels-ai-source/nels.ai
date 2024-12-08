using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Nels.Abp.SysMng.FunctionPage
{
    public interface IBusinessUnitAppService
    {
        Task<PagedResultDto<BusinessUnitOutputDto>> GetAllListAsync();
    }
}