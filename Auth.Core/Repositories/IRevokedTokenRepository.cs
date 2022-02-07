using System.Threading.Tasks;

namespace Auth.Core.Repositories
{
    public interface IRevokedTokenRepository
    {
        Task InsertAsync(string token);
        Task<bool> ExistsAsync(string token);
    }
}
