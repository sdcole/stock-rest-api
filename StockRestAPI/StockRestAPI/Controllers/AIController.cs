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
    public class AIController(IConfiguration configuration) : Controller
    {


        private readonly string _aiEndpoint = configuration["ApiEndpoints:AI"];

        /**
         * This will make API calls to the local OLAMMA instance
         * 
         * 
         **/
        [HttpPost("v1/prompt-json")]
        public async Task<IActionResult> PromptJson(SimplePromptRequest request)
        {
            var promptWithJsonRequest = new PromptWithJsonRequest
            {
                Prompt = request.Prompt,
                Model = "llama3:8b-instruct-q4_K_M",
                Stream = false
            };

            using var client = new HttpClient();

            var jsonString = JsonSerializer.Serialize(promptWithJsonRequest, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(_aiEndpoint + "/api/generate", httpContent);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, response);
            }

            var result = await response.Content.ReadAsStringAsync();

            //This will ignore any of the "Thoughts" the ai model will have before it responds.
            using var doc = JsonDocument.Parse(result);

            string rawResponse = doc.RootElement.GetProperty("response").GetString();

            // Remove the <think>...</think> section using regex
            string cleaned = Regex.Replace(rawResponse, @"<think>.*?</think>\s*", "", RegexOptions.Singleline);

 
            return StatusCode(200, new { response = cleaned.Trim() });
        }


    }
}