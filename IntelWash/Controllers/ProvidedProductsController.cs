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
    public class ProvidedProductsController : ControllerBase
    {
         private readonly ApplicationContext context;

        public ProvidedProductsController(ApplicationContext context)
        {
            this.context = context;
        }

        
        [HttpGet]
        [Route("/GetProvidedProducts")]
        public async Task<ActionResult<IEnumerable<ProvidedProduct>>> GetProvidedProducts()
        {
            return await context.ProvidedProducts.ToListAsync();
        }

       
        [HttpGet]
        [Route("/GetProvidedProductById/{id}")]
        public async Task<ActionResult<ProvidedProduct>> GetProvidedProduct(int id)
        {
            var product = await context.ProvidedProducts.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        
        [HttpPut]
        [Route("/PutProvidedProductById/{id}")]
        public async Task<IActionResult> PutProvidedProduct(int id, ProvidedProduct product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            if (context.ProvidedProducts.Where(p => p.Id == id).Any(p=> p.ProductId != product.ProductId))
                throw new Exception("You can't change ProductId of current product.");

            context.Entry(product).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.ProvidedProducts.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }


        
        [HttpDelete]
        [Route("/DeleteProvidedProduct/{id}")]
        public async Task<IActionResult> DeleteProvidedProduct(int id)
        {
            var product = await context.ProvidedProducts.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            context.ProvidedProducts.Remove(product);
            await context.SaveChangesAsync();

            return Ok();
        }

        
    }
}