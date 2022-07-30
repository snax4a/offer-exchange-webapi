namespace FSH.WebApi.Shared.Multitenancy;

public class MultitenancyConstants
{
    public static class Root
    {
        public const string Id = "root";
        public const string Name = "Root";
        public const string EmailAddress = "admin@root.com";
    }

    public const string DefaultPassword = "SovHM2a!JZXKNGP2it2i";

    public const string TenantIdName = "tenant";
}