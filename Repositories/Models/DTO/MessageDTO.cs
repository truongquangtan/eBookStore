namespace BookStoreWebApp.Models.DTO
{
    public enum MessageType
    {
        Success,
        Error,
        Warning,
    }
    public class MessageDTO
    {
        public string Message { get; set; }
        public MessageType MessageType { get; set; }
    }
}
