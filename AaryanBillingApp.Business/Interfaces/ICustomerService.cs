using AaryanBillingApp.Models.Dtos;
using AaryanBillingApp.Models.DTOs;
using AaryanBillingApp.Models.Entities;

namespace AaryanBillingApp.Business.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<Customer>> GetAllCustomersAsync();

    Task<IEnumerable<CustomerListItemDto>> GetCustomerListAsync();

    Task<int> AddCustomerAsync(Customer customer);

    Task UpdateCustomerAsync(Customer customer);

    Task DeleteCustomerAsync(int id);

    Task<IEnumerable<Customer>> SearchCustomersAsync(string searchText);

    Task<CustomerDetailsDto?> GetCustomerByIdAsync(int customerId);
}