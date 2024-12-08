using Nels.Abp.SysMng.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity;
using Volo.Abp.Localization;

namespace Nels.Abp.SysMng.Permissions;

public class SysMngPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public SysMngPermissionDefinitionProvider()
    {
    }

    public override void Define(IPermissionDefinitionContext context)
    {
        context.RemoveGroup(IdentityPermissions.GroupName);

        var sysMngGroup = context.AddGroup(SysMngPermissions.GroupName, GetDisplayName(SysMngPermissions.GroupName));

        #region functionPage
        sysMngGroup.AddPermission(SysMngPermissions.App.Default, GetDisplayName(SysMngPermissions.App.Default))
        .AddChild(SysMngPermissions.App.GetList, GetDisplayName(SysMngPermissions.App.GetList))
        .AddChild(SysMngPermissions.App.Create, GetDisplayName(SysMngPermissions.App.Create))
        .AddChild(SysMngPermissions.App.Update, GetDisplayName(SysMngPermissions.App.Update))
        .AddChild(SysMngPermissions.App.Delete, GetDisplayName(SysMngPermissions.App.Delete));

        sysMngGroup.AddPermission(SysMngPermissions.BusinessUnit.Default, GetDisplayName(SysMngPermissions.BusinessUnit.Default))
         .AddChild(SysMngPermissions.BusinessUnit.GetList, GetDisplayName(SysMngPermissions.BusinessUnit.GetList))
         .AddChild(SysMngPermissions.BusinessUnit.Create, GetDisplayName(SysMngPermissions.BusinessUnit.Create))
         .AddChild(SysMngPermissions.BusinessUnit.Update, GetDisplayName(SysMngPermissions.BusinessUnit.Update))
         .AddChild(SysMngPermissions.BusinessUnit.Delete, GetDisplayName(SysMngPermissions.BusinessUnit.Delete));

        sysMngGroup.AddPermission(SysMngPermissions.Page.Default, GetDisplayName(SysMngPermissions.Page.Default))
        .AddChild(SysMngPermissions.Page.GetList, GetDisplayName(SysMngPermissions.Page.GetList))
        .AddChild(SysMngPermissions.Page.Create, GetDisplayName(SysMngPermissions.Page.Create))
        .AddChild(SysMngPermissions.Page.Update, GetDisplayName(SysMngPermissions.Page.Update))
        .AddChild(SysMngPermissions.Page.Delete, GetDisplayName(SysMngPermissions.Page.Delete));


        sysMngGroup.AddPermission(SysMngPermissions.User.Default, GetDisplayName(SysMngPermissions.User.Default))
       .AddChild(SysMngPermissions.User.GetList, GetDisplayName(SysMngPermissions.User.GetList))
       .AddChild(SysMngPermissions.User.Create, GetDisplayName(SysMngPermissions.User.Create))
       .AddChild(SysMngPermissions.User.Update, GetDisplayName(SysMngPermissions.User.Update))
       .AddChild(SysMngPermissions.User.Delete, GetDisplayName(SysMngPermissions.User.Delete))
       .AddChild(SysMngPermissions.User.ManagePermissions, GetDisplayName(SysMngPermissions.User.ManagePermissions));

        sysMngGroup.AddPermission(SysMngPermissions.OrganizationUnit.Default, GetDisplayName(SysMngPermissions.OrganizationUnit.Default))
       .AddChild(SysMngPermissions.OrganizationUnit.GetList, GetDisplayName(SysMngPermissions.OrganizationUnit.GetList))
       .AddChild(SysMngPermissions.OrganizationUnit.Create, GetDisplayName(SysMngPermissions.OrganizationUnit.Create))
       .AddChild(SysMngPermissions.OrganizationUnit.Update, GetDisplayName(SysMngPermissions.OrganizationUnit.Update))
       .AddChild(SysMngPermissions.OrganizationUnit.Delete, GetDisplayName(SysMngPermissions.OrganizationUnit.Delete));

        sysMngGroup.AddPermission(SysMngPermissions.Role.Default, GetDisplayName(SysMngPermissions.Role.Default))
        .AddChild(SysMngPermissions.Role.GetList, GetDisplayName(SysMngPermissions.Role.GetList))
        .AddChild(SysMngPermissions.Role.Create, GetDisplayName(SysMngPermissions.Role.Create))
        .AddChild(SysMngPermissions.Role.Update, GetDisplayName(SysMngPermissions.Role.Update))
        .AddChild(SysMngPermissions.Role.Delete, GetDisplayName(SysMngPermissions.Role.Delete))
        .AddChild(SysMngPermissions.Role.ManagePermissions, GetDisplayName(SysMngPermissions.Role.ManagePermissions));

        sysMngGroup.AddPermission(SysMngPermissions.BizParameter.Default, GetDisplayName(SysMngPermissions.BizParameter.Default))
        .AddChild(SysMngPermissions.BizParameter.GetList, GetDisplayName(SysMngPermissions.BizParameter.GetList))
        .AddChild(SysMngPermissions.BizParameter.Create, GetDisplayName(SysMngPermissions.BizParameter.Create))
        .AddChild(SysMngPermissions.BizParameter.Update, GetDisplayName(SysMngPermissions.BizParameter.Update))
        .AddChild(SysMngPermissions.BizParameter.Delete, GetDisplayName(SysMngPermissions.BizParameter.Delete));

        sysMngGroup.AddPermission(SysMngPermissions.BizParameteValue.Default, GetDisplayName(SysMngPermissions.BizParameteValue.Default))
       .AddChild(SysMngPermissions.BizParameteValue.GetList, GetDisplayName(SysMngPermissions.BizParameteValue.GetList))
       .AddChild(SysMngPermissions.BizParameteValue.Create, GetDisplayName(SysMngPermissions.BizParameteValue.Create))
       .AddChild(SysMngPermissions.BizParameteValue.Update, GetDisplayName(SysMngPermissions.BizParameteValue.Update))
       .AddChild(SysMngPermissions.BizParameteValue.Delete, GetDisplayName(SysMngPermissions.BizParameteValue.Delete));
        #endregion


    }

    public static LocalizableString GetDisplayName(string name)
    {
        return L($"Permission:{name}");
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<SysMngResource>(name);
    }
}
