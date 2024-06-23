using System.ComponentModel.DataAnnotations;

namespace SupermarketAPI.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The product name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "The price must be greater than 0.")]
        public decimal Price { get; set; }

        public string Description { get; set; } // Descrierea produsului

        public string Category { get; set; } // Categoria produsului

        public int StockQuantity { get; set; } // Cantitatea disponibilă în stoc

        public string ImageUrl { get; set; } // URL-ul imaginii produsului

        // Alte proprietăți după necesități
    }
}
