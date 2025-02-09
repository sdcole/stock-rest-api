using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace StockRestAPI.Controllers
{

    /**
     *      Minute Bars Controller
     *      
     *      This controller returns the bars data by minute.
     * 
     * 
     * 
     * 
     **/
    [ApiController]
    [Route("")]
    public class MinuteBarsController : Controller
    {
        
        private readonly AppDbContext _context;
        public MinuteBarsController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet("v1/minute-bars")]
        public async Task<IActionResult> GetMinuteBars(string ticker, DateTime startTime, DateTime endTime)
        {
            var minuteBars = await _context.MinuteBars
                .Where(mb => mb.Symbol.ToUpper() == ticker.Trim().ToUpper() && mb.Timestamp >= startTime.ToUniversalTime()) // Optional startTime
                .OrderBy(mb => mb.Timestamp) // Order by Timestamp in ascending order
                .ToListAsync(); // Async database query

            return StatusCode(200, minuteBars);
        }
    }
}
