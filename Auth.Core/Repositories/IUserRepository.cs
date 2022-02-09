using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Auth.Core.Models;

namespace Auth.Core.Repositories
{
    public interface IUserRepository
    {
        Task<(bool result, User user)> TryGetAsync(Expression<Func<User, bool>> where = null);
        Task<bool> ExistsAsync(Expression<Func<User, bool>> where = null);
        void Insert(User user);
    }
}
