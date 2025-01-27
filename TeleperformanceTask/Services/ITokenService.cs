using TeleperformanceTask.Models;

namespace TeleperformanceTask.Services
{
    public interface ITokenService
    {
        string GenerateToken(ApplicationUser user);
    }
}
