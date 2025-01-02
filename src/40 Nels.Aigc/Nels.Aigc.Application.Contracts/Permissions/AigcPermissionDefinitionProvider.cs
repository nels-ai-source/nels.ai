using Nels.Abp.SysMng.Permissions;
using Nels.Aigc.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Nels.Aigc.Permissions;

public class AigcPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(AigcPermissions.GroupName);



        #region aigc
        myGroup.AddPermission(AigcPermissions.Model.Default, GetDisplayName(AigcPermissions.Model.Default))
        .AddChild(AigcPermissions.Model.GetList, GetDisplayName(AigcPermissions.Model.GetList))
        .AddChild(AigcPermissions.Model.Create, GetDisplayName(AigcPermissions.Model.Create))
        .AddChild(AigcPermissions.Model.Update, GetDisplayName(AigcPermissions.Model.Update))
        .AddChild(AigcPermissions.Model.Delete, GetDisplayName(AigcPermissions.Model.Delete));

        myGroup.AddPermission(AigcPermissions.ModelInstance.Default, GetDisplayName(AigcPermissions.ModelInstance.Default))
        .AddChild(AigcPermissions.ModelInstance.GetList, GetDisplayName(AigcPermissions.ModelInstance.GetList))
        .AddChild(AigcPermissions.ModelInstance.Create, GetDisplayName(AigcPermissions.ModelInstance.Create))
        .AddChild(AigcPermissions.ModelInstance.Update, GetDisplayName(AigcPermissions.ModelInstance.Update))
        .AddChild(AigcPermissions.ModelInstance.Delete, GetDisplayName(AigcPermissions.ModelInstance.Delete));

        myGroup.AddPermission(AigcPermissions.Prompt.Default, GetDisplayName(AigcPermissions.Prompt.Default))
        .AddChild(AigcPermissions.Prompt.GetList, GetDisplayName(AigcPermissions.Prompt.GetList))
        .AddChild(AigcPermissions.Prompt.Create, GetDisplayName(AigcPermissions.Prompt.Create))
        .AddChild(AigcPermissions.Prompt.Update, GetDisplayName(AigcPermissions.Prompt.Update))
        .AddChild(AigcPermissions.Prompt.Delete, GetDisplayName(AigcPermissions.Prompt.Delete));

        myGroup.AddPermission(AigcPermissions.Agent.Default, GetDisplayName(AigcPermissions.Agent.Default))
        .AddChild(AigcPermissions.Agent.GetList, GetDisplayName(AigcPermissions.Agent.GetList))
        .AddChild(AigcPermissions.Agent.Create, GetDisplayName(AigcPermissions.Agent.Create))
        .AddChild(AigcPermissions.Agent.Update, GetDisplayName(AigcPermissions.Agent.Update))
        .AddChild(AigcPermissions.Agent.Delete, GetDisplayName(AigcPermissions.Agent.Delete));

        myGroup.AddPermission(AigcPermissions.Space.Default, GetDisplayName(AigcPermissions.Space.Default))
.AddChild(AigcPermissions.Space.GetList, GetDisplayName(AigcPermissions.Space.GetList))
.AddChild(AigcPermissions.Space.Create, GetDisplayName(AigcPermissions.Space.Create))
.AddChild(AigcPermissions.Space.Update, GetDisplayName(AigcPermissions.Space.Update))
.AddChild(AigcPermissions.Space.Delete, GetDisplayName(AigcPermissions.Space.Delete));


        myGroup.AddPermission(AigcPermissions.Knowledge.Default, GetDisplayName(AigcPermissions.Knowledge.Default))
.AddChild(AigcPermissions.Knowledge.GetList, GetDisplayName(AigcPermissions.Knowledge.GetList))
.AddChild(AigcPermissions.Knowledge.Create, GetDisplayName(AigcPermissions.Knowledge.Create))
.AddChild(AigcPermissions.Knowledge.Update, GetDisplayName(AigcPermissions.Knowledge.Update))
.AddChild(AigcPermissions.Knowledge.Delete, GetDisplayName(AigcPermissions.Knowledge.Delete));

        myGroup.AddPermission(AigcPermissions.KnowledgeDocument.Default, GetDisplayName(AigcPermissions.KnowledgeDocument.Default))
.AddChild(AigcPermissions.KnowledgeDocument.GetList, GetDisplayName(AigcPermissions.KnowledgeDocument.GetList))
.AddChild(AigcPermissions.KnowledgeDocument.Create, GetDisplayName(AigcPermissions.KnowledgeDocument.Create))
.AddChild(AigcPermissions.KnowledgeDocument.Update, GetDisplayName(AigcPermissions.KnowledgeDocument.Update))
.AddChild(AigcPermissions.KnowledgeDocument.Delete, GetDisplayName(AigcPermissions.KnowledgeDocument.Delete));
        #endregion


    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AigcResource>(name);
    }

    public static LocalizableString GetDisplayName(string name)
    {
        return L($"Permission:{name}");
    }
}
