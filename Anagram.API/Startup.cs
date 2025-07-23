using System;
using Azure.Core;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AnagramAPI
{
    /// <summary>
    /// Start up class
    /// </summary>
    /// <author>Michael</author>
    /// <datetime>5/25/2017 7:03 PM</datetime>
    /// <remarks>Start up class</remarks>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Startup"/> class.
    /// </remarks>
    /// <param name="configuration">The configuration.</param>
    public class Startup(IConfiguration configuration)
    {
        private string _connection = null;

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public IConfiguration Configuration { get; } = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMemoryCache();
            services.AddDbContext<DictionaryDBContext>((DbContextOptionsBuilder options) => { options.UseSqlServer(_connection); });
            //services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            var builder = new SqlConnectionStringBuilder(Configuration.GetConnectionString("SBLDB"));
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else if (env.IsProduction())
            {
                SecretClientOptions options = new()
                {
                    Retry =
                            {
                                Delay= TimeSpan.FromSeconds(2),
                                MaxDelay = TimeSpan.FromSeconds(16),
                                MaxRetries = 5,
                                Mode = RetryMode.Exponential
                             }
                };
            }

            _connection = builder.ConnectionString;

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