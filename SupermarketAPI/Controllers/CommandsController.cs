using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupermarketAPI.Data;
using SupermarketAPI.DTOs;
using SupermarketAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupermarketAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CommandsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommandDTO>>> GetCommands(string sortOrder = "orderdate")
        {
            var commandsQuery = _context.Commands
                .Include(c => c.CommandProducts)
                .ThenInclude(cp => cp.Product)
                .Select(c => new CommandDTO
                {
                    Id = c.Id,
                    OrderDate = c.OrderDate,
                    CommandProducts = c.CommandProducts.Select(cp => new CommandProductDTO
                    {
                        CommandId = cp.CommandId,
                        ProductId = cp.ProductId
                    }).ToList()
                });

            switch (sortOrder.ToLower())
            {
                case "orderdate":
                    commandsQuery = commandsQuery.OrderBy(c => c.OrderDate);
                    break;
                case "id":
                    commandsQuery = commandsQuery.OrderBy(c => c.Id);
                    break;
                default:
                    commandsQuery = commandsQuery.OrderBy(c => c.OrderDate);
                    break;
            }

            var commands = await commandsQuery.ToListAsync();
            return Ok(commands);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CommandDTO>> GetCommand(int id)
        {
            var command = await _context.Commands
                .Include(c => c.CommandProducts)
                .ThenInclude(cp => cp.Product)
                .Where(c => c.Id == id)
                .Select(c => new CommandDTO
                {
                    Id = c.Id,
                    OrderDate = c.OrderDate,
                    CommandProducts = c.CommandProducts.Select(cp => new CommandProductDTO
                    {
                        CommandId = cp.CommandId,
                        ProductId = cp.ProductId
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (command == null)
            {
                return NotFound();
            }

            return Ok(command);
        }

        [HttpPost]
        public async Task<ActionResult<CommandDTO>> PostCommand(CommandDTO commandDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new Command
            {
                OrderDate = commandDto.OrderDate
            };

            _context.Commands.Add(command);
            await _context.SaveChangesAsync();

            commandDto.Id = command.Id;

            return CreatedAtAction(nameof(GetCommand), new { id = commandDto.Id }, commandDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCommand(int id, CommandDTO commandDto)
        {
            if (id != commandDto.Id)
            {
                return BadRequest();
            }

            var command = await _context.Commands.FindAsync(id);
            if (command == null)
            {
                return NotFound();
            }

            command.OrderDate = commandDto.OrderDate;

            _context.Entry(command).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommandExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommand(int id)
        {
            var command = await _context.Commands.FindAsync(id);
            if (command == null)
            {
                return NotFound();
            }

            _context.Commands.Remove(command);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommandExists(int id)
        {
            return _context.Commands.Any(e => e.Id == id);
        }
    }
}
