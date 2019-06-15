using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NG_Core_Auth.Data;
using NG_Core_Auth.Models;

namespace NG_Core_Auth.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {

        private readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/values
        [HttpGet("[action]")]
        [Authorize("RequireLoggedIn")]
        public IActionResult GetProducts()
        {
            return Ok(_db.Products.ToList());
        }

        [HttpPost("[action]")]
        [Authorize("Admin")]
        public async Task<IActionResult> AddProduct([FromBody] ProductModel formdata)
        {
            var newProduct = new ProductModel
            {
                Name = formdata.Name,
                ImageUrl = formdata.ImageUrl,
                Description = formdata.Description,
                OutOfStock = formdata.OutOfStock,
                Price = formdata.Price
            };

            await _db.Products.AddAsync(newProduct);

            await _db.SaveChangesAsync();

            return Ok();

        }

        [HttpPut("[action]/{id}")]
        [Authorize("Admin")]
        public async Task<IActionResult> UpdateProduct([FromRoute] int id,[FromBody] ProductModel formdata)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var product = _db.Products.Where(prod => prod.ProductId == id).FirstOrDefault();

            if (product == null)
            {
                return NotFound();
            }

            product.Name = formdata.Name;
            product.Description = formdata.Description;
            product.ImageUrl = formdata.ImageUrl;
            product.OutOfStock = formdata.OutOfStock;
            product.Price = formdata.Price;

            _db.Entry(product).State = EntityState.Modified;

            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("[action]/{id}")]
        [Authorize("Admin")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _db.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            _db.Products.Remove(product);

            await _db.SaveChangesAsync();

            return Ok();
        }

    }
}
