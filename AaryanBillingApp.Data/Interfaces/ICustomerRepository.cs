using AaryanBillingApp.Models.Dtos;
using AaryanBillingApp.Models.DTOs;
using AaryanBillingApp.Models.Entities;

namespace AaryanBillingApp.Data.Interfaces;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetAllAsync();

    Task<IEnumerable<CustomerListItemDto>> GetCustomerListAsync();

    Task<Customer?> GetByIdAsync(int id);

    Task<int> AddAsync(Customer customer);

    Task UpdateAsync(Customer customer);

    Task DeleteAsync(int id);

    Task<bool> ExistsByCustomerCodeAsync(string customerCode, int? excludeId = null);

    Task<bool> ExistsByMobileAsync(string mobile, int? excludeId = null);

    Task<bool> ExistsByGSTAsync(string? gstNumber, int? excludeId = null);

    Task<IEnumerable<Customer>> SearchAsync(string searchText);

    Task<CustomerDetailsDto?> GetCustomerByIdAsync(int customerId);
}