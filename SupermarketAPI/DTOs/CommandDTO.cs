using System;
using System.Collections.Generic;

namespace SupermarketAPI.DTOs
{
    public class CommandDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }


        public List<CommandProductDTO> CommandProducts { get; set; }
    }
}
