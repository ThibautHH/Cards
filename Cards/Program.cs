using System.Globalization;

using Cards.Data;
using Cards.Server.Hubs;
using Cards.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;

using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;

namespace Cards
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            void configureServices()
            {
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        builder.Configuration.GetConnectionString("Cards")));
                builder.Services.AddDatabaseDeveloperPageExceptionFilter();

                builder.Services.AddDefaultIdentity<IdentityUser>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

                //builder.Services.AddSingleton<IEmailSender, EmailSender>();

                builder.Services.AddHttpContextAccessor();
                
                builder.Services.AddScoped<HttpContextService>();

                builder.Services.AddTransient<TrickFactory>();

                builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

                builder.Services.Configure<RequestLocalizationOptions>(options =>
                {
                    options.DefaultRequestCulture = new RequestCulture(culture: "fr", uiCulture: "fr");
                    options.SupportedCultures =
                        options.SupportedUICultures = new[]
                            {
                                new CultureInfo("fr"),
                                new CultureInfo("en")
                            };
                });

                builder.Services.AddRazorPages().AddMvcLocalization(LanguageViewLocationExpanderFormat.Suffix);
                builder.Services.AddServerSideBlazor();
                builder.Services.AddSignalR();

                void configureAuthenticationAndAuthorization()
                {
                    builder.Services.Configure<IdentityOptions>(options =>
                    {
                        options.Password = new()
                        {
                            RequireUppercase = true,
                            RequireLowercase = true,
                            RequireDigit = true,
                            RequireNonAlphanumeric = true,
                            RequiredLength = 8,
                            RequiredUniqueChars = 1
                        };

                        options.Lockout = new()
                        {
                            DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5.0),
                            MaxFailedAccessAttempts = 10,
                            AllowedForNewUsers = true
                        };

                        options.SignIn = new()
                        {
                            RequireConfirmedAccount = true,
                            RequireConfirmedEmail = true,
                            RequireConfirmedPhoneNumber = false
                        };

                        options.User = new()
                        {
                            RequireUniqueEmail = true
                        };
                    });

                    builder.Services.AddAuthentication(options => options.RequireAuthenticatedSignIn = true);

                    builder.Services.AddAuthorization();
                }
                configureAuthenticationAndAuthorization();
            }
            configureServices();

            WebApplication application = builder.Build();

            void configure()
            {
                application.UseRequestLocalization();

                if (application.Environment.IsDevelopment())
                {
                    application.UseDeveloperExceptionPage();
                    application.UseMigrationsEndPoint();
                } else
                {
                    application.UseExceptionHandler("/Error");
                    application.UseHsts();
                }

                application.UseHttpsRedirection();
                application.UseStaticFiles();

                application.UseRouting();

                application.UseAuthentication();
                application.UseAuthorization();

                application.MapBlazorHub();
                application.MapHub<PlayerListHub>("/Hubs/PlayerList");
                application.MapHub<TarotHub>("/Hubs/Tarot");
                application.MapFallbackToPage("/_Host");
            }
            configure();

            application.Run();
        }
    }
}
