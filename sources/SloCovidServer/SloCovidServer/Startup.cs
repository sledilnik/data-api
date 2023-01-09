using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus;
using SloCovidServer.Formatters;
using SloCovidServer.Services.Abstract;
using SloCovidServer.Services.Implemented;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading;

namespace SloCovidServer
{
    public class Startup
    {
        const string SchemaVersion = "48";
        const string CorsPolicy = "Any";
        readonly IWebHostEnvironment env;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            this.env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<ICommunicator, Communicator>();
            services.AddSingleton<Mapper>();
            services.AddSingleton<HttpClient>();
            services.AddSingleton<ISlackService, SlackService>();

            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicy, builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders("Timestamp", "Etag");
                });
            });
            services.AddResponseCompression();
            // don't include null value properties in JSON content to limit content payload
            services.AddMvc(options =>
                {
                    if (!env.IsDevelopment())
                    {
                        options.CacheProfiles.Add(nameof(CacheProfiles.Default60),
                           new CacheProfile()
                           {
                               VaryByQueryKeys = new[] { "*" },
                               VaryByHeader = "Accept",
                               Duration = 60
                           });
                    }
                    else
                    {
                        // no cache for development
                        options.CacheProfiles.Add(nameof(CacheProfiles.Default60),
                            new CacheProfile()
                            {
                                NoStore = true,
                            });
                    }
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                })
                // adds support for text/csv output, won't work for nested structures
                .AddCsvSerializerFormatters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ISlackService slackService, ICommunicator communicator)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors(CorsPolicy);
            app.UseResponseCompression();
            app.UseResponseCaching();

            app.UseAuthorization();
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("SchemaVersion", SchemaVersion);
                context.Response.GetTypedHeaders().CacheControl =
                    new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = System.TimeSpan.FromSeconds(10)
                    };
                context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
                    new string[] { "Accept-Encoding" };
                await next.Invoke();
            });

            // notifies slack when an exception occurs
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    //var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    //var exception = exceptionHandlerPathFeature?.Error;
                    try
                    {
                        await slackService.SendNotificationAsync($"DATA API REST service failed on {context.Request?.Path}", CancellationToken.None);
                    }
                    catch
                    { }
                });
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapMetrics();
                endpoints.MapControllers();
            });
        }
    }
}
