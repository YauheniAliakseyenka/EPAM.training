using System.Threading.Tasks;
using User.WebApi.Helper;

namespace TicketManagementWPF.Infrastructure.Utils.UserManagement
{
    public interface IAuth
    {
        Task<ResponseModel> SignIn(AuthModel credentials);
    }
}
