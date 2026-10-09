using AaryanBillingApp.Business.Interfaces;
using AaryanBillingApp.Data.Interfaces;
using AaryanBillingApp.Models.Dtos;
using AaryanBillingApp.Models.DTOs;
using AaryanBillingApp.Models.Entities;

namespace AaryanBillingApp.Business.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _supplierRepository;

    public SupplierService(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<IEnumerable<Supplier>> GetAllSuppliersAsync()
    {
        return await _supplierRepository.GetAllAsync();
    }

    public async Task<IEnumerable<SupplierListItemDto>> GetSupplierListAsync()
    {
        return await _supplierRepository.GetSupplierListAsync();
    }

    public async Task<Supplier?> GetSupplierByIdAsync(int id)
    {
        return await _supplierRepository.GetByIdAsync(id);
    }

    public async Task<int> AddSupplierAsync(Supplier supplier)
    {
        if (await _supplierRepository.SupplierExistsAsync(supplier.CompanyName))
            throw new Exception("Supplier already exists.");

        return await _supplierRepository.AddAsync(supplier);
    }

    public async Task UpdateSupplierAsync(Supplier supplier)
    {
        if (await _supplierRepository.SupplierExistsAsync(
                supplier.CompanyName,
                supplier.Id))
        {
            throw new Exception("Supplier already exists.");
        }

        await _supplierRepository.UpdateAsync(supplier);
    }

    public async Task DeleteSupplierAsync(int id)
    {
        await _supplierRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Supplier>> SearchSuppliersAsync(string searchText)
    {
        var suppliers = await _supplierRepository.GetAllAsync();

        return suppliers.Where(x =>
            x.CompanyName.Contains(searchText,
                StringComparison.OrdinalIgnoreCase)
            || x.ContactPerson.Contains(searchText,
                StringComparison.OrdinalIgnoreCase)
            || x.Mobile.Contains(searchText,
                StringComparison.OrdinalIgnoreCase)
            || x.SupplierCode.Contains(searchText,
                StringComparison.OrdinalIgnoreCase));
    }
}