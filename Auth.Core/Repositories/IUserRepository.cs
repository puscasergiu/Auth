using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Auth.Core.Models;

namespace Auth.Core.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Tries to find the first user based on the where clause;
        /// </summary>
        /// <returns>The first user entity if found; Null if no entity was found</returns>
        Task<User> TryGetAsync(Expression<Func<User, bool>> where = null);
        Task<bool> ExistsAsync(Expression<Func<User, bool>> where = null);
        void Insert(User user);
    }
}
