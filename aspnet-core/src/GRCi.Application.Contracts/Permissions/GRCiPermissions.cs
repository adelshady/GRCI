namespace GRCi.Permissions;

public static class GRCiPermissions
{
    public const string GroupName = "GRCi";

    public static class Compliance
    {
        public const string Default = GroupName + ".Compliance";

        public static class Templates
        {
            public const string Default = Compliance.Default + ".Templates";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
            public const string Submit = Default + ".Submit";
            public const string Approve = Default + ".Approve";
            public const string Return = Default + ".Return";
            public const string Archive = Default + ".Archive";
        }

        public static class Items
        {
            public const string Default = Compliance.Default + ".Items";
            public const string Manage = Default + ".Manage";
        }

        public static class Attachments
        {
            public const string Default = Compliance.Default + ".Attachments";
            public const string Manage = Default + ".Manage";
        }
    }
}
