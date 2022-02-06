using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Hyponome.Web.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace Hyponome.Web
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
            services.Configure<GitHubOptions>(Configuration.GetSection("GitHub"));
            services.AddSingleton(typeof(IGitHubClientService), typeof(GitHubClientService));
            services.AddMvc(x => x.EnableEndpointRouting = false);
            services.AddLogging(builder => builder.AddConsole().AddFilter(level => level >= LogLevel.Warning));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/404.html");
            }

            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}
