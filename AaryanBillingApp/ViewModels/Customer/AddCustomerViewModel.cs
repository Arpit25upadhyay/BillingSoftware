using AaryanBillingApp.Business.Interfaces;
using AaryanBillingApp.Models.Entities;
using AaryanBillingApp.UI.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;

namespace AaryanBillingApp.UI.ViewModels.Customers;

public partial class AddCustomerViewModel : ViewModelBase
{
    private readonly ICustomerService _customerService;

    public AddCustomerViewModel(ICustomerService customerService)
    {
        _customerService = customerService;

        Customer = new Customer
        {
            CustomerCode = GenerateCustomerCode(),
            IsActive = true,
            CreditLimit = 0,
            OpeningBalance = 0,
            LoyaltyPoints = 0
        };
    }

    [ObservableProperty]
    private Customer customer;

    [ObservableProperty]
    private bool isEditMode;

    [ObservableProperty]
    private int editingCustomerId;

    public Action<bool?>? CloseAction { get; set; }

    public ObservableCollection<string> Genders { get; } =
    [
        "Male",
        "Female",
        "Other"
    ];

    [RelayCommand]
    private async Task Save()
    {
        try
        {
            Validate();

            if (IsEditMode)
                await _customerService.UpdateCustomerAsync(Customer);
            else
                await _customerService.AddCustomerAsync(Customer);

            CloseAction?.Invoke(true);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        CloseAction?.Invoke(false);
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

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Customer.Name))
            throw new Exception("Customer Name is required.");

        if (string.IsNullOrWhiteSpace(Customer.Mobile))
            throw new Exception("Mobile Number is required.");

        if (!IsValidMobile(Customer.Mobile))
            throw new Exception("Invalid Mobile Number.");

        if (!string.IsNullOrWhiteSpace(Customer.GSTNumber))
        {
            if (!IsValidGST(Customer.GSTNumber))
                throw new Exception("Invalid GST Number.");
        }

        if (!string.IsNullOrWhiteSpace(Customer.Email))
        {
            try
            {
                _ = new System.Net.Mail.MailAddress(Customer.Email);
            }
            catch
            {
                throw new Exception("Invalid Email Address.");
            }
        }

        if (Customer.CreditLimit < 0)
            throw new Exception("Credit Limit cannot be negative.");

        if (Customer.OpeningBalance < 0)
            throw new Exception("Opening Balance cannot be negative.");
    }

    private static string GenerateCustomerCode()
    {
        return $"CUST-{DateTime.Now:yyyyMMddHHmmss}";
    }

    public async Task InitializeForEditAsync(int customerId)
    {
        IsEditMode = true;
        EditingCustomerId = customerId;

        var customer = await _customerService.GetCustomerByIdAsync(customerId);

        if (customer == null)
            return;

        Customer = new Customer
        {
            Id = customer.Customer.Id,

            CustomerCode = customer.Customer.CustomerCode,

            Name = Customer.Name.Trim(),

            Mobile = Customer.Mobile.Trim(),

            Email = Customer.Email?.Trim(),

            GSTNumber = Customer.GSTNumber?.Trim(),

            Gender = customer.Customer.Gender,

            DateOfBirth = customer.Customer.DateOfBirth,

            Address = customer.Customer.Address,

            City = customer.Customer.City,

            State = customer.Customer.State,

            Pincode = customer.Customer.Pincode,

            CreditLimit = customer.Customer.CreditLimit,

            OpeningBalance = customer.Customer.OpeningBalance,

            LoyaltyPoints = customer.Customer.LoyaltyPoints,

            IsActive = customer.Customer.IsActive
        };
    }
}