using System;
using System.IO;
using System.Reflection;
using CalorieTracker.Data;
using CalorieTracker.Extensions;
using CalorieTracker.Helpers;
using CalorieTracker.Mappings;
using CalorieTracker.Models;
using CalorieTracker.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.PlatformAbstractions;

namespace CalorieTracker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DbContext>(); 
            services.AddIdentity<AppUser, AppRole>(_ => {
                _.Password.RequireNonAlphanumeric = false;
                _.Password.RequireUppercase = false;
                _.Lockout.MaxFailedAccessAttempts = Constants.AccessFailedCountThreshold;
                _.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(Constants.AccountLockoutDuration);
            }).AddPasswordValidator<CustomPasswordValidator>()
                .AddUserValidator<CustomUserValidator>()
                .AddEntityFrameworkStores<DbContext>();
            
            services.AddControllers();

            services.AddControllersWithViews()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressMapClientErrors = true;
                    options.SuppressModelStateInvalidFilter = true;
                })
                .AddFluentValidation();
            
            services.AddValidationServices();
            services.AddAutoMapper(Assembly.GetAssembly(typeof(IMappingProfile)));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "CalorieTracker", Version = "v1"});
                // integrate xml comments
                c.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath,
                    Assembly.GetExecutingAssembly().GetName().Name + ".xml"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CalorieTracker v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            // app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}