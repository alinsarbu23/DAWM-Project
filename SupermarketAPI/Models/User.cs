using System.ComponentModel.DataAnnotations;

namespace SupermarketAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(255, ErrorMessage = "The Name must not exceed {1} characters.")]
        public string Name { get; set; }

    }
}
