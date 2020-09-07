using MiCake.Identity.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace StepFly.Extensions
{
    public static class JWTServiceExtension
    {
        public static IServiceCollection AddJwtService(this IServiceCollection services, IConfiguration configuration)
        {
            var seurityKey = Encoding.Default.GetBytes(configuration["JwtConfig:SecurityKey"]);

            services.PostConfigure<MiCakeJwtOptions>(jwtOptions =>
            {
                jwtOptions.Audience = configuration["JwtConfig:Audience"];
                jwtOptions.Issuer = configuration["JwtConfig:Issuer"];
                jwtOptions.ExpirationMinutes = int.Parse(configuration["JwtConfig:ExpireDay"]) * 24 * 60;
                jwtOptions.SecurityKey = seurityKey;
            });

            var result = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                                 .AddJwtBearer(jwtOptions =>
                                 {
                                     jwtOptions.TokenValidationParameters = new TokenValidationParameters()
                                     {
                                         ValidateAudience = false,
                                         IssuerSigningKey = new SymmetricSecurityKey(seurityKey),
                                         ValidIssuer = configuration["JwtConfig:Issuer"],
                                         ValidAudience = configuration["JwtConfig:Audience"],
                                     };
                                 });

            return services;
        }
    }
}
