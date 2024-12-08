using Volo.Abp.Application.Dtos;

namespace Nels.Abp.SysMng.Identity;

public class GetIdentityUsersInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; } = string.Empty;
}
