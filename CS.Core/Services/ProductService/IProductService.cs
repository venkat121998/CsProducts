using CS.Core.Models.Dto;

namespace CS.Core.Services.ProductService
{
    public interface IProductService
    {
        Task AddProductAsync(ProductDto product);

        Task<ProductDto?> GetProductByIdAsync(int id);

        Task<IEnumerable<ProductDto>> GetProductsAsync();

        Task UpdateProductAsync(ProductDto product);

        Task DeleteProductAsync(int id);

        Task DecrementStockAsync(int id, int quantity);

        Task IncrementStockAsync(int id, int quantity);
    }
}
