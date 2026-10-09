using AaryanBillingApp.Models.Dtos;
using AaryanBillingApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AaryanBillingApp.Data.Interfaces
{
    public interface ISupplierRepository
    {
        Task<IEnumerable<Supplier>> GetAllAsync();

        Task<IEnumerable<SupplierListItemDto>> GetSupplierListAsync();

        Task<Supplier?> GetByIdAsync(int id);

        Task<int> AddAsync(Supplier supplier);

        Task UpdateAsync(Supplier supplier);

        Task DeleteAsync(int id);

        Task<bool> SupplierExistsAsync(
            string companyName,
            int? excludeId = null);
    }
}
