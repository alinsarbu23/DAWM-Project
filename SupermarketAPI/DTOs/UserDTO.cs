using SupermarketAPI.Models;

namespace SupermarketAPI.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Command> Commands { get; set; } = new List<Command>();
    }
}
