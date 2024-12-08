namespace Nels.Aigc.Permissions;

public static class AigcPermissions
{
    public const string GroupName = "Aigc";

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";

    #region aigc
    public static class Prompt
    {
        public const string Default = GroupName + ".Prompt";
        public const string GetList = Default + ".GetList";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }
    public static class Model
    {
        public const string Default = GroupName + ".Model";
        public const string GetList = Default + ".GetList";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }
    public static class ModelInstance
    {
        public const string Default = GroupName + ".ModelInstance";
        public const string GetList = Default + ".GetList";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }
    public static class Agent
    {
        public const string Default = GroupName + ".Agent";
        public const string GetList = Default + ".GetList";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class Space
    {
        public const string Default = GroupName + ".Space";
        public const string GetList = Default + ".GetList";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }
    public static class Knowledge
    {
        public const string Default = GroupName + ".Knowledge";
        public const string GetList = Default + ".GetList";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }
    public static class KnowledgeDocument
    {
        public const string Default = GroupName + ".KnowledgeDocument";
        public const string GetList = Default + ".GetList";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }
    #endregion

}
