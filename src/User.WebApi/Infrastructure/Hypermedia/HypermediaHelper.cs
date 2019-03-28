using System.Collections.Generic;
using User.WebApi.Controllers;
using User.WebApi.Models;

namespace User.WebApi.Infrastructure.Hypermedia
{
    internal static class HypermediaHelper
    {
        public static IEnumerable<LinkModel> AuthHypermediaLinks(AuthController controller)
        {
            var links = new List<LinkModel>();

			links.Add(new LinkModel
            {
                Href = controller.Url.Link(nameof(controller.RefreshToken), null),
                Method = "POST",
                Rel = "refresh_token"
            });

            return links;
        }

        public static IEnumerable<LinkModel> GetUserHypermediaLinks(UserController controller, int userId)
        {
            var links = new List<LinkModel>();
            var userHelper = new UserLinks(controller);

            userHelper.AddDeleteUserLink(links, userId);
            userHelper.AddPutUserLink(links, userId);
			userHelper.AddUpdateUserBalanceLink(links, userId);

			return links;
        }

        public static IEnumerable<LinkModel> PutUserHypermediaLinks(UserController controller, int userId)
        {
            var links = new List<LinkModel>();
            var userHelper = new UserLinks(controller);

            userHelper.AddDeleteUserLink(links, userId);
            userHelper.AddGetUserLink(links, userId);
			userHelper.AddUpdateUserBalanceLink(links, userId);

			return links;
        }

        public static IEnumerable<LinkModel> PostUserHypermediaLinks(UserController controller, int userId)
        {
            var links = new List<LinkModel>();
            var userHelper = new UserLinks(controller);

            userHelper.AddDeleteUserLink(links, userId);
            userHelper.AddGetUserLink(links, userId);
            userHelper.AddPutUserLink(links, userId);
			userHelper.AddUpdateUserBalanceLink(links, userId);

            return links;
        }
    }
}
