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
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var products = await _context.Products.Join(_context.Categories,
              product => product.Id,
              category => category.Products.Select(p => p.Id).FirstOrDefault(),
              (product, category) => new ProductDTO
              {
                  Id = product.Id,
                  Name = product.Name,
                  Price = product.Price,
                  Category = category.Name
              })
                .ToListAsync();

            return Ok(products);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await _context.Products
                    .Where(p => p.Id == id)
                    .Join(_context.Categories,
                          product => product.Id,
                          category => category.Products.Select(p => p.Id).FirstOrDefault(),
                          (product, category) => new ProductDTO
                          {
                              Id = product.Id,
                              Name = product.Name,
                              Price = product.Price,
                              Category = category.Name // Map the category name
                                                       // Add other properties from Product as needed
                          })
                    .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> PostProduct(ProductDTO productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }



            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price
                // Set other properties as needed
            };


            var category = await _context.Categories
                             .FirstOrDefaultAsync(c => c.Name == productDto.Category);

            if (category != null)
            {
                category.Products.Add(product);
            }
            else
            {
                return BadRequest("Please type a valid category for this product.");
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            productDto.Id = product.Id;

            return CreatedAtAction(nameof(GetProduct), new { id = productDto.Id }, productDto);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductDTO productDto)
        {
            if (id != productDto.Id)
            {
                return BadRequest();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            product.Name = productDto.Name;
            product.Price = productDto.Price;
            // Update other properties as needed

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
