using System.Threading.Tasks;
using User.WebApi.Helper;

namespace TicketManagementWPF.Infrastructure.Utils.UserManagement
{
    public interface IUserProfile
    {
        /// <summary>
        /// Update a profile
        /// </summary>
        /// <param name="user">User model</param>
        /// <returns></returns>
        Task<ResponseModel> Update(Models.User user);

        /// <summary>
        /// Get user's profile
        /// </summary>
        /// <returns></returns>
        Task<ResponseModel> Get();
    }
}
