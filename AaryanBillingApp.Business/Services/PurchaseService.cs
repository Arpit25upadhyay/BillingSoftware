using AaryanBillingApp.Business.Interfaces;
using AaryanBillingApp.Data.Interfaces;
using AaryanBillingApp.Models.DTOs;
using AaryanBillingApp.Models.Entities;

namespace AaryanBillingApp.Business.Services;

public class PurchaseService : IPurchaseService
{
    private readonly IPurchaseRepository _repository;

    public PurchaseService(IPurchaseRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PurchaseListItemDto>> GetPurchaseListAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<PurchaseDetailsDto?> GetPurchaseByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<int> AddPurchaseAsync(
        Purchase purchase,
        IEnumerable<PurchaseItem> items,
        IEnumerable<PurchasePayment> payments)
    {
        if (purchase.SupplierId <= 0)
            throw new Exception("Supplier is required.");

        var itemList = items.ToList();

        if (itemList.Count == 0)
            throw new Exception("At least one product is required.");

        foreach (var item in itemList)
        {
            if (item.ProductVariantId <= 0)
                throw new Exception("Invalid product.");

            if (item.Quantity <= 0)
                throw new Exception("Quantity must be greater than zero.");

            if (item.PurchasePrice < 0)
                throw new Exception("Purchase price cannot be negative.");
        }

        if (purchase.GrandTotal < 0)
            throw new Exception("Grand total cannot be negative.");

        if (purchase.PaidAmount < 0)
            throw new Exception("Paid amount cannot be negative.");

        if (purchase.PaidAmount > purchase.GrandTotal)
            throw new Exception("Paid amount cannot be greater than Grand Total.");

        purchase.DueAmount =
            purchase.GrandTotal - purchase.PaidAmount;

        purchase.Status =
            purchase.DueAmount > 0
                ? "Pending"
                : "Paid";

        return await _repository.AddAsync(
            purchase,
            itemList,
            payments);
    }

    public async Task DeletePurchaseAsync(int id)
    {
        if (id <= 0)
            throw new Exception("Invalid purchase.");

        await _repository.DeleteAsync(id);
    }
}