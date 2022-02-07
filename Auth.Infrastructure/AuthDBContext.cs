using System;
using Auth.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure
{
    public class AuthDBContext : DbContext
    {
        public AuthDBContext(DbContextOptions<AuthDBContext> options)
            : base(options ?? throw new ArgumentNullException(nameof(options)))
        {
        }

        public DbSet<User> Users { get; init; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthDBContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
