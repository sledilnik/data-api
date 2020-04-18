using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus;
using SloCovidServer.Services.Abstract;
using SloCovidServer.Services.Implemented;
using System.Net.Http;
using System.Threading;

namespace SloCovidServer
{
    public class Startup
    {
        const string SchemaVersion = "13";
        const string CorsPolicy = "Any";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
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
                        .AllowAnyHeader();
                });
            });
            services.AddResponseCompression();
            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = $"v{SchemaVersion}";
                    document.Info.Title = "slo-covid-19 data API";
                    document.Info.Description = "SchemaVersion";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "Miha Markič",
                        Email = "miha@rthand.com",
                        Url = "https://blog.rthand.com/"
                    };
                    document.Info.License = new NSwag.OpenApiLicense
                    {
                        Name = "Use under MIT",
                        Url = "https://raw.githubusercontent.com/slo-covid-19/data-api/master/LICENSE"
                    };
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ISlackService slackService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors(CorsPolicy);
            app.UseResponseCompression();

            app.UseAuthorization();
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("SchemaVersion", SchemaVersion);
                await next.Invoke();
            });
            // Register the Swagger generator and the Swagger UI middleware
            app.UseOpenApi();
            app.UseSwaggerUi3();

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
                    {}
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
