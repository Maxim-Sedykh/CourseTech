using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CourseTech.DAL;

namespace CourseTech.CourseDb.Migrator
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            // Создаем хост для DI (Dependency Injection)
            var host = CreateHostBuilder(args).Build();

            // Запускаем миграции
            await MigrateDatabaseAsync(host);

            Console.WriteLine("Миграции выполнены. Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    // Здесь мы предполагаем, что ваш DbContext уже зарегистрирован в другом проекте
                    // Если у вас есть специфические настройки, вы можете добавить их здесь
                    services.AddDbContext<YourDbContext>(options =>
                        options.UseSqlServer(context.Configuration.GetConnectionString("YourConnectionString")));
                });

        private static async Task MigrateDatabaseAsync(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<YourDbContext>();
                await dbContext.Database.MigrateAsync();
            }
        }
    }
}
