using AaryanBillingApp.Business.Interfaces;
using AaryanBillingApp.Data.Interfaces;
using AaryanBillingApp.Data.Repositories;
using AaryanBillingApp.Models.DTOs;
using AaryanBillingApp.Models.Entities;
using AaryanBillingApp.Models.DTOs;

namespace AaryanBillingApp.Business.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IProductVariantRepository _variantRepository;

    public ProductService(
        IProductRepository productRepository,
        IProductVariantRepository variantRepository)
    {
        _productRepository = productRepository;
        _variantRepository = variantRepository;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<IEnumerable<ProductListItemDto>> GetProductListAsync()
    {
        return await _productRepository.GetProductListAsync();
    }

    public async Task<IEnumerable<ProductVariant>> GetVariantsAsync(int productId)
    {
        return await _variantRepository.GetByProductIdAsync(productId);
    }

    public async Task<int> AddProductAsync(Product product, IEnumerable<ProductVariant> variants)
    {
        if (await _productRepository.ExistsByNameAsync(product.Name))
            throw new Exception("Product already exists.");

        var variantList = variants.ToList();

        // Validate duplicate barcodes/SKUs within the request
        if (variantList.GroupBy(v => v.Barcode).Any(g => g.Count() > 1))
            throw new Exception("Duplicate barcode found.");

        if (variantList.GroupBy(v => v.SKU).Any(g => g.Count() > 1))
            throw new Exception("Duplicate SKU found.");

        // Validate against database
        foreach (var variant in variantList)
        {
            if (await _variantRepository.BarcodeExistsAsync(variant.Barcode))
                throw new Exception($"Barcode '{variant.Barcode}' already exists.");

            if (await _variantRepository.SKUExistsAsync(variant.SKU))
                throw new Exception($"SKU '{variant.SKU}' already exists.");
        }

        var productId = await _productRepository.AddAsync(product);

        foreach (var variant in variantList)
        {
            variant.ProductId = productId;
            await _variantRepository.AddAsync(variant);
        }

        return productId;
    }

    public async Task UpdateProductAsync(Product product, IEnumerable<ProductVariant> variants)
    {
        if (await _productRepository.ExistsByNameAsync(product.Name, product.Id))
            throw new Exception("Product already exists.");

        var variantList = variants.ToList();

        if (variantList.GroupBy(v => v.Barcode).Any(g => g.Count() > 1))
            throw new Exception("Duplicate barcode found.");

        if (variantList.GroupBy(v => v.SKU).Any(g => g.Count() > 1))
            throw new Exception("Duplicate SKU found.");

        foreach (var variant in variantList)
        {
            if (await _variantRepository.BarcodeExistsAsync(variant.Barcode, variant.Id))
                throw new Exception($"Barcode '{variant.Barcode}' already exists.");

            if (await _variantRepository.SKUExistsAsync(variant.SKU, variant.Id))
                throw new Exception($"SKU '{variant.SKU}' already exists.");
        }

        await _productRepository.UpdateAsync(product);

        foreach (var variant in variantList)
        {
            variant.ProductId = product.Id;

            if (variant.Id == 0)
            {
                await _variantRepository.AddAsync(variant);
            }
            else
            {
                await _variantRepository.UpdateAsync(variant);
            }
        }
    }

    public async Task DeleteProductAsync(int id)
    {
        var variants = await _variantRepository.GetByProductIdAsync(id);

        foreach (var variant in variants)
        {
            await _variantRepository.DeleteAsync(variant.Id);
        }

        await _productRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Product>> SearchProductsAsync(string searchText)
    {
        var products = await _productRepository.GetAllAsync();

        return products.Where(x =>
                x.Name.Contains(searchText,
                StringComparison.OrdinalIgnoreCase));
    }

    public async Task<ProductDetailsDto?> GetProductByIdAsync(int productId)
    {
        return await _productRepository.GetProductByIdAsync(productId);
    }
}