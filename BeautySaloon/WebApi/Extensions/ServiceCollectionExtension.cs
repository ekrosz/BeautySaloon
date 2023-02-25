using BeautySaloon.Core.Dto.Requests.Auth;
using BeautySaloon.Core.Services;
using BeautySaloon.Core.Services.Contracts;
using BeautySaloon.Core.Settings;
using BeautySaloon.DAL;
using BeautySaloon.DAL.Providers;
using BeautySaloon.DAL.Repositories.Abstract;
using BeautySaloon.DAL.Settings;
using BeautySaloon.DAL.Uow;
using BeautySaloon.WebApi.Providers;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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

    public static IServiceCollection AddBusinessLayer(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();

        services.AddValidatorsFromAssembly(typeof(AuthorizeByCredentialsRequestDto).Assembly);
        services.AddAutoMapper(typeof(AuthorizeByCredentialsRequestDto).Assembly);

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
                ValidateIssuerSigningKey = true
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
