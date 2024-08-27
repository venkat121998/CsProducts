using CS.Core.Helper;
using CS.Core.Models.Dto;
using CS.Core.Models.Entities;
using CS.Core.Repositories.ProductRepository;

namespace CS.Core.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task AddProductAsync(ProductDto productDto)
        {
            productDto.Id = GetDistributedUniqueId();
            var product = ConvertToProduct(productDto);

            if (product == null)
                throw new ArgumentException("Cannot add empty product");

            await _repository.AddAsync(product);
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            return ConvertToProductDto(await _repository.GetByIdAsync(id));
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var products = await _repository.GetAllAsync();

            List<ProductDto> productDtos = new();

            foreach (var product in products)
            {
                var productDto = ConvertToProductDto(product);

                if(productDto != null)
                    productDtos.Add(productDto);
            }

            return productDtos;
        }

        public async Task UpdateProductAsync(ProductDto productDto)
        {
            var product = ConvertToProduct(productDto);

            if (product == null)
                throw new ArgumentException("Cannot update empty product");

            await _repository.UpdateAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task DecrementStockAsync(int id, int quantity)
        {
            await _repository.UpdateStockAsync(id, quantity, false);
        }

        public async Task IncrementStockAsync(int id, int quantity)
        {
            await _repository.UpdateStockAsync(id, quantity, true);
        }

        #region PrivateMethods
        
        private static int GetDistributedUniqueId()
        {
            int NodeId = int.Parse(Environment.GetEnvironmentVariable(Constants.NodeId) ?? "1");
            return (new DistributedTimeIdGenerator(NodeId)).NextId();
        }

        private static ProductDto? ConvertToProductDto(Product? product)
        {
            if (product == null)
                return null;

            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockAvailable = product.StockAvailable
            };

            return productDto;
        }

        private static Product? ConvertToProduct(ProductDto? productDto)
        {
            if (productDto == null)
                return null;

            var product = new Product
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                StockAvailable = productDto.StockAvailable
            };

            return product;
        }

        #endregion
    }
}
