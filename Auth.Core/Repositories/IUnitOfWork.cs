using System.Threading.Tasks;

namespace Auth.Core.Repositories
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}
