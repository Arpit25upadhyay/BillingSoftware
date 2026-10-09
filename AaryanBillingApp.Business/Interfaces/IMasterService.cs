using AaryanBillingApp.Models.Entities;

namespace AaryanBillingApp.Business.Interfaces;

public interface IMasterDataService
{
    Task<IEnumerable<Category>> GetCategoriesAsync();

    Task<IEnumerable<Brand>> GetBrandsAsync();

    Task<IEnumerable<Units>> GetUnitsAsync();

    Task<IEnumerable<Color>> GetColorsAsync();

    Task<IEnumerable<Size>> GetSizesAsync();
    Task<IEnumerable<PaymentMode>> GetPaymentModesAsync();
}