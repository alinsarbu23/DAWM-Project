using System;
using System.Collections.Generic;

namespace SupermarketAPI.Models
{
    public class Command
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        public List<CommandProduct> CommandProducts { get; set; }
    }
}
