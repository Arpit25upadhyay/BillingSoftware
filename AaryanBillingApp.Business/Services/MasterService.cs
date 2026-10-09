using AaryanBillingApp.Business.Interfaces;
using AaryanBillingApp.Data.Interfaces;
using AaryanBillingApp.Models.Entities;

namespace AaryanBillingApp.Business.Services;

public class MasterDataService : IMasterDataService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly IUnitRepository _unitRepository;
    private readonly IColorRepository _colorRepository;
    private readonly ISizeRepository _sizeRepository;
    private readonly IPaymentModeRepository _paymentModeRepository;

    public MasterDataService(
        ICategoryRepository categoryRepository,
        IBrandRepository brandRepository,
        IUnitRepository unitRepository,
        IColorRepository colorRepository,
        ISizeRepository sizeRepository,
        IPaymentModeRepository paymentModeRepository)
    {
        _categoryRepository = categoryRepository;
        _brandRepository = brandRepository;
        _unitRepository = unitRepository;
        _colorRepository = colorRepository;
        _sizeRepository = sizeRepository;
        _paymentModeRepository = paymentModeRepository;
    }

    public Task<IEnumerable<Category>> GetCategoriesAsync()
        => _categoryRepository.GetAllAsync();

    public Task<IEnumerable<Brand>> GetBrandsAsync()
        => _brandRepository.GetAllAsync();

    public Task<IEnumerable<Units>> GetUnitsAsync()
        => _unitRepository.GetAllAsync();

    public Task<IEnumerable<Color>> GetColorsAsync()
        => _colorRepository.GetAllAsync();

    public Task<IEnumerable<Size>> GetSizesAsync()
        => _sizeRepository.GetAllAsync();

    public Task<IEnumerable<PaymentMode>> GetPaymentModesAsync()
    => _paymentModeRepository.GetAllAsync();
}