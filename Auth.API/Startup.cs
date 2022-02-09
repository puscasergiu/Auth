using Auth.API.Filters;
using Auth.API.Miscellaneous;
using Auth.API.Models.Options;
using Auth.Core.Abstractions;
using Auth.Core.Commands.RegisterUserCommand;
using Auth.Core.Cryptography;
using Auth.Core.Models;
using Auth.Core.Repositories;
using Auth.Infrastructure;
using Auth.Infrastructure.Repositories;
using FluentValidation.AspNetCore;
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
            ).AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RegisterUserCommandValidator>());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth.API", Version = "v1" });
            });

            services.AddSingleton<ITokenSettings>(Configuration.GetSection(TokenOptions.SectionName).Get<TokenOptions>());
            var connectionStringsOptions = Configuration.GetSection(ConnectionStringsOptions.SectionName).Get<ConnectionStringsOptions>();

            services.AddMediatR(typeof(RegisterUserCommand).Assembly);

            services.AddDbContext<AuthDBContext>(options => options.UseSqlServer(connectionStringsOptions.Database));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();

            var multiplexer = ConnectionMultiplexer.Connect(connectionStringsOptions.Redis);
            services.AddScoped(sp => multiplexer.GetDatabase());
            services.AddScoped<IRevokedTokenRepository, RevokedTokenRepository>();

            services.AddSingleton<HashCrypter>();
            services.AddSingleton<TokenCrypter>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
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
