using axotlXires_ManagementConsoleNETBlazor.Components;
using Radzen;
using axotlXires_ManagementConsoleNETBlazor.Services;

namespace axotlXires_ManagementConsoleNETBlazor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHttpClient();

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.AddRadzenComponents();


            // Agregar servicios al contenedor
            builder.Services.AddRazorComponents().AddInteractiveServerComponents();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(DAL.Parametros.UrlBaseApi) });
            // ...
            //builder.Services.AddScoped<AuthService>();

            // En Program.cs
            builder.Services.AddScoped<DialogService>();
            builder.Services.AddScoped<NotificationService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
