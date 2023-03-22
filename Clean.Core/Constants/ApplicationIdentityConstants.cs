namespace Clean.Core.Constants
{
    public class ApplicationIdentityConstants
    {
        public static class Roles
        {
            public static readonly string Administrator = "Administrator";
            public static readonly string Config = "Config";
            public static readonly string Member = "Member";

            public static readonly string[] RolesSupported = { Administrator,Config, Member };
        }

        public static readonly string DefaultPassword = "Password@1";
    }
}
