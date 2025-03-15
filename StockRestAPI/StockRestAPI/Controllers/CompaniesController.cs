using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace StockRestAPI.Controllers
{
    [ApiController]
    [Route("api/market")]
    public class CompaniesController : Controller
    {
        private readonly AppDbContext _context;
        public CompaniesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("v1/companies")]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _context.Companies
                .ToListAsync(); // Async database query

            return StatusCode(200, companies);
        }

    }
}
