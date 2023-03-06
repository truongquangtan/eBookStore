using System.Text.Json.Serialization;

namespace BookStoreWebApp.Models.Request
{
    public class AddToCardRequest
    {
        [JsonPropertyName("productId")]
        public int ProductId { get; set; }
        [JsonPropertyName("username")]
        public string Username { get; set; }
    }
}
