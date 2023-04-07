using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

using Radzen;
using WebApplication.Services;
using BeautySaloon.Api.Services;
using WebApplication.Handlers;
using Refit;
using WebApplication.Profiles;
using WebApplication.Wrappers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using BeautySaloon.Api.Dto.Common;

namespace WebApplication
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        partial void OnConfigureServices(IServiceCollection services);

        partial void OnConfiguringServices(IServiceCollection services);

        public void ConfigureServices(IServiceCollection services)
        {
            OnConfiguringServices(services);

            services.AddHttpContextAccessor();
            services.AddScoped<HttpClient>(serviceProvider =>
            {

              var uriHelper = serviceProvider.GetRequiredService<NavigationManager>();

              return new HttpClient
              {
                BaseAddress = new Uri(uriHelper.BaseUri)
              };
            });

            services.AddHttpClient();
            services.AddScoped<LocalDbService>();

            services.AddDbContext<WebApplication.Data.LocalDbContext>(options =>
            {
              options.UseNpgsql(Configuration.GetConnectionString("LocalDbConnection"));
            });

            services.AddRazorPages();
            services.AddServerSideBlazor().AddHubOptions(o =>
            {
                o.MaximumReceiveMessageSize = 10 * 1024 * 1024;
            });

            services.AddScoped<DialogService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<TooltipService>();
            services.AddScoped<ContextMenuService>();

            services.AddTransient<CustomRefitErrorHandler>();

            services.AddScoped<IHttpClientWrapper, HttpClientWrapper>();

            var refitSettings = new RefitSettings
            {
                UrlParameterFormatter = new CustomUrlParameterFormatter(),
                ContentSerializer = new NewtonsoftJsonContentSerializer(new JsonSerializerSettings
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                    DateFormatString = DateTimeFormats.DateTimeWithTimeZoneFormat
                })
            };

            services.AddRefitClient<IAuthHttpClient>(refitSettings)
                .ConfigureHttpClient(_ => _.BaseAddress = new Uri("http://localhost:40001"))
                .AddHttpMessageHandler<CustomRefitErrorHandler>();

            services.AddRefitClient<IUserHttpClient>(refitSettings)
                .ConfigureHttpClient(_ => _.BaseAddress = new Uri("http://localhost:40001"))
                .AddHttpMessageHandler<CustomRefitErrorHandler>();

            services.AddRefitClient<IPersonHttpClient>(refitSettings)
                .ConfigureHttpClient(_ => _.BaseAddress = new Uri("http://localhost:40001"))
                .AddHttpMessageHandler<CustomRefitErrorHandler>();

            services.AddRefitClient<ICosmeticServiceHttpClient>(refitSettings)
                .ConfigureHttpClient(_ => _.BaseAddress = new Uri("http://localhost:40001"))
                .AddHttpMessageHandler<CustomRefitErrorHandler>();

            services.AddRefitClient<ISubscriptionHttpClient>(refitSettings)
                .ConfigureHttpClient(_ => _.BaseAddress = new Uri("http://localhost:40001"))
                .AddHttpMessageHandler<CustomRefitErrorHandler>();

            services.AddRefitClient<IOrderHttpClient>(refitSettings)
                .ConfigureHttpClient(_ => _.BaseAddress = new Uri("http://localhost:40001"))
                .AddHttpMessageHandler<CustomRefitErrorHandler>();

            services.AddRefitClient<IAppointmentHttpClient>(refitSettings)
                .ConfigureHttpClient(_ => _.BaseAddress = new Uri("http://localhost:40001"))
                .AddHttpMessageHandler<CustomRefitErrorHandler>();

            services.AddAutoMapper(typeof(UserProfile).Assembly);

            OnConfigureServices(services);
        }

        partial void OnConfigure(IApplicationBuilder app, IWebHostEnvironment env);
        partial void OnConfiguring(IApplicationBuilder app, IWebHostEnvironment env);

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            OnConfiguring(app, env);
            if (env.IsDevelopment())
            {
                Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.Use((ctx, next) =>
                {
                    return next();
                });
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            OnConfigure(app, env);
        }
    }


}
