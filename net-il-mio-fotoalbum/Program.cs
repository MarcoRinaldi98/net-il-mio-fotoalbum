using Microsoft.AspNetCore.Identity;
using net_il_mio_fotoalbum.CustomLoggers;
using net_il_mio_fotoalbum.Database;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace net_il_mio_fotoalbum
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<PhotoContext>();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<PhotoContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Codice di configurazione per il serializzatore JSON, in modo che ignori completamente le eventuali relazioni 1:N e N:N presenti nel JSON risultante
            builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            // Custom Loggers
            builder.Services.AddScoped<ICustomLogger, CustomFileLogger>();
            builder.Services.AddScoped<PhotoContext, PhotoContext>();
            builder.Services.AddScoped<IRepositoryPhotos, RepositoryPhotos>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
                        
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.Run();
        }
    }
}