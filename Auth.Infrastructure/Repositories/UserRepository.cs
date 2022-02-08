using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Auth.Core.Models;
using Auth.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDBContext _context;

        public UserRepository(AuthDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc/>
        public async Task<User> TryGetAsync(Expression<Func<User, bool>> where)
        {
            var query = _context.Users.AsQueryable();

            if (where != null)
            {
                query = query.Where(where);
            }

            return await query.FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(Expression<Func<User, bool>> where)
        {
            var query = _context.Users.AsQueryable();

            if (where != null)
            {
                query = query.Where(where);
            }

            return await query.AnyAsync();
        }

        /// <inheritdoc/>
        public void Insert(User user)
        {
            _context.Users.Add(user);
        }
    }
}
