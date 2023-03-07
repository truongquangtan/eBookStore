using System.Text.Json.Serialization;

namespace BookStoreWebApp.Models.Request
{
    public class GetTotalItemsRequest
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }
    }
}
