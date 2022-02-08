using System;
using System.Threading.Tasks;
using Auth.Core.Repositories;

namespace Auth.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AuthDBContext _context;

        public UnitOfWork(AuthDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
