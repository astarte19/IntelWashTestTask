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
        public async Task<IEnumerable<Sale>> GetSales()
        {
            IEnumerable<Sale> sales = await context.Sales.Include(s => s.SalesData).ToListAsync();
            _logger.LogInformation("Displayed items in Sales");
            return sales;
        }
        [HttpGet]
        [Route("/GetSaleById/{id}")]
        public async Task<ActionResult<Sale>>  GetSalesById(int id)
        {
            var sale = await context.Sales.FindAsync(id);
            context.Entry(sale).Collection(s => s.SalesData).Load();
            if (sale == null)
            {
                return NotFound();
            }

            return sale;
        }
        [HttpDelete]
        [Route("/DeleteSaleById/{id}")]
        public async Task<ActionResult> DeleteSaleById(int id)
        {
            var sale = await context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }
            context.Entry(sale).Collection(s => s.SalesData).Load();
            foreach (var saleData in sale.SalesData)
                context.SaleDatas.Remove(saleData);

            context.Sales.Remove(sale);
            await context.SaveChangesAsync();

            return Ok();
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
        [Route("/UpdateSaleById/{id}")]  
        public async Task<ActionResult> UpdateSale(int id,  Sale sale)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            Sale oldsale = context.Sales.SingleOrDefault(x => x.Id == id);
            if (oldsale == null) return NotFound();
            oldsale.SalesPointId = sale.SalesPointId;
            oldsale.BuyerId = sale.BuyerId;
            
            var old_saledata = sale.SalesData.ToArray();
            oldsale.SalesData = old_saledata;
            context.Sales.Update(oldsale);
            await context.SaveChangesAsync();
            _logger.LogInformation($"Sale ID:{sale.Id} and SalesData was updated!");
            
            return Ok();
        }
        
    }
}