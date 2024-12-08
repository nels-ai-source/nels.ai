using Volo.Abp.Reflection;

namespace Nels.Abp.SysMng.Permissions;

public class SysMngPermissions
{
    public const string GroupName = "SysMng";
    //Add your own permission names. Example:
    #region functionPage
    public static class App
    {
        public const string Default = GroupName + ".App";
        public const string GetList = Default + ".GetList";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class BusinessUnit
    {
        public const string Default = GroupName + ".BusinessUnit";
        public const string GetList = Default + ".GetList";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }
    public static class Page
    {
        public const string Default = GroupName + ".Page";
        public const string GetList = Default + ".GetList";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }
    public static class User
    {
        public const string Default = GroupName + ".User";
        public const string GetList = Default + ".GetList";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string ManagePermissions = Default + ".ManagePermissions";
    }

    public static class OrganizationUnit
    {
        public const string Default = GroupName + ".OrganizationUnit";
        public const string GetList = Default + ".GetList";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class Role
    {
        public const string Default = GroupName + ".Role";
        public const string GetList = Default + ".GetList";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string ManagePermissions = Default + ".ManagePermissions";
    }

    public static class BizParameter
    {
        public const string Default = GroupName + ".BizParameter";
        public const string GetList = Default + ".GetList";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }
    public static class BizParameteValue
    {
        public const string Default = GroupName + ".BizParameterValue";
        public const string GetList = Default + ".GetList";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }
    #endregion

    
    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(SysMngPermissions));
    }
}
