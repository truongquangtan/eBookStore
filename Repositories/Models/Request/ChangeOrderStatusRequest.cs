namespace BookStoreWebApp.Models.Request
{
    public class ChangeOrderStatusRequest
    {
        public int OrderId { get; set; }
        public string NewStatus { get; set; }
    }
}
