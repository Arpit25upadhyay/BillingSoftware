using AaryanBillingApp.Models.Entities;

namespace AaryanBillingApp.Data.Interfaces;

public interface IPaymentModeRepository
{
    Task<IEnumerable<PaymentMode>> GetAllAsync();

    Task<PaymentMode?> GetByIdAsync(int id);
}