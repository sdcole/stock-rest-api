using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace StockRestAPI.Controllers
{

    /**
     *      Daily Bars Controller
     *      
     *      This controller returns the bars data by day.
     * 
     * 
     * 
     * 
     **/
    [ApiController]
    [Route("api/market")]
    public class DailyBarsController : Controller
    {

        private readonly AppDbContext _context;
        public DailyBarsController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet("v1/daily-bars")]
        public async Task<IActionResult> GetDailyBars(string symbol, DateTime startTime, DateTime endTime)
        {
            var dailyBars = await _context.DailyBars
                .Where(db => db.Symbol.ToUpper() == symbol.Trim().ToUpper() && db.Timestamp >= startTime.ToUniversalTime()) // Optional startTime
                .OrderBy(db => db.Timestamp) // Order by Timestamp in ascending order
                .ToListAsync(); // Async database query

            return StatusCode(200, dailyBars);
        }



    }
}
