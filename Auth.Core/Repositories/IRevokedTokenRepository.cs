using System.Threading.Tasks;

namespace Auth.Core.Repositories
{
    public interface IRevokedTokenRepository
    {
        Task<bool> ExistsAsync(string token);
        Task InsertAsync(string token);
    }
}
