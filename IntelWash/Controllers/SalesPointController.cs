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
        public IEnumerable<SalesPoint> GetSalesPoint()
        {
            IEnumerable<SalesPoint> salesPoints = context.SalesPoints.ToArray();
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
                //Ну и при удалении точки продажи я решил удалять записи ProvidedProducts этой точки продажи
                context.SalesPoints.Remove(context.SalesPoints.SingleOrDefault(p => p.Id ==id));
                context.ProvidedProducts.RemoveRange(context.ProvidedProducts.Where(p=>p.SalesPointId==id));
                await context.SaveChangesAsync();
                return Ok();
            }
        }
        [HttpPost]
        [Route("/AddSalesPoint")]
        public async Task<ActionResult> Add([FromBody] SalesPoint salesPoint)
        {
            Random rnd = new Random();
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _logger.LogInformation($"SalesPoint ID:{salesPoint.Id} was added!");
            context.SalesPoints.Add(salesPoint);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        [Route("/UpdateSalesPoint")]  
        public async Task<ActionResult> Update([FromBody] SalesPoint salesPoint)
        {
            //Update в SalePoint только по имени
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            SalesPoint oldItem = context.SalesPoints.SingleOrDefault(x => x.Id == salesPoint.Id);
            if (oldItem == null) return NotFound();
            oldItem.Name = salesPoint.Name;
            var old_provided = salesPoint.ProvidedProducts.ToArray();
            oldItem.ProvidedProducts = old_provided;
            context.SalesPoints.Update(oldItem);
            await context.SaveChangesAsync();
            _logger.LogInformation($"SalesPoint ID:{salesPoint.Id} was updated!");
            return Ok();
        }
    }
}