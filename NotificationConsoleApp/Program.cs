using InvestigationCaseManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using InvestigationCaseManagement.Services;

class Program
{
    /// <summary>
    // La función Main en C# crea un host, recupera un NotificationService del proveedor de servicios 
    // y verifica y envía notificaciones de forma asincrónica.
    /// </summary>
    static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var notificationService = services.GetRequiredService<NotificationService>();
                await notificationService.CheckAndSendNotificationsAsync(); //Llamada al metodo asincrono encargado de enviar notificaciones.
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    /// <summary>
    // La función CreateHostBuilder configura un generador de host en C# para configurar servicios 
    // como el contexto de base de datos y el servicio de notificación.
    /// </summary>
    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));
                services.AddTransient<NotificationService>();
                services.AddHttpContextAccessor();
            });
}