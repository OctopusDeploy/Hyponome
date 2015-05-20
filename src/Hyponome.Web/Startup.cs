using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Hosting;
using Microsoft.Framework.ConfigurationModel;
using Hyponome.Core;

namespace Hyponome
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var configuration = new Configuration()
                .AddJsonFile("config.json")
                .AddJsonFile($"config.{env.EnvironmentName}.json", optional: true);
                
            if(env.IsEnvironment("Development"))
            {
                configuration.AddUserSecrets();
            }
            configuration.AddEnvironmentVariables();
            Configuration = configuration;
        }
        
        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSubKey("AppSettings"));
            services.Configure<GithubOptions>(Configuration.GetSubKey("Github"));
            services.AddSingleton(typeof(IGithubClientService), typeof(GithubClientService));
            services.AddCaching();
            services.AddSession();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IGithubClientService githubClientService)
        {
            loggerFactory.AddConsole(minLevel: LogLevel.Warning);
            
            app.UseCookieAuthentication(options => {
                options.AutomaticAuthentication = true;
                options.LoginPath = new PathString("/account/login");
            });
            
            app.Use(async (context, next) => {
//                 if (string.IsNullOrEmpty(context.User.Identity.Name))
//                 {
//                     context.Response.Challenge();
//                 }
//                 else
//                 {
                    if(!string.IsNullOrEmpty(context.User.Identity.Name) && githubClientService.CurrentUser == null)
                    {
                        System.Console.WriteLine("Authenticating GitHub client for {0}", context.User.Identity.Name);
                        await githubClientService.SetCredentials(context.User.FindFirst("urn:github:accessToken").Value);
                    }
//                 }
                
                await next();
            });
            
            app.UseStaticFiles();

            if(env.IsEnvironment("Development"))
            {
                app.UseErrorPage(ErrorPageOptions.ShowAll);
            }
            else
            {
                app.UseErrorHandler("/404.html");
            }
            
            app.UseSession();
            app.UseMvc();
        }
    }
}
