using MiCake.Identity.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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

            var events = new JwtBearerEvents();
            events.OnTokenValidated += AccountActiveValidate;
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

                                     jwtOptions.Events = events;
                                 });

            services.AddAuthorization(options =>
                  options.AddPolicy("Admin",
                  policy => policy.RequireClaim(ClaimTypes.Role, "admin")));

            return services;
        }

        static Task AccountActiveValidate(TokenValidatedContext context)
        {
            var activeClaim = context.Principal.FindFirst("lockout");
            if (activeClaim != null && activeClaim.Value.Equals("1"))
            {
                context.Fail("账号被锁定");
            }

            return Task.CompletedTask;
        }
    }
}
