using CS.Core.Models.Entities;

namespace CS.Core.Repositories.ProductRepository
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);

        Task<Product?> GetByIdAsync(int id);

        Task<IEnumerable<Product>> GetAllAsync();

        Task UpdateAsync(Product product);

        Task DeleteAsync(int id);

        Task UpdateStockAsync(int id, int quantity, bool isIncrement);
    }
}
