using System.Collections.Generic;
using User.WebApi.Controllers;
using User.WebApi.Models;

namespace User.WebApi.Infrastructure.Hypermedia
{
    internal class UserLinks
    {
        private readonly UserController _controller;

        public UserLinks(UserController controller)
        {
			_controller = controller;
        }

        public void AddPutUserLink(List<LinkModel> links, int userId)
        {
            links.Add(new LinkModel
            {
                Href = _controller.Url.Link(nameof(_controller.Put), new { id = userId}),
                Method = "PUT",
                Rel = "update_user"
            });
        }

        public void AddGetUserLink(List<LinkModel> links, int userId)
        {
            links.Add(new LinkModel
            {
                Href = _controller.Url.Link(nameof(_controller.Get), new { id = userId }),
                Method = "GET",
                Rel = "get_user"
            });
        }

        public void AddDeleteUserLink(List<LinkModel> links, int userId)
        {
            links.Add(new LinkModel
            {
                Href = _controller.Url.Link(nameof(_controller.Get), new { id = userId }),
                Method = "DELETE",
                Rel = "delete_user (admin only)"
            });
        }
    }
}
