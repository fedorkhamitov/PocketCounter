using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PocketCounter.Application.Authorization;
using PocketCounter.Application.Authorization.DataModels;

namespace PocketCounter.Infrastructure.Authentication;

public static class Inject
{
    public static IServiceCollection AddInfrastructureAuth(this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddTransient<ITokenProvider, JwtTokenProvider>();
        
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.JWT));

        services.AddOptions<JwtOptions>();
        
        services
            .AddIdentity<User, Role>(options => { options.User.RequireUniqueEmail = true; })
            .AddEntityFrameworkStores<AuthorizationDbContext>()
            .AddDefaultTokenProviders();
        
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtOptions = configuration.GetSection(JwtOptions.JWT).Get<JwtOptions>()
                                 ?? throw new ApplicationException("Missing JWT configs");
                
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes(jwtOptions.Key)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true
                };
            });
        
        services.AddScoped<AuthorizationDbContext>();

        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireClaim(ClaimTypes.Role, "User")
                .RequireAuthenticatedUser()
                .Build();
            
            options.AddPolicy("CreateIssueRequirement", policy =>
            {
                policy.AddRequirements(new CreateIssueRequirement("Issue"));
            });
        });

        services.AddSingleton<IAuthorizationHandler, CreateIssueRequirementHandler>();
        
        return services;
    }
}