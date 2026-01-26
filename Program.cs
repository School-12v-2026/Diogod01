using Microsoft.EntityFrameworkCore;
using EasyShop2.Data;
using EasyShop2.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ------------------------
        // Services
        // ------------------------
        builder.Services.AddControllersWithViews();

        // DbContext с SQL Server
        builder.Services.AddDbContext<ShopContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        );

        // Session за role-based access
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
        });

        // IHttpContextAccessor е нужно за @inject в Razor
        builder.Services.AddHttpContextAccessor();

        var app = builder.Build();

        // ------------------------
        // Създаване на база и първоначални данни
        // ------------------------
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ShopContext>();

            db.Database.EnsureCreated(); // създава базата и таблиците автоматично

            if (!db.Users.Any())
            {
                db.Users.Add(new User
                {
                    Username = "admin",
                    Password = "1234",
                    Role = "Admin"
                });

                db.Users.Add(new User
                {
                    Username = "user1",
                    Password = "1111",
                    Role = "User"
                });

                db.Users.Add(new User
                {
                    Username = "user2",
                    Password = "2222",
                    Role = "User"
                });

                db.Products.Add(new Product
                {
                    Name = "Mouse",
                    Price = 25,
                    ImageUrl = "/images/mouse.jpg"
                });

                db.Products.Add(new Product
                {
                    Name = "Keyboard",
                    Price = 50,
                    ImageUrl = "/images/keyboard.jpg"
                });

                db.SaveChanges();
            }
        }

        // ------------------------
        // Middleware
        // ------------------------
        app.UseStaticFiles();
        app.UseRouting();
        app.UseSession(); // важно за role-based проверки
        app.UseAuthorization();

        // ------------------------
        // Routing
        // ------------------------
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Account}/{action=Login}/{id?}"
        );

        app.Run();
    }
}
