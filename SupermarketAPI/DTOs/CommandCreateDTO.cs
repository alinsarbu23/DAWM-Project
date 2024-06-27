using System.ComponentModel.DataAnnotations;

namespace SupermarketAPI.DTOs
{
    public class CommandCreateDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "The User is required.")]
        public string Name { get; set; }
    }
}
