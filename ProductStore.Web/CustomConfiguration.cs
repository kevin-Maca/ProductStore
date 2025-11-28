using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductStore.Web.Data;
using ProductStore.Web.Data.Entities;
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

            //Identity and Access Management
            AddIAM(builder);

            // Services
            AddServices(builder);

            builder.Services.AddHttpContextAccessor();

            return builder;
        }

        private static void AddIAM(WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<User, IdentityRole>(conf =>
            {
                conf.User.RequireUniqueEmail = true;
                conf.Password.RequireDigit = false;
                conf.Password.RequiredUniqueChars = 0;
                conf.Password.RequireLowercase = false;
                conf.Password.RequireUppercase = false;
                conf.Password.RequireNonAlphanumeric = false;
                conf.Password.RequiredLength = 4;
            }).AddEntityFrameworkStores<DataContext>()
              .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(conf =>
            {
                conf.Cookie.Name = "Auth";
                conf.ExpireTimeSpan = TimeSpan.FromDays(100);
                conf.LoginPath = "/Account/Login";
                conf.AccessDeniedPath = "/Error/403";
            });

            builder.Services.AddAuthentication()
                            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                            {
                                options.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuer = true,
                                    ValidateAudience = true,
                                    ValidateLifetime = true,
                                    ValidateIssuerSigningKey = true,
                                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"])),
                                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                                    ValidAudience = builder.Configuration["Jwt:Audience"],
                                    ClockSkew = TimeSpan.Zero
                                };
                            });
        }

        private static void AddServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICategoryServices, CategoryServices>();
            builder.Services.AddScoped<IProductServices, ProductServices>();
            builder.Services.AddScoped<IHomeServices, HomeServices>();
            builder.Services.AddScoped<IRolesServices, RolesServices>();
            builder.Services.AddScoped<IUsersServices, UsersServices>();

            builder.Services.AddTransient<SeedDb>();
            builder.Services.AddTransient<ICombosHelper, CombosHelper>();

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
