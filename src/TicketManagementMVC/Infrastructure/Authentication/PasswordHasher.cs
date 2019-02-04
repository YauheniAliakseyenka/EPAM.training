using Microsoft.AspNet.Identity;

namespace TicketManagementMVC.Infrastructure.Authentication
{
	internal class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return password;
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            return HashPassword(providedPassword) == hashedPassword ? 
                PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}