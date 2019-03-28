using System.Threading.Tasks;
using User.WebApi.Helper;

namespace TicketManagementWPF.Infrastructure.Utils.UserManagement
{
    public interface IUserManagement
    {
        /// <summary>
        /// Create a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<ResponseModel> Create(Models.User user);

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<ResponseModel> Update(Models.User user);

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<ResponseModel> Delete(Models.User user);

        /// <summary>
        /// Get list of users
        /// </summary>
        /// <returns></returns>
        Task<ResponseModel> ToList();

        /// <summary>
        /// Get a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseModel> Get(int id);

		/// <summary>
		/// Find a user
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<ResponseModel> FindBy(UserSearchEnum by, string value);
    }
}
