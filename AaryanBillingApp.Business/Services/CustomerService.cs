using AaryanBillingApp.Business.Interfaces;
using AaryanBillingApp.Data.Interfaces;
using AaryanBillingApp.Models.Dtos;
using AaryanBillingApp.Models.DTOs;
using AaryanBillingApp.Models.Entities;

namespace AaryanBillingApp.Business.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        return await _customerRepository.GetAllAsync();
    }

    public async Task<IEnumerable<CustomerListItemDto>> GetCustomerListAsync()
    {
        return await _customerRepository.GetCustomerListAsync();
    }

    public async Task<int> AddCustomerAsync(Customer customer)
    {
        if (await _customerRepository.ExistsByCustomerCodeAsync(customer.CustomerCode))
            throw new Exception("Customer Code already exists.");

        if (await _customerRepository.ExistsByMobileAsync(customer.Mobile))
            throw new Exception("Mobile number already exists.");

        if (!string.IsNullOrWhiteSpace(customer.GSTNumber))
        {
            if (await _customerRepository.ExistsByGSTAsync(customer.GSTNumber))
                throw new Exception("GST Number already exists.");
        }

        return await _customerRepository.AddAsync(customer);
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
        if (await _customerRepository.ExistsByCustomerCodeAsync(customer.CustomerCode, customer.Id))
            throw new Exception("Customer Code already exists.");

        if (await _customerRepository.ExistsByMobileAsync(customer.Mobile, customer.Id))
            throw new Exception("Mobile number already exists.");

        if (!string.IsNullOrWhiteSpace(customer.GSTNumber))
        {
            if (await _customerRepository.ExistsByGSTAsync(customer.GSTNumber, customer.Id))
                throw new Exception("GST Number already exists.");
        }

        await _customerRepository.UpdateAsync(customer);
    }

    public async Task DeleteCustomerAsync(int id)
    {
        await _customerRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Customer>> SearchCustomersAsync(string searchText)
    {
        return await _customerRepository.SearchAsync(searchText);
    }

    public async Task<CustomerDetailsDto?> GetCustomerByIdAsync(int customerId)
    {
        return await _customerRepository.GetCustomerByIdAsync(customerId);
    }
}