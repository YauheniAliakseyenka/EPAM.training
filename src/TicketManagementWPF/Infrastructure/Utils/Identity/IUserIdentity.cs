using TicketManagementWPF.Models;

namespace TicketManagementWPF.Infrastructure.Utils.Identity
{
    public interface IUserIdentity
    {
        void SaveTokens(TokenModel tokens);
        TokenModel GetTokens();
        int GetId();
		bool IsAdmin();
    }
}
