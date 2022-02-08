using Auth.API.Filters;
using Auth.API.Models;
using Auth.Core.Commands.LoginUserCommand;
using Auth.Core.Commands.RegisterUserCommand;
using Auth.Core.Cryptography;
using Auth.Core.Models;
using Auth.Core.Repositories;
using Auth.Infrastructure;
using Auth.Infrastructure.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

namespace Auth.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
                options.Filters.Add<CustomExceptionFilter>()
            );

            var tokenSettings = Configuration.GetSection("Token").Get<TokenSettings>();
            var connectionStringsSettings = Configuration.GetSection("ConnectionStrings").Get<ConnectionStringsSettings>();
            services.AddSingleton(tokenSettings);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth.API", Version = "v1" });
            });

            services.AddMediatR(typeof(RegisterUserCommand).Assembly);

            services.AddDbContext<AuthDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Database")));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();

            var multiplexer = ConnectionMultiplexer.Connect(Configuration.GetConnectionString("Redis"));
            services.AddScoped(sp => multiplexer.GetDatabase());
            services.AddScoped<IRevokedTokenRepository, RevokedTokenRepository>();

            services.AddSingleton<HashCrypter>();
            services.AddSingleton<TokenCrypter>();
            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<AbstractValidator<RegisterUserCommand>, RegisterUserCommandValidator>();
            services.AddScoped<AbstractValidator<LoginUserCommand>, LoginUserCommandValidator>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AuthDBContext authDBContext)
        {
            authDBContext.Database.EnsureCreated();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
