﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TennisBookings.Web.Core.DependencyInjection;
using TennisBookings.Web.Data;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using TennisBookings.Web.Configuration;
using TennisBookings.Web.Services;

namespace TennisBookings.Web
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
            services.AddDbContext<TennisBookingDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            
            // services.Configure<HomePageConfiguration>(Configuration.GetSection("Features:HomePage"));

            services.AddOptions<HomePageConfiguration>()
                .Bind(Configuration.GetSection("Features:HomePage"))
                .Validate(x =>
                {
                    if (x.EnableWeatherForecast && string.IsNullOrEmpty(x.ForecastSectionTitle))
                    {
                        return false;
                    }

                    return true;
                }, "section title must be provided.");
                //.ValidateDataAnnotations();

            services.Configure<GreetingConfiguration>(Configuration.GetSection("Features:Greeting"));
            services.Configure<ExternalServiceConfiguration>(ExternalServiceConfiguration.WeatherApi, 
                Configuration.GetSection("ExternalServices:WeatherApi"));
            services.Configure<ExternalServiceConfiguration>(ExternalServiceConfiguration.ProductsApi,
                Configuration.GetSection("ExternalServices:ProductsApi"));

            services.AddHostedService<TimedHostedService>();

            services
                .AddAppConfiguration(Configuration)
                .AddBookingServices()
                .AddBookingRules()
                .AddCourtUnavailability()
                .AddMembershipServices()
                .AddStaffServices()
                .AddCourtServices()
                .AddWeatherForecasting(Configuration)
                .AddExternalProducts()
                .AddNotifications()                
                .AddGreetings()
                .AddCaching()
                .AddTimeServices()
                .AddAuditing();

            //services.Configure<ExternalServiceConfig>(
            //    Configuration.GetSection("ExternalServices"));

            services.AddControllersWithViews();
            services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizePage("/FindAvailableCourts");
                options.Conventions.AuthorizePage("/BookCourt");
                options.Conventions.AuthorizePage("/Bookings");
            });

            services.AddIdentity<TennisBookingsUser, TennisBookingsRole>()
                .AddEntityFrameworkStores<TennisBookingDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
