using Abp.AspNetCore.Dependency;
using Abp.Dependency;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ProjectBlack.EntityFrameworkCore;

namespace ProjectBlack.Web.Host.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            if (args.Contains("--migrate"))
            {
                using (var scope = host.Services.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ProjectBlackDbContext>();
                    dbContext.Database.Migrate();
                }

                return; // Termina luego de ejecutar migraciones
            }

            host.Run();
        }

        internal static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseCastleWindsor(IocManager.Instance.IocContainer);
    }
}
