using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.EntityFrameworkCore;
using ProductStore.Web.Data;
using ProductStore.Web.Data.Seeders;
using ProductStore.Web.Helpers.Abstractions;
using ProductStore.Web.Helpers.Implementations;
using ProductStore.Web.Services.Abstractions;
using ProductStore.Web.Services.Implementations;

namespace ProductStore.Web
{
    public static class CustomConfiguration
    {
        public static WebApplicationBuilder AddCustomConfiguration(this WebApplicationBuilder builder)
        {
            string? cnn = builder.Configuration.GetConnectionString("MyConnection");
            // Data Context
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"));
            });

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(Program));

            // Toast Notification Setup
            builder.Services.AddNotyf(config =>
            {
                config.DurationInSeconds = 10;
                config.IsDismissable = true;
                config.Position = NotyfPosition.BottomRight;
            });

            // Services
            AddServices(builder);

            return builder;
        }

        private static void AddServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICategoryServices, CategoryServices>();
            builder.Services.AddScoped<IProductServices, ProductServices>();

            builder.Services.AddTransient<SeedDb>();

            builder.Services.AddTransient<ICombosHelper, CombosHelper>();
            builder.Services.AddTransient<SeedDb>();
        }

        public static WebApplication AddCustomWebApplicationConfiguration(this WebApplication app)
        {
            app.UseNotyf();
            SeedData(app);

            return app;
        }
        private static void SeedData(WebApplication app)
        {
            IServiceScopeFactory scopeFactory = app.Services.GetService<IServiceScopeFactory>();

            using IServiceScope scope = scopeFactory.CreateScope();
            SeedDb service = scope.ServiceProvider.GetService<SeedDb>();
            service.SeedAsync().Wait();
        }
    }
}
