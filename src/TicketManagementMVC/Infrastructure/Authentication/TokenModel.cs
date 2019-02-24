namespace TicketManagementMVC.Infrastructure.Authentication
{
    public class TokenModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}