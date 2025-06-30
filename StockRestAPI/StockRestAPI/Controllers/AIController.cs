using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockRestAPI.Models;

namespace StockRestAPI.Controllers
{
    [ApiController]
    [Route("api/ai")]
    public class AIController : Controller
    {

        /**
         * This will make API calls to the local OLAMMA instance
         * 
         * 
         **/
        [HttpPost("v1/prompt-json")]
        public async Task<IActionResult> PromptJson()
        {
            var promptWithJsonRequest = new PromptWithJsonRequest
            {
                Prompt = "This is a test for connectivity, respond with 'Hello World!'",
                Model = "deepseek-r1:8b",
                Stream = false
            };

            using var client = new HttpClient();

            var jsonString = JsonSerializer.Serialize(promptWithJsonRequest, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("", httpContent);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, response);
            }

            var result = await response.Content.ReadAsStringAsync();

            //This will ignore any of the "Thoughts" the ai model will have before it responds.
            result = Regex.Replace(result, @"\u003cthink\u003e.*?\u003c/think\u003e\n\n", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            return StatusCode(200,result);
        }


    }
}