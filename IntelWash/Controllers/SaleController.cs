using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntelWash.Model;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IntelWash.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SaleController : ControllerBase
    {
        private readonly ApplicationContext context;
        private readonly ILogger<SaleController> _logger;

        public SaleController(ILogger<SaleController> logger,ApplicationContext context)
        {
            _logger = logger;
            this.context = context;
        }
        [HttpGet]
        [Route("/GetSales")]
        public IEnumerable<Sale> GetSales()
        {
            IEnumerable<Sale> sales = context.Sales.ToArray();
            _logger.LogInformation("Displayed items in Sales");
            return sales;
        }
        [HttpGet]
        [Route("/GetSaleById/{id}")]
        public IActionResult GetSalesById(int id)
        {
            Sale sale = context.Sales.SingleOrDefault(x => x.Id == id);
            if (sale == null)
            {
                _logger.LogInformation($"Sale ID:{id} is null!");
                return NotFound();
            }
            else
            {
                _logger.LogInformation($"Sale ID:{id}");
                return Ok(sale);
            }
        }
        [HttpDelete]
        [Route("/DeleteSaleById/{id}")]
        public async Task<ActionResult> DeleteSaleById(int id)
        {
            Sale sale = await context.Sales.FindAsync(id);
            if (sale == null)
            {
                _logger.LogInformation($"Sale ID:{id} is null!");
                return NotFound();
            }
            else
            {
                _logger.LogInformation($"Sale ID:{id} was removed!");
                context.Sales.Remove(context.Sales.SingleOrDefault(p => p.Id ==id));
                await context.SaveChangesAsync();
                return Ok();
            }
        }
        [HttpPost]
        [Route("/AddSale")]
        public async Task<ActionResult> AddSale([FromBody] Sale sale)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _logger.LogInformation($"Sale ID:{sale.Id} was added!");
            context.Sales.Add(sale);
            await context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut]
        [Route("/UpdateSaleById")]  
        public async Task<ActionResult> UpdateSale([FromBody] Sale sale)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            Sale oldsale = context.Sales.SingleOrDefault(x => x.Id == sale.Id);
            if (oldsale == null) return NotFound();
            oldsale.SalesPointId = sale.SalesPointId;
            oldsale.BuyerId = sale.BuyerId;
            oldsale.TotalAmmount = sale.TotalAmmount;
            var old_saledata = sale.SalesData.ToArray();
            oldsale.SalesData = old_saledata;
            context.Sales.Update(oldsale);
            await context.SaveChangesAsync();
            _logger.LogInformation($"Sale ID:{sale.Id} and SalesData was updated!");
            return Ok();
        }
    }
}