using AaryanBillingApp.Models.DTOs;
using AaryanBillingApp.Models.Entities;

namespace AaryanBillingApp.Business.Interfaces;

public interface IPurchaseService
{
    Task<IEnumerable<PurchaseListItemDto>> GetPurchaseListAsync();

    Task<PurchaseDetailsDto?> GetPurchaseByIdAsync(int id);

    Task<int> AddPurchaseAsync(
        Purchase purchase,
        IEnumerable<PurchaseItem> items,
        IEnumerable<PurchasePayment> payments);

    Task DeletePurchaseAsync(int id);
}