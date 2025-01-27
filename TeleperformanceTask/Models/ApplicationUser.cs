using Microsoft.AspNetCore.Identity;

namespace TeleperformanceTask.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; }

    }
}
