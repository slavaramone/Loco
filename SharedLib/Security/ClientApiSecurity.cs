namespace SharedLib.Security
{
    public static class ClientApiSecurity
    {
        public static class Claims
        {
            public const string UserId = nameof(UserId);

            public const string UserRoles = nameof(UserRoles);
        }

        public static class Policies
        {
            public const string ContainsToken = nameof(ContainsToken);
        }
    }
}
