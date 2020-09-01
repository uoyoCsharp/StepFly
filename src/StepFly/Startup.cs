using MiCake;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using StepFly.EFCore;
using System;

namespace StepFly
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<StepFlyDbContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("DBConnection"), mySqlOptions =>
                   {
                       mySqlOptions.ServerVersion(new ServerVersion(new Version(10, 5, 0), ServerType.MariaDb));
                   });
            });

            services.AddMiCakeWithDefault<StepFlyDbContext, StepFlyModule>()
                    .Build();

            services.AddHttpClient();

            services.AddSwaggerDocument();

            services.AddCors(builder =>
            {
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                using var scope = app.ApplicationServices.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<StepFlyDbContext>();
                dbContext.Database.EnsureCreated();
            }

           // app.UseHttpsRedirection(); ²»Æô¶¯https
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin().AllowAnyHeader();
            });
            app.StartMiCake();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseOpenApi();
            app.UseSwaggerUi3();
        }
    }
}
