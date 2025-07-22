namespace StockRestAPI.Models
{
    public class PromptWithJsonRequest
    {
        public string Prompt { get; set; } = string.Empty;

        // Can be any structured data — use `object` to allow flexibility

        // Optional fields
        public string Model { get; set; } = "deepseek-r1:14b";
        public bool Stream {  get; set; }
    }

}
