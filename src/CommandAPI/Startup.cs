using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CommandAPI.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CommandAPI
{
    public class Startup
    {
        public IConfiguration Configuration {get;}
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            //SECTION 1. Add code below

            services.AddDbContext<CommandContext>(options => options.UseNpgsql(Configuration.GetConnectionString("PostgreSqlConnection")));
            services.AddControllers();

            services.AddScoped<ICommandAPIRepo, MockCommandAPIRepo>();
        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            //We map controllers to our endpoints. This means we make use of the Controller services as endpoints in the Request Pipeline
            app.UseEndpoints(endpoints =>
            {
                //Section 2. Add code below
                endpoints.MapControllers();
                
            });
        }
    }
}
