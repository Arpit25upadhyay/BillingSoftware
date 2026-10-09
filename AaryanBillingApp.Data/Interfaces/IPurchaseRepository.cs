using AaryanBillingApp.Models.DTOs;
using AaryanBillingApp.Models.Entities;

namespace AaryanBillingApp.Data.Interfaces;

public interface IPurchaseRepository
{
    Task<IEnumerable<PurchaseListItemDto>> GetAllAsync();

    Task<PurchaseDetailsDto?> GetByIdAsync(int id);

    Task<int> AddAsync(
        Purchase purchase,
        IEnumerable<PurchaseItem> items,
        IEnumerable<PurchasePayment> payments);

    Task DeleteAsync(int id);
}