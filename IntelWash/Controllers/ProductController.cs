using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntelWash.Model;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IntelWash.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationContext context;
        private readonly ILogger<ProductController> _logger;
        public ProductController(ILogger<ProductController> logger,ApplicationContext context)
        {
            _logger = logger;
            this.context = context;
        }
        
        [HttpGet]
        [Route("/GetProducts")]
        public IEnumerable<Product> GetProducts()
        {
            IEnumerable<Product> products = context.Products.ToArray();
            _logger.LogInformation("Displayed items in Products");
            return products;
        }
        [HttpGet]
        [Route("/GetProductById/{id}")]
        public IActionResult GetProductById(int id)
        {
           
            Product product = context.Products.SingleOrDefault(x => x.Id == id);
            if (product == null)
            {
                _logger.LogInformation($"Product ID:{id} is null!");
                return NotFound();
            }
            else
            {
                _logger.LogInformation($"Product ID:{id} Name:{product.Name} Price:{product.Price}");
                return Ok(product);
            }
           
        }
        [HttpDelete]
        [Route("/DeleteProductById/{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            Product item = await context.Products.FindAsync(id);
            if (item == null)
            {
                _logger.LogInformation($"Product ID:{id} is null!");
                return NotFound();
            }
            else
            {
                _logger.LogInformation($"Product ID:{id} was removed!");
                context.Products.Remove(context.Products.SingleOrDefault(p => p.Id ==id));
                await context.SaveChangesAsync();
                return Ok();
            }
        }
        [HttpPost]
        [Route("/AddProduct")]
        public async Task<ActionResult> Add([FromBody] Product item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _logger.LogInformation($"Product ID:{item.Id} Name:{item.Name} Price:{item.Price} was added!");
            context.Products.Add(item);
            await context.SaveChangesAsync();
            return Ok();
        }

       
        
        [HttpPut]
        [Route("/UpdateProductById/{id}")]
        public async Task<IActionResult> Update(int id, Product product)
        {
            if (id != product.Id)
            {
                throw new Exception("Ids don't match!");
            }
            
            if (context.Products.Where(p => p.Id == id).Any(p => p.Name != product.Name))
            {
                if (context.Products.Any(p => p.Name == product.Name))
                {
                    throw new Exception($"item with this name: {product.Name} already exists!");
                }
            }
            
            context.Entry(product).State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.Products.Any(e => e.Id == id))
                {
                    throw new Exception($"item with id{id} not found!");
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProductById", new { id = product.Id }, product);
        }
        
    }
}