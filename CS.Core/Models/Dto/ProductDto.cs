using System.ComponentModel.DataAnnotations;

namespace CS.Core.Models.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The product name is required.")]
        [StringLength(100, ErrorMessage = "The product name can't be longer than 100 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "The product description can't be longer than 500 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "The product price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "The product price must be greater than zero.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "The stock available is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "The stock available must be a non-negative integer.")]
        public int StockAvailable { get; set; }
    }
}
