using Nels.Abp.Ddd.Application.Contracts;

namespace Nels.Abp.SysMng;

public class SysMngRemoteServiceConsts : RemoteServiceConsts
{
    public const string RemoteServiceName = "SysMng";

    public const string ModuleName = "sysMng";

    public const string appRoute = "api/app";
    public const string businessUnitRoute = "api/businessUnit";
    public const string pageRoute = "api/page";

    public const string UserRoute = "api/user";
    public const string OrganizationUnitRoute = "api/organizationUnit";
    public const string RoleRoute = "api/role";

    public const string PermissionRoute = "api/permission";

    public const string BizParamRoute = "api/bizParam";
    public const string BizParamValueRoute = "api/bizParamValue";

    public const string WfDefinitionRoute = "api/wfDefinition";

    public const string ApiRouteExecute = "ApiRouteExecute";

}
