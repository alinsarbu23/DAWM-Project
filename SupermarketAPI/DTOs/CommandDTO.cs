using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SupermarketAPI.DTOs
{
    public class CommandDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "The User is required.")]
        public string Name { get; set; }
        public List<CommandProductDTO> CommandProducts { get; set; }
    }
}
