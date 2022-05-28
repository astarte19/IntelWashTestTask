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
    public class BuyerController : ControllerBase
    {
        private readonly ApplicationContext context;
        private readonly ILogger<BuyerController> _logger;
        public BuyerController(ILogger<BuyerController> logger,ApplicationContext context)
        {
            _logger = logger;
            this.context = context;
        }
            [HttpGet]
        [Route("/GetBuyers")]
        public IEnumerable<Buyer> GetBuyers()
        {
            IEnumerable<Buyer> buyers = context.Buyers.ToArray();
            _logger.LogInformation("Displayed Buyers!");
            return buyers;
        }
        [HttpGet]
        [Route("/GetBuyerById/{id}")]
        public IActionResult GetBuyerById(int id)
        {
            Buyer buyer = context.Buyers.SingleOrDefault(x => x.Id == id);
            if (buyer == null)
            {
                _logger.LogInformation($"Buyer ID:{id} is null!");
                return NotFound();
            }
            else
            {
                _logger.LogInformation($"Displayed Buyer ID:{id}");
                return Ok(buyer);
            }
           
        }
        [HttpDelete]
        [Route("/DeleteBuyerById/{id}")]
        public async Task<ActionResult> DeleteBuyerById(int id)
        {
            Buyer buyer = await context.Buyers.FindAsync(id);
            if (buyer == null)
            {
                _logger.LogInformation($"Buyer ID:{id} is null!");
                return NotFound();
            }
            else
            {
                context.Buyers.Remove(context.Buyers.SingleOrDefault(p => p.Id ==id));
                _logger.LogInformation($"Buyer ID:{id} was removed!");
                await context.SaveChangesAsync();
                return Ok();
            }
        }
        [HttpPost]
        [Route("/AddBuyer")]
        public async Task<ActionResult> Add([FromBody] Buyer buyer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _logger.LogInformation($"Buyer ID:{buyer.Id} was added!");
            context.Buyers.Add(buyer);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        [Route("/UpdateBuyerById")]  
        public async Task<ActionResult> Update([FromBody] Buyer buyer)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            Buyer oldbuyer = context.Buyers.SingleOrDefault(x => x.Id == buyer.Id);
            if (oldbuyer == null) return NotFound();
            oldbuyer.Name = buyer.Name;
            var old_salesids = buyer.SalesId.ToArray();
            if (old_salesids==null)
            {
                context.Buyers.Update(oldbuyer);
                await context.SaveChangesAsync();
                _logger.LogInformation($"Buyer ID:{buyer.Id} was updated!");
            }
            else
            {
                oldbuyer.SalesId = old_salesids;
                context.Buyers.Update(oldbuyer);
                await context.SaveChangesAsync();
                _logger.LogInformation($"Buyer ID:{buyer.Id} with saleids was updated!");
            }
           
            return Ok();
        }
    }
}
