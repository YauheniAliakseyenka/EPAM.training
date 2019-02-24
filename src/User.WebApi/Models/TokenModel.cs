using System.ComponentModel.DataAnnotations;

namespace User.WebApi.Models
{
    public class TokenModel
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
