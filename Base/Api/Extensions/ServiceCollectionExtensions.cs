using BaseApi.Auth.Constants;
using BaseApi.Auth.Handlers;
using BaseApi.Auth.Requirements;
using BaseApi.ModelBinders;
using BaseApi.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.Serialization.Conventions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using BaseInfraestructure.AuditModels;
using BaseInfraestructure.Messaging.Helpers;
using BaseInfraestructure.Messaging.Models;
using Microsoft.AspNetCore.Http;

namespace BaseApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var secretKey = configuration[ConfigurationKeys.UserJwtKey];
        
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                };
            });
        
         services.AddAuthorizationBuilder()
             .AddPolicy(Policies.Administrator, policy => policy.Requirements.Add(new AdministratorRequirement()))
             .AddPolicy(Policies.Client, policy => policy.Requirements.Add(new ClientRequirement()));
         
         services.AddSingleton<IAuthorizationHandler, AdministratorAuthorizationHandler>();
         services.AddSingleton<IAuthorizationHandler, ClientAuthorizationHandler>();
         
         services.AddScoped<UserIdentification>((sp) =>
         {
             var httpContext = sp.GetService<IHttpContextAccessor>();
             return httpContext?.HttpContext?.GetUserIdentification() ?? new UserIdentification();
         });
    }

    public static void AddSwaggerAuthentication(this IServiceCollection service)
    {
        const string authSchemeDescription =
            "Autenticação com Token JWT.\n" +
            "Gere o token a partir do POST **Adicionar endpoint**.\n" + 
            "Ao logar utilizando o token utilize o prefixo 'Bearer'.";
        
        service.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(
                JwtBearerDefaults.AuthenticationScheme,
                new OpenApiSecurityScheme
                {
                    Description = authSchemeDescription,
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                }
            );
    
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    { 
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        },
                    },
                    Array.Empty<string>()
                },
            });
        });
    }
    
    public static void AddJsonConfigs(this IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.SerializerOptions.WriteIndented = true;
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        })
        .AddControllers(options =>
        {
            options.ModelBinderProviders.Insert(0, new WhiteSpaceStringBinderProvider());
        });
        
        var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
        ConventionRegistry.Register("camelCase", conventionPack, _ => true);
    }
    
    public static void AddRabbitMQServices(this IServiceCollection services, IConfiguration configuration, Type[]? consumers = null)
    {
        #if DEBUG
        ConfigurationHelper.RegisterRabbitMQConnections(
            services,
            new RabbitMQConfig
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest",
                ConnectionName = "getajob"
            });
        #else
        var rabbitConfigurations = configuration.GetRabbitMQConfiguration();
            ConfigurationHelper.RegisterRabbitMQConnections(
                services,
                new RabbitMQConfig
                {
                    HostName = rabbitConfigurations.HostName,
                    Port = rabbitConfigurations.Port,
                    UserName = rabbitConfigurations.UserName,
                    Password = rabbitConfigurations.Password,
                    ConnectionName = "getajob"
                });

            if (!rabbitConfigurations.IsValid || !rabbitConfigurations.Active) return;
        #endif

        RegisterConsumers(consumers, services);
    }

    private static void RegisterConsumers(Type[]? consumers, IServiceCollection services)
    {
        foreach (var consumer in consumers ?? [])
        {
            services.AddScoped(consumer);
        }
    }
}