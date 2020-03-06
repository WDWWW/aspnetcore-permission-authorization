using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wd3w.TokenAuthentication;

namespace Wd3w.AspNetCore.Permission.Sample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAuthentication("Bearer")
                .AddTokenAuthenticationScheme<FakeTokenAuthService>("Bearer", new TokenAuthenticationConfiguration
                {
                    Realm = "www.sample.com",
                    TokenLength = 11
                });
            services.AddAuthorization();
            services.AddPermissionServices<FakePermissionProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }

    public class FakeTokenAuthService : ITokenAuthService
    {
        public Task<bool> IsValidateAsync(string token)
        {
            return Task.FromResult(true);
        }

        public Task<ClaimsPrincipal> GetPrincipalAsync(string token)
        {
            return Task.FromResult(new ClaimsPrincipal(new ClaimsIdentity(new []
            {
                new Claim(ClaimTypes.Email, "test@test.com"),
            })));
        }
    }

}