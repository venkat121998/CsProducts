using CS.Core.Models.Dto;
using CS.Core.Services.ProductService;
using CS.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CS.Tests
{
        public class ProductControllerTests
        {
            private readonly Mock<IProductService> _mockProductService;
            private readonly ProductController _productController;

            public ProductControllerTests()
            {
                _mockProductService = new Mock<IProductService>();
                _productController = new ProductController(_mockProductService.Object);
            }

            [Fact]
            public async Task CreateProduct_ReturnsCreatedAtAction_WhenProductIsValid()
            {
                // Arrange
                var productDto = new ProductDto { Id = 1, Name = "Test Product", Price = 10.99M, StockAvailable = 100 };
                _mockProductService.Setup(service => service.AddProductAsync(productDto)).Returns(Task.CompletedTask);

                // Act
                var result = await _productController.CreateProduct(productDto) as CreatedAtActionResult;

                // Assert
                Assert.NotNull(result);
                Assert.Equal("GetProductById", result.ActionName);
                Assert.Equal(1, ((ProductDto)result.Value).Id);
            }

            [Fact]
            public async Task CreateProduct_ReturnsBadRequest_WhenModelStateIsInvalid()
            {
                // Arrange
                _productController.ModelState.AddModelError("Name", "Required");

                // Act
                var result = await _productController.CreateProduct(new ProductDto()) as BadRequestObjectResult;

                // Assert
                Assert.NotNull(result);
                Assert.IsType<BadRequestObjectResult>(result);
            }

            [Fact]
            public async Task GetProducts_ReturnsOkResult_WithListOfProducts()
            {
                // Arrange
                var products = new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "Product 1", Price = 10.99M, StockAvailable = 100 },
                new ProductDto { Id = 2, Name = "Product 2", Price = 12.99M, StockAvailable = 200 }
            };
                _mockProductService.Setup(service => service.GetProductsAsync()).ReturnsAsync(products);

                // Act
                var result = await _productController.GetProducts();

                // Assert
                Assert.NotNull(result);
                Assert.IsType<ActionResult<IEnumerable<ProductDto>>>(result);
                var returnedProducts = (IEnumerable<ProductDto>)(((ObjectResult)result?.Result)?.Value);
            Assert.Equal(2, returnedProducts.Count());
            }

            [Fact]
            public async Task GetProductById_ReturnsOkResult_WhenProductExists()
            {
                // Arrange
                var productDto = new ProductDto { Id = 1, Name = "Test Product", Price = 10.99M, StockAvailable = 100 };
                _mockProductService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync(productDto);

                // Act
                var result = await _productController.GetProductById(1);

                // Assert
                Assert.NotNull(result);
                Assert.IsType<ActionResult<ProductDto>>(result);
                Assert.Equal(1, ((ProductDto)((ObjectResult)result?.Result)?.Value)?.Id);
            }

            [Fact]
            public async Task GetProductById_ReturnsNotFound_WhenProductDoesNotExist()
            {
                // Arrange
                _mockProductService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync((ProductDto)null);

                // Act
                var result = await _productController.GetProductById(1);

                // Assert
                Assert.IsType<NotFoundResult>(result.Result);
            }

            [Fact]
            public async Task UpdateProduct_ReturnsNoContent_WhenUpdateIsSuccessful()
            {
                // Arrange
                var productDto = new ProductDto { Id = 1, Name = "Updated Product", Price = 10.99M, StockAvailable = 100 };
                _mockProductService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync(productDto);
                _mockProductService.Setup(service => service.UpdateProductAsync(productDto)).Returns(Task.CompletedTask);

                // Act
                var result = await _productController.UpdateProduct(1, productDto);

                // Assert
                Assert.IsType<NoContentResult>(result);
            }

            [Fact]
            public async Task DeleteProduct_ReturnsNoContent_WhenDeleteIsSuccessful()
            {
                // Arrange
                var productDto = new ProductDto { Id = 1, Name = "Test Product", Price = 10.99M, StockAvailable = 100 };
                _mockProductService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync(productDto);
                _mockProductService.Setup(service => service.DeleteProductAsync(1)).Returns(Task.CompletedTask);

                // Act
                var result = await _productController.DeleteProduct(1);

                // Assert
                Assert.IsType<NoContentResult>(result);
            }

            [Fact]
            public async Task DecrementStock_ReturnsOkResult_WhenDecrementIsSuccessful()
            {
                // Arrange
                var productDto = new ProductDto { Id = 1, Name = "Test Product", Price = 10.99M, StockAvailable = 100 };
                _mockProductService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync(productDto);
                _mockProductService.Setup(service => service.DecrementStockAsync(1, 10)).Returns(Task.CompletedTask);

                // Act
                var result = await _productController.DecrementStock(1, 10);

                // Assert
                Assert.IsType<OkResult>(result);
            }

            [Fact]
            public async Task IncrementStock_ReturnsOkResult_WhenIncrementIsSuccessful()
            {
                // Arrange
                var productDto = new ProductDto { Id = 1, Name = "Test Product", Price = 10.99M, StockAvailable = 100 };
                _mockProductService.Setup(service => service.GetProductByIdAsync(1)).ReturnsAsync(productDto);
                _mockProductService.Setup(service => service.IncrementStockAsync(1, 10)).Returns(Task.CompletedTask);

                // Act
                var result = await _productController.IncrementStock(1, 10);

                // Assert
                Assert.IsType<OkResult>(result);
            }
        }
}