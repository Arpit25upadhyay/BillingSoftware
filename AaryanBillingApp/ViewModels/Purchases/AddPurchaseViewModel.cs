using AaryanBillingApp.Business.Interfaces;
using AaryanBillingApp.Models.Entities;
using AaryanBillingApp.UI.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace AaryanBillingApp.UI.ViewModels.Purchases;

public partial class AddPurchaseViewModel : ViewModelBase
{
    private readonly IPurchaseService _purchaseService;
    private readonly ISupplierService _supplierService;
    private readonly IMasterDataService _masterDataService;

    public AddPurchaseViewModel(
        IPurchaseService purchaseService,
        ISupplierService supplierService,
        IMasterDataService masterDataService)
    {
        _purchaseService = purchaseService;
        _supplierService = supplierService;
        _masterDataService = masterDataService;

        Purchase = new Purchase
        {
            PurchaseNo = GeneratePurchaseNumber(),
            PurchaseDate = DateTime.Today,
            InvoiceDate = DateTime.Today,
            Status = "Pending",
            UserId = 1
        };

        Items = new ObservableCollection<PurchaseItem>();
        Payments = new ObservableCollection<PurchasePayment>();
    }

    [ObservableProperty]
    private Purchase purchase;

    public ObservableCollection<PurchaseItem> Items { get; }

    public ObservableCollection<PurchasePayment> Payments { get; }

    public ObservableCollection<Supplier> Suppliers { get; } = new();

    public ObservableCollection<PaymentMode> PaymentModes { get; } = new();

    [ObservableProperty]
    private Supplier? selectedSupplier;

    [ObservableProperty]
    private PaymentMode? selectedPaymentMode;

    [ObservableProperty]
    private decimal paymentAmount;

    [ObservableProperty]
    private string? transactionNumber;

    public Action<bool?>? CloseAction { get; set; }


    // ==============================
    // LOAD SUPPLIERS / PAYMENT MODES
    // ==============================

    public async Task LoadMastersAsync()
    {
        Suppliers.Clear();

        var suppliers = await _supplierService.GetAllSuppliersAsync();

        foreach (var supplier in suppliers)
        {
            Suppliers.Add(supplier);
        }

        PaymentModes.Clear();

        var paymentModes =
            await _masterDataService.GetPaymentModesAsync();

        foreach (var mode in paymentModes)
        {
            PaymentModes.Add(mode);
        }
    }


    // ==============================
    // ADD ITEM
    // ==============================

    public void AddItem(
        int productVariantId,
        decimal quantity,
        decimal purchasePrice,
        decimal sellingPrice,
        decimal discountAmount,
        decimal gstPercentage)
    {
        if (productVariantId <= 0)
            throw new Exception("Product is required.");

        if (quantity <= 0)
            throw new Exception("Quantity must be greater than zero.");

        if (purchasePrice < 0)
            throw new Exception("Purchase price cannot be negative.");

        var taxableAmount =
            (purchasePrice * quantity) - discountAmount;

        if (taxableAmount < 0)
            taxableAmount = 0;

        var gstAmount =
            taxableAmount * gstPercentage / 100m;

        var lineTotal =
            taxableAmount + gstAmount;

        var item = new PurchaseItem
        {
            ProductVariantId = productVariantId,
            Quantity = quantity,
            PurchasePrice = purchasePrice,
            SellingPrice = sellingPrice,
            DiscountAmount = discountAmount,
            GSTPercentage = gstPercentage,
            GSTAmount = gstAmount,
            LineTotal = lineTotal
        };

        Items.Add(item);

        CalculateTotals();
    }


    // ==============================
    // REMOVE ITEM
    // ==============================

    [RelayCommand]
    private void RemoveItem(PurchaseItem? item)
    {
        if (item == null)
            return;

        Items.Remove(item);

        CalculateTotals();
    }


    // ==============================
    // ADD PAYMENT
    // ==============================

    [RelayCommand]
    private void AddPayment()
    {
        if (SelectedPaymentMode == null)
            throw new Exception("Payment mode is required.");

        if (PaymentAmount <= 0)
            throw new Exception(
                "Payment amount must be greater than zero.");

        CalculateTotals();

        if (PaymentAmount > Purchase.DueAmount)
            throw new Exception(
                "Payment amount cannot be greater than due amount.");

        var payment = new PurchasePayment
        {
            PurchaseId = Purchase.Id,
            PaymentModeId = SelectedPaymentMode.Id,
            Amount = PaymentAmount,
            TransactionNumber = TransactionNumber,
            PaymentDate = DateTime.Now
        };

        Payments.Add(payment);

        Purchase.PaidAmount += PaymentAmount;

        PaymentAmount = 0;
        TransactionNumber = string.Empty;

        CalculateTotals();
    }


    // ==============================
    // SAVE
    // ==============================

    [RelayCommand]
    private async Task Save()
    {
        if (SelectedSupplier == null)
            throw new Exception("Supplier is required.");

        if (Items.Count == 0)
            throw new Exception(
                "Add at least one product.");

        Purchase.SupplierId = SelectedSupplier.Id;

        CalculateTotals();

        if (Purchase.PaidAmount > Purchase.GrandTotal)
            throw new Exception(
                "Paid amount cannot be greater than Grand Total.");

        Purchase.DueAmount =
            Purchase.GrandTotal - Purchase.PaidAmount;

        Purchase.Status =
            Purchase.DueAmount > 0
                ? "Pending"
                : "Paid";

        await _purchaseService.AddPurchaseAsync(
            Purchase,
            Items,
            Payments);

        CloseAction?.Invoke(true);
    }


    // ==============================
    // CANCEL
    // ==============================

    [RelayCommand]
    private void Cancel()
    {
        CloseAction?.Invoke(false);
    }


    // ==============================
    // CALCULATE TOTALS
    // ==============================

    private void CalculateTotals()
    {
        Purchase.SubTotal =
            Items.Sum(x =>
                x.Quantity * x.PurchasePrice);

        Purchase.DiscountAmount =
            Items.Sum(x => x.DiscountAmount);

        Purchase.TaxAmount =
            Items.Sum(x => x.GSTAmount);

        Purchase.GrandTotal =
            Purchase.SubTotal
            - Purchase.DiscountAmount
            + Purchase.TaxAmount
            + Purchase.RoundOff;

        Purchase.DueAmount =
            Purchase.GrandTotal
            - Purchase.PaidAmount;
    }


    // ==============================
    // PURCHASE NUMBER
    // ==============================

    private static string GeneratePurchaseNumber()
    {
        return $"PUR-{DateTime.Now:yyyyMMddHHmmss}";
    }

    public async Task InitializeForEditAsync(int purchaseId)
    {
        if (purchaseId <= 0)
            throw new ArgumentException(
                "Invalid purchase id.",
                nameof(purchaseId));

        // Load suppliers and payment modes first
        await LoadMastersAsync();

        // Get purchase details
        var purchaseDetails =
            await _purchaseService.GetPurchaseByIdAsync(purchaseId);

        if (purchaseDetails == null)
            throw new Exception("Purchase not found.");

        // -----------------------------
        // PURCHASE
        // -----------------------------

        Purchase = new Purchase
        {
            Id = purchaseDetails.Id,
            PurchaseNo = purchaseDetails.PurchaseNo,
            SupplierId = purchaseDetails.SupplierId,
            UserId = purchaseDetails.UserId,
            InvoiceNumber = purchaseDetails.InvoiceNumber,
            InvoiceDate = purchaseDetails.InvoiceDate,
            PurchaseDate = purchaseDetails.PurchaseDate,

            SubTotal = purchaseDetails.SubTotal,
            DiscountAmount = purchaseDetails.DiscountAmount,
            TaxAmount = purchaseDetails.TaxAmount,
            RoundOff = purchaseDetails.RoundOff,
            GrandTotal = purchaseDetails.GrandTotal,

            PaidAmount = purchaseDetails.PaidAmount,
            DueAmount = purchaseDetails.DueAmount,

            Status = purchaseDetails.Status,
            Remarks = purchaseDetails.Remarks
        };

        // -----------------------------
        // SUPPLIER
        // -----------------------------

        SelectedSupplier =
            Suppliers.FirstOrDefault(x =>
                x.Id == Purchase.SupplierId);

        // -----------------------------
        // ITEMS
        // -----------------------------

        Items.Clear();

        foreach (var item in purchaseDetails.Items)
        {
            Items.Add(new PurchaseItem
            {
                Id = item.Id,
                PurchaseId = Purchase.Id,

                ProductVariantId = item.ProductVariantId,
                Quantity = item.Quantity,

                PurchasePrice = item.PurchasePrice,
                SellingPrice = item.SellingPrice,

                DiscountAmount = item.DiscountAmount,

                GSTPercentage = item.GSTPercentage,
                GSTAmount = item.GSTAmount,

                LineTotal = item.LineTotal
            });
        }

        // -----------------------------
        // PAYMENTS
        // -----------------------------

        Payments.Clear();

        foreach (var payment in purchaseDetails.Payments)
        {
            Payments.Add(new PurchasePayment
            {
                Id = payment.Id,
                PurchaseId = Purchase.Id,

                PaymentModeId = payment.PaymentModeId,
                Amount = payment.Amount,

                TransactionNumber = payment.TransactionNumber,
                PaymentDate = payment.PaymentDate,
                Remarks = payment.Remarks
            });
        }

        CalculateTotals();
    }
}