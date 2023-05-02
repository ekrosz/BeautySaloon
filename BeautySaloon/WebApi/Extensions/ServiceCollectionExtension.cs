using BeautySaloon.Api.Dto.Common;
using BeautySaloon.Api.Dto.Requests.Auth;
using BeautySaloon.Api.Services;
using BeautySaloon.Core.Api.SmartPay;
using BeautySaloon.Core.IntegrationServices.MailKit;
using BeautySaloon.Core.IntegrationServices.MailKit.Contracts;
using BeautySaloon.Core.IntegrationServices.SmartPay;
using BeautySaloon.Core.IntegrationServices.SmartPay.Contracts;
using BeautySaloon.Core.Jobs;
using BeautySaloon.Core.Profiles;
using BeautySaloon.Core.Services;
using BeautySaloon.Core.Services.Contracts;
using BeautySaloon.Core.Settings;
using BeautySaloon.Core.Utils;
using BeautySaloon.Core.Utils.Contracts;
using BeautySaloon.Core.Utils.Dto;
using BeautySaloon.DAL;
using BeautySaloon.DAL.Providers;
using BeautySaloon.DAL.Repositories.Abstract;
using BeautySaloon.DAL.Settings;
using BeautySaloon.DAL.Uow;
using BeautySaloon.WebApi.Providers;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Refit;
using System.Net.Http.Headers;

namespace BeautySaloon.WebApi.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDatabaseLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var dbSettings = configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();

        services.AddDbContext<BeautySaloonDbContext>(x => x.UseNpgsql(dbSettings.ConnectionString));

        services.AddScoped(typeof(IQueryRepository<>), typeof(QueryRepository<>));
        services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static IServiceCollection AddProviders(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();

        return services;
    }

    public static IServiceCollection AddBusinessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<ICosmeticServiceService, CosmeticServiceService>();
        services.AddScoped<ISubscriptionService, SubscriptionService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IMaterialService, MaterialService>();

        services.AddScoped<ISmartPayService, SmartPayService>();

        services.AddScoped<IDocumentGenerator<ReceiptRequestDto>, ReceiptDocumentGenerator>();

        services.Configure<BLayerSettings>(configuration.GetSection(nameof(BLayerSettings)));
        services.AddHostedService<RefreshPersonSubscriptionStatusJob>();
        services.AddHostedService<AppointmentNotificationJob>();

        services.AddValidatorsFromAssembly(typeof(AuthorizeByCredentialsRequestValidator).Assembly);
        services.AddAutoMapper(typeof(UserProfile).Assembly);

        return services;
    }

    public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection(nameof(BLayerSettings)).Get<BLayerSettings>();

        var refitSettings = new RefitSettings
        {
            UrlParameterFormatter = new CustomUrlParameterFormatter(),
            ContentSerializer = new NewtonsoftJsonContentSerializer(new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                DateFormatString = DateTimeFormats.DateTimeWithTimeZoneFormat
            })
        };

        services.AddRefitClient<ISmartPayHttpClient>(refitSettings)
                .ConfigureHttpClient(_ =>
                {
                    _.BaseAddress = new Uri(settings.SmartPaySettings.BaseUrl);
                    _.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(settings.SmartPaySettings.AuthScheme, settings.SmartPaySettings.AuthToken);
                })
                .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                });

        services.AddScoped<IMailKitService, MailKitService>();

        return services;
    }

    public static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        var authenticationSection = configuration.GetSection(nameof(AuthenticationSettings));

        var authenticationSettings = authenticationSection.Get<AuthenticationSettings>();

        services.Configure<AuthenticationSettings>(authenticationSection);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(opt =>
        {
            opt.RequireHttpsMetadata = false;
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = authenticationSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = authenticationSettings.Audience,
                ValidateLifetime = true,
                IssuerSigningKey = authenticationSettings.AccessSecurityKey,
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddSwaggerGen(setup =>
        {
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Description = "Enter token without \"Bearer\".",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

            setup.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    jwtSecurityScheme, Array.Empty<string>()
                }
            });
        });

        return services;
    }
}
