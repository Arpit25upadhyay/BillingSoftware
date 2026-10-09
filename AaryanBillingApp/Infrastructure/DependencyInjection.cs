using AaryanBillingApp.Business.Interfaces;
using AaryanBillingApp.Business.Services;
using AaryanBillingApp.Data.Interfaces;
using AaryanBillingApp.Data.Repositories;
using AaryanBillingApp.UI.Navigation;
using AaryanBillingApp.UI.Services;
using AaryanBillingApp.UI.ViewModels.Brands;
using AaryanBillingApp.UI.ViewModels.Categories;
using AaryanBillingApp.UI.ViewModels.Customers;
using AaryanBillingApp.UI.ViewModels.Dashboard;
using AaryanBillingApp.UI.ViewModels.Products;
using AaryanBillingApp.UI.ViewModels.Purchases;
using AaryanBillingApp.UI.ViewModels.Reports;
using AaryanBillingApp.UI.ViewModels.Settings;
using AaryanBillingApp.UI.ViewModels.Shell;
using AaryanBillingApp.UI.ViewModels.Suppliers;
using AaryanBillingApp.UI.Views.Brands;
using AaryanBillingApp.UI.Views.Categories;
using AaryanBillingApp.UI.Views.Customers;
using AaryanBillingApp.UI.Views.Products;
using AaryanBillingApp.UI.Views.Settings;
using AaryanBillingApp.UI.Views.Shell;
using AaryanBillingApp.UI.Views.Suppliers;
using Microsoft.Extensions.DependencyInjection;

namespace AaryanBillingApp.UI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        // ----------------------------
        // Database
        // ----------------------------

        services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

        // ----------------------------
        // Navigation
        // ----------------------------

        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IDialogService, DialogService>();

        // ----------------------------
        // Dialogs
        // ----------------------------

        services.AddSingleton<IDialogService, DialogService>();

        // ----------------------------
        // Repositories
        // ----------------------------
        
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<IColorRepository, ColorRepository>();
        services.AddScoped<IUnitRepository, UnitRepository>();
        services.AddScoped<ISizeRepository, SizeRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddSingleton<ICustomerRepository, CustomerRepository>();
        services.AddSingleton<ISupplierRepository, SupplierRepository>();
        services.AddScoped<IPaymentModeRepository, PaymentModeRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<IUnitRepository, UnitRepository>();
        services.AddScoped<IColorRepository, ColorRepository>();
        services.AddScoped<ISizeRepository, SizeRepository>();
        services.AddScoped<IPaymentModeRepository, PaymentModeRepository>();
        services.AddTransient<IPurchaseRepository, PurchaseRepository>();
        services.AddScoped<IReportRepository, ReportRepository>();
        services.AddScoped<ISettingRepository, SettingRepository>();

        // ----------------------------
        // Services
        // ----------------------------

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IMasterDataService, MasterDataService>();
        services.AddSingleton<ICustomerService, CustomerService>();
        services.AddSingleton<ISupplierService, SupplierService>();
        services.AddTransient<IPurchaseService, PurchaseService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<ISettingService, SettingsService>();
        services.AddScoped<IBrandService, BrandService>();
        services.AddScoped<IColorService, ColorService>();

        // ----------------------------
        // ViewModels
        // ----------------------------

        services.AddSingleton<ShellViewModel>();
        services.AddTransient<DashboardViewModel>();
        services.AddTransient<ProductListViewModel>();
        services.AddTransient<AddProductViewModel>();
        services.AddTransient<CategoryListViewModel>();
        services.AddTransient<AddCategoryViewModel>();
        services.AddTransient<ProductListViewModel>();
        services.AddTransient<AddProductViewModel>();
        services.AddTransient<CustomerListViewModel>();
        services.AddTransient<AddCustomerViewModel>();
        services.AddTransient<SupplierListViewModel>();
        services.AddTransient<AddSupplierViewModel>();
        services.AddTransient<PurchaseListViewModel>();
        services.AddTransient<AddPurchaseViewModel>();
        services.AddTransient<ReportsViewModel>();
        services.AddTransient<SettingsViewModel>();
        services.AddTransient<AddBrandViewModel>();
        services.AddTransient<AddColorViewModel>();


        // ----------------------------
        // Views
        // ----------------------------

        services.AddSingleton<ShellView>();
        services.AddTransient<ProductListView>();
        services.AddTransient<AddProductWindow>();
        services.AddTransient<CategoryListView>();
        services.AddTransient<AddCategoryWindow>();
        services.AddTransient<CustomerListView>();
        services.AddTransient<AddCustomerWindow>();
        services.AddTransient<SupplierListView>();
        services.AddTransient<AddSupplierWindow>();
        services.AddTransient<SettingsView>();
        services.AddTransient<AddBrandWindow>();
        services.AddTransient<AddColorWindow>();


        return services;
    }
}