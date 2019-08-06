using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using System.Threading.Tasks;
using Brainiac.Attribute;
using Brainiac.Contract;
using Brainiac.Entity;
using Brainiac.Services;
using Brainiac.System;

namespace Brainiac
{
    /// <summary>
    /// Startup helper class.
    /// </summary>
    public class StartUp
    {
        /// <summary>
        /// Configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Constructor that reads in data from the appsettings json config file and sets up the the 
        /// StartUp helper class with the config settings.
        /// </summary>
        public StartUp()
        {
            string appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder().AddJsonFile(appSettingsPath, false, true);
            Configuration = configurationBuilder.Build();
        }

        /// <summary>
        /// Configure Services.
        /// </summary>
        /// <param name="services">Services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddScoped<AuthenticationFilterAttribute>();

            // todo Add an attribute to allow the type of IRepository and config type to be set by config

            //services.Configure<FileRepoConfig>(Configuration);
            //services.AddSingleton<IRepository<Person, int>, FileSystemRepository<Person, int>>();
            services.Configure<ElasticSearchRepoConfig>(Configuration);
            services.AddSingleton<IRepository<Person>, ElasticSearchRepository<Person>>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("0.0.1", new Info { Title = "Brainiac API", Version = "0.0.1" });
            });

            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Trace);
                builder.AddConsole();
                builder.AddDebug();
                builder.AddProvider(new NLogLoggerProvider());
            });
        }

        /// <summary>
        /// Configure the application.
        /// </summary>
        /// <param name="app">Application Builder.</param>
        /// <param name="env">The hosting environment.</param>
        public void Configure(IApplicationBuilder app,
                    IHostingEnvironment env)
        {
            app.UseMiddleware<HttpExceptionMiddleware>();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/0.0.1/swagger.json", "Brainiac API 0.0.1");
            });

            app.Run(async (context) => await Task.Run(() => context.Response.Redirect("/swagger")));        }
    }
}
