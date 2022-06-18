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
    public class SalesPointController : ControllerBase
    {
        private readonly ApplicationContext context;
        private readonly ILogger<ProductController> _logger;

        public SalesPointController(ILogger<ProductController> logger,ApplicationContext context)
        {
            _logger = logger;
            this.context = context;
        }
         [HttpGet]
        [Route("/GetSalesPoints")]
        public async Task<IEnumerable<SalesPoint>> GetSalesPoint()
        {
            IEnumerable<SalesPoint> salesPoints =  await context.SalesPoints.Include(sp => sp.ProvidedProducts).ToListAsync();
            _logger.LogInformation("Displayed items in SalesPoints");
            return salesPoints;
        }
        [HttpGet]
        [Route("/GetSalesPointById/{id}")]
        public IActionResult GetSalesPointById(int id)
        {
            SalesPoint salesPoint = context.SalesPoints.SingleOrDefault(x => x.Id == id);
            if (salesPoint == null)
            {
                _logger.LogInformation($"SalesPoint ID:{id} is null!");
                return NotFound();
            }
            else
            {
                _logger.LogInformation($"SalesPoint ID:{id}");
                return Ok(salesPoint);
            }
           
        }
        [HttpDelete]
        [Route("/DeleteSalesPointById/{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            SalesPoint salesPoint = await context.SalesPoints.FindAsync(id);
            if (salesPoint == null)
            {
                _logger.LogInformation($"SalesPoint ID:{id} is null!");
                return NotFound();
            }
            else
            {
                _logger.LogInformation($"SalesPoint ID:{id} was removed!");
                context.SalesPoints.Remove(context.SalesPoints.SingleOrDefault(p => p.Id ==id));
                
                await context.SaveChangesAsync();
                return Ok();
            }
        }
        
        [HttpPost]
        [Route("/AddSalesPoint")]
        public async Task<ActionResult<SalesPoint>> AddSalesPoint(SalesPoint salesPoint)
        {
            foreach (var providedProduct in salesPoint.ProvidedProducts)
            {
                CheckProductInProductsTable(providedProduct.ProductId, context);
            }

            context.SalesPoints.Add(salesPoint);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetSalesPoint", new { id = salesPoint.Id }, salesPoint);
        }

        [HttpPut]
        [Route("/UpdateSalesPointById/{id}")]  
        public async Task<ActionResult> UpdateSalesPoint(int id, SalesPoint salesPoint)
        {
            if (id != salesPoint.Id)
            {
                throw new Exception("Ids don't match!");
            }
            
            foreach (var providedProduct in salesPoint.ProvidedProducts)
            {
                MatchProvidedProducts(id, providedProduct.Id, "You can't change Id of product in other SalesPoint!");
                CheckProductInProductsTable(providedProduct.ProductId, context);
                context.Entry(providedProduct).State = EntityState.Modified;
            }

            context.Entry(salesPoint).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.SalesPoints.Any(e => e.Id == id))
                {
                    throw new Exception($"Salespoint with id{id} Not Found!");
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSalesPoint", new { id = salesPoint.Id }, salesPoint);
        }
        private void MatchProvidedProducts(int salesPointId, int providedProductId, string errorMsg)
        {
            
            if (context.SalesPoints.Where(sp => sp.Id != salesPointId).Any(sp => sp.ProvidedProducts.Any(pp => pp.Id == providedProductId)))
                throw new Exception(errorMsg);
        }
        public static void CheckProductInProductsTable(int productId, ApplicationContext _context)
        {
            if (!_context.Products.Any(p => p.Id == productId))
                throw new Exception($"Product with Id{productId} Not Found!");
        }
        public static void СheckForRepeatProductsIds(List<IProductId> productIds)
        {
            var salesPointProductsIdList = (from providedProduct in productIds
                select providedProduct.ProductId).ToList();
            if (salesPointProductsIdList.Count != salesPointProductsIdList.Distinct().Count())
                throw new Exception("Salespoint contains repeatable ids!");
        }

    }
}