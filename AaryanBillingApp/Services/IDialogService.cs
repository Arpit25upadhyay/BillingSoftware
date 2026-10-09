using AaryanBillingApp.Models.DTOs;
using AaryanBillingApp.Models.Entities;

namespace AaryanBillingApp.UI.Services;

public interface IDialogService
{
    // Product
    bool ShowAddProductDialog();
    bool ShowEditProductDialog(int productId);

    // Category
    int? ShowAddCategoryDialog();
    bool ShowEditCategoryDialog(Category category);

    // Customer
    bool ShowAddCustomerDialog();
    bool ShowEditCustomerDialog(CustomerListItemDto customer);

    // Supplier
    bool ShowAddSupplierDialog();
    bool ShowEditSupplierDialog(int supplierId);

    // Purchase
    bool ShowAddPurchaseDialog();
    bool ShowEditPurchaseDialog(int purchaseId);

    //Brand
    int? ShowAddBrandDialog();
    
    //Color
    int ShowAddColorDialog();
}