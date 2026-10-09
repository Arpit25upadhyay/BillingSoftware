using AaryanBillingApp.Business.Interfaces;
using AaryanBillingApp.Models.Entities;
using AaryanBillingApp.UI.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AaryanBillingApp.UI.ViewModels.Suppliers;

public partial class AddSupplierViewModel : ViewModelBase
{
    private readonly ISupplierService _supplierService;

    public AddSupplierViewModel(ISupplierService supplierService)
    {
        _supplierService = supplierService;

        Supplier = new Supplier
        {
            SupplierCode = GenerateSupplierCode(),
            IsActive = true,
            OpeningBalance = 0,
            PaymentDays = 0
        };
    }

    [ObservableProperty]
    private Supplier supplier;

    [ObservableProperty]
    private bool isEditMode;

    [ObservableProperty]
    private int editingSupplierId;

    public Action<bool?>? CloseAction { get; set; }

    [RelayCommand]
    private async Task Save()
    {
        Validate();

        if (IsEditMode)
        {
            await _supplierService.UpdateSupplierAsync(Supplier);
        }
        else
        {
            await _supplierService.AddSupplierAsync(Supplier);
        }

        CloseAction?.Invoke(true);
    }

    [RelayCommand]
    private void Cancel()
    {
        CloseAction?.Invoke(false);
    }

    private void Validate()
    {
        Supplier.CompanyName = Supplier.CompanyName?.Trim() ?? string.Empty;
        Supplier.ContactPerson = Supplier.ContactPerson?.Trim() ?? string.Empty;
        Supplier.Mobile = Supplier.Mobile?.Trim() ?? string.Empty;
        Supplier.AlternateMobile = Supplier.AlternateMobile?.Trim();
        Supplier.Email = Supplier.Email?.Trim();
        Supplier.GSTNumber = Supplier.GSTNumber?.Trim();
        Supplier.PANNumber = Supplier.PANNumber?.Trim();

        if (string.IsNullOrWhiteSpace(Supplier.CompanyName))
            throw new Exception("Company Name is required.");

        if (string.IsNullOrWhiteSpace(Supplier.Mobile))
            throw new Exception("Mobile Number is required.");

        if (!IsValidMobile(Supplier.Mobile))
            throw new Exception("Invalid Mobile Number.");

        if (!string.IsNullOrWhiteSpace(Supplier.AlternateMobile))
        {
            if (!IsValidMobile(Supplier.AlternateMobile))
                throw new Exception("Invalid Alternate Mobile Number.");
        }

        if (!string.IsNullOrWhiteSpace(Supplier.Email))
        {
            try
            {
                _ = new System.Net.Mail.MailAddress(Supplier.Email);
            }
            catch
            {
                throw new Exception("Invalid Email Address.");
            }
        }

        if (!string.IsNullOrWhiteSpace(Supplier.GSTNumber))
        {
            if (!IsValidGST(Supplier.GSTNumber))
                throw new Exception("Invalid GST Number.");
        }

        if (Supplier.OpeningBalance < 0)
            throw new Exception("Opening Balance cannot be negative.");

        if (Supplier.PaymentDays < 0)
            throw new Exception("Payment Days cannot be negative.");
    }

    private static bool IsValidMobile(string mobile)
    {
        return mobile.Length == 10 &&
               mobile.All(char.IsDigit);
    }

    private static bool IsValidGST(string gst)
    {
        return gst.Length == 15 &&
               gst.All(char.IsLetterOrDigit);
    }

    private static string GenerateSupplierCode()
    {
        return $"SUP-{DateTime.Now:yyyyMMddHHmmss}";
    }

    public async Task InitializeForEditAsync(int supplierId)
    {
        IsEditMode = true;
        EditingSupplierId = supplierId;

        var supplier = await _supplierService.GetSupplierByIdAsync(supplierId);

        if (supplier == null)
            return;

        Supplier = new Supplier
        {
            Id = supplier.Id,
            SupplierCode = supplier.SupplierCode,
            CompanyName = supplier.CompanyName,
            ContactPerson = supplier.ContactPerson,
            Mobile = supplier.Mobile,
            AlternateMobile = supplier.AlternateMobile,
            Email = supplier.Email,
            GSTNumber = supplier.GSTNumber,
            PANNumber = supplier.PANNumber,
            Address = supplier.Address,
            City = supplier.City,
            State = supplier.State,
            Pincode = supplier.Pincode,
            OpeningBalance = supplier.OpeningBalance,
            PaymentDays = supplier.PaymentDays,
            Remarks = supplier.Remarks,
            IsActive = supplier.IsActive
        };
    }
}