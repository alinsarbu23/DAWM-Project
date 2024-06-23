namespace SupermarketAPI.Models
{
    public class CommandProduct
    {
        public int CommandId { get; set; }
        public Command Command { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

    }
}
