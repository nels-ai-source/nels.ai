using System.Collections.Generic;
using Volo.Abp.Users;

namespace Nels.Abp.SysMng.FunctionPage;

public class UserMenuOutputDto
{
    public List<MenuDto> Menus { get; set; } = [];
    public required ICurrentUser UserInfo { get; set; }
    public List<string> Permissions { get; set; } = [];

    public ApplicationLocalizationConfigurationDto Localization { get; set; } = new();
}
