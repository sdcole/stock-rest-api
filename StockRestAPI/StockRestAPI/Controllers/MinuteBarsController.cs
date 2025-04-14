using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockRestAPI.Models;

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
    [Route("api/market")]
    public class MinuteBarsController : Controller
    {
        
        private readonly AppDbContext _context;
        public MinuteBarsController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet("v1/minute-bars")]
        public async Task<IActionResult> GetMinuteBars(string symbol, DateTime startTime, DateTime endTime, string? interval)
        {
            List<MinuteBar> minuteBars = new List<MinuteBar>();

            if (interval == null || interval == "1m")
            {
                minuteBars = await _context.MinuteBars
                .Where(mb => mb.Symbol.ToUpper() == symbol.Trim().ToUpper() && mb.Timestamp >= startTime.ToUniversalTime()) // Optional startTime
                .OrderBy(mb => mb.Timestamp) // Order by Timestamp in ascending order
                .ToListAsync(); // Async database query
            }
            else if(interval == "1h")
            {
                minuteBars = await _context.MinuteBars
                .Where(mb => mb.Symbol.ToUpper() == symbol.Trim().ToUpper() 
                && mb.Timestamp >= startTime.ToUniversalTime()
                && mb.Timestamp.Minute == 0
                && mb.Timestamp.Second == 0) // Optional startTime
                .OrderBy(mb => mb.Timestamp) // Order by Timestamp in ascending order
                .ToListAsync(); // Async database query
            }
            

            return StatusCode(200, minuteBars);
        }
    }
}
