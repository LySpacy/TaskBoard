using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;
using TaskBoard.Infrastructure.Authentication;

namespace TaskBoard.API.Extension
{
    public static class ApiExtensions
    {
        public static void AddApiAuthentication(
             this IServiceCollection services,
             IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions!.SecretKey))
                };


                options.Events = new JwtBearerEvents
                {

                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["jwtToken"];

                        return Task.CompletedTask;
                    }
                };

            })
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            services.AddAuthorization(options =>
            {
                
                options.AddPolicy("AdminOrManagerPolicy", policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireRole("Администратор", "Менеджер");
                });
            });
            services.AddAuthorization(options =>
            {
                
                options.AddPolicy("AdminPolicy", policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireRole("Администратор");
                });
            });
            services.AddAuthorization(options =>
            {

                options.AddPolicy("AllNotBanedUsers", policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireRole("Администратор", "Менеджер", "Пользователь");
                });
            });
        }
    }
}
