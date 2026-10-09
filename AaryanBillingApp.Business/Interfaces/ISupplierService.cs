using AaryanBillingApp.Models.Dtos;
using AaryanBillingApp.Models.DTOs;
using AaryanBillingApp.Models.Entities;

namespace AaryanBillingApp.Business.Interfaces;

public interface ISupplierService
{
    Task<IEnumerable<Supplier>> GetAllSuppliersAsync();

    Task<IEnumerable<SupplierListItemDto>> GetSupplierListAsync();

    Task<Supplier?> GetSupplierByIdAsync(int id);

    Task<int> AddSupplierAsync(Supplier supplier);

    Task UpdateSupplierAsync(Supplier supplier);

    Task DeleteSupplierAsync(int id);

    Task<IEnumerable<Supplier>> SearchSuppliersAsync(string searchText);
}