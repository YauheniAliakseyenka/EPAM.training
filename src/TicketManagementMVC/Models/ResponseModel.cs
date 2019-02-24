namespace TicketManagementMVC.Models
{
    public class ResponseModel
    {
        public object Object { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }
}