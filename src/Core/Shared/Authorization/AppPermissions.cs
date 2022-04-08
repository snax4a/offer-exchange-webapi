using System.Collections.ObjectModel;

namespace FSH.WebApi.Shared.Authorization;

public static class ResourceAction
{
    public const string View = nameof(View);
    public const string Search = nameof(Search);
    public const string Create = nameof(Create);
    public const string Update = nameof(Update);
    public const string Delete = nameof(Delete);
    public const string Export = nameof(Export);
    public const string Generate = nameof(Generate);
    public const string Clean = nameof(Clean);
    public const string UpgradeSubscription = nameof(UpgradeSubscription);
}

public static class Resource
{
    public const string Tenants = nameof(Tenants);
    public const string Dashboard = nameof(Dashboard);
    public const string Hangfire = nameof(Hangfire);
    public const string Users = nameof(Users);
    public const string UserRoles = nameof(UserRoles);
    public const string Roles = nameof(Roles);
    public const string RoleClaims = nameof(RoleClaims);
    public const string Products = nameof(Products);
    public const string Brands = nameof(Brands);
    public const string Groups = nameof(Groups);
    public const string Traders = nameof(Traders);
    public const string Inquiries = nameof(Inquiries);
    public const string Offers = nameof(Offers);
    public const string Orders = nameof(Orders);
}

public static class AppPermissions
{
    private static readonly AppPermission[] _all = new AppPermission[]
    {
        new("View Dashboard", ResourceAction.View, Resource.Dashboard),
        new("View Hangfire", ResourceAction.View, Resource.Hangfire),
        new("View Users", ResourceAction.View, Resource.Users),
        new("Search Users", ResourceAction.Search, Resource.Users),
        new("Create Users", ResourceAction.Create, Resource.Users),
        new("Update Users", ResourceAction.Update, Resource.Users),
        new("Delete Users", ResourceAction.Delete, Resource.Users),
        new("Export Users", ResourceAction.Export, Resource.Users),
        new("View UserRoles", ResourceAction.View, Resource.UserRoles),
        new("Update UserRoles", ResourceAction.Update, Resource.UserRoles),
        new("View Roles", ResourceAction.View, Resource.Roles),
        new("Create Roles", ResourceAction.Create, Resource.Roles),
        new("Update Roles", ResourceAction.Update, Resource.Roles),
        new("Delete Roles", ResourceAction.Delete, Resource.Roles),
        new("View RoleClaims", ResourceAction.View, Resource.RoleClaims),
        new("Update RoleClaims", ResourceAction.Update, Resource.RoleClaims),
        new("View Products", ResourceAction.View, Resource.Products, IsBasic: true),
        new("Search Products", ResourceAction.Search, Resource.Products, IsBasic: true),
        new("Create Products", ResourceAction.Create, Resource.Products),
        new("Update Products", ResourceAction.Update, Resource.Products),
        new("Delete Products", ResourceAction.Delete, Resource.Products),
        new("Export Products", ResourceAction.Export, Resource.Products),
        new("View Brands", ResourceAction.View, Resource.Brands, IsBasic: true),
        new("Search Brands", ResourceAction.Search, Resource.Brands, IsBasic: true),
        new("Create Brands", ResourceAction.Create, Resource.Brands),
        new("Update Brands", ResourceAction.Update, Resource.Brands),
        new("Delete Brands", ResourceAction.Delete, Resource.Brands),
        new("Generate Brands", ResourceAction.Generate, Resource.Brands),
        new("Clean Brands", ResourceAction.Clean, Resource.Brands),
        new("Search Groups", ResourceAction.Search, Resource.Groups, IsBasic: true),
        new("View Groups", ResourceAction.View, Resource.Groups, IsBasic: true),
        new("Create Groups", ResourceAction.Create, Resource.Groups, IsBasic: true),
        new("Update Groups", ResourceAction.Update, Resource.Groups, IsBasic: true),
        new("Delete Groups", ResourceAction.Delete, Resource.Groups, IsBasic: true),
        new("Search Traders", ResourceAction.Search, Resource.Traders, IsBasic: true),
        new("View Traders", ResourceAction.View, Resource.Traders, IsBasic: true),
        new("Create Traders", ResourceAction.Create, Resource.Traders, IsBasic: true),
        new("Update Traders", ResourceAction.Update, Resource.Traders, IsBasic: true),
        new("Delete Traders", ResourceAction.Delete, Resource.Traders, IsBasic: true),
        new("Search Inquiries", ResourceAction.Search, Resource.Inquiries, IsBasic: true),
        new("View Inquiries", ResourceAction.View, Resource.Inquiries, IsBasic: true),
        new("Create Inquiries", ResourceAction.Create, Resource.Inquiries, IsBasic: true),
        new("Search Offers", ResourceAction.Search, Resource.Offers, IsBasic: true),
        new("View Offers", ResourceAction.View, Resource.Offers, IsBasic: true),
        new("Search Orders", ResourceAction.Search, Resource.Orders, IsBasic: true),
        new("View Orders", ResourceAction.View, Resource.Orders, IsBasic: true),
        new("Create Orders", ResourceAction.Create, Resource.Orders, IsBasic: true),
        new("View Tenants", ResourceAction.View, Resource.Tenants, IsRoot: true),
        new("Create Tenants", ResourceAction.Create, Resource.Tenants, IsRoot: true),
        new("Update Tenants", ResourceAction.Update, Resource.Tenants, IsRoot: true),
        new("Upgrade Tenant Subscription", ResourceAction.UpgradeSubscription, Resource.Tenants, IsRoot: true)
    };

    public static IReadOnlyList<AppPermission> All { get; } = new ReadOnlyCollection<AppPermission>(_all);
    public static IReadOnlyList<AppPermission> Root { get; } = new ReadOnlyCollection<AppPermission>(_all.Where(p => p.IsRoot).ToArray());
    public static IReadOnlyList<AppPermission> Admin { get; } = new ReadOnlyCollection<AppPermission>(_all.Where(p => !p.IsRoot).ToArray());
    public static IReadOnlyList<AppPermission> Basic { get; } = new ReadOnlyCollection<AppPermission>(_all.Where(p => p.IsBasic).ToArray());
}

public record AppPermission(string Description, string Action, string Resource, bool IsBasic = false, bool IsRoot = false)
{
    public string Name => NameFor(Action, Resource);
    public static string NameFor(string action, string resource) => $"Permissions.{resource}.{action}";
}