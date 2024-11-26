using CSCI2910_Lab5.Components;
using CSCI2910_Lab5.Services;
using CSCI2910_Lab5.Models;

namespace CSCI2910_Lab5
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<Book> borrowedBooks = new List<Book>();

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            // Register LibraryService
            builder.Services.AddSingleton<LibraryService>();

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
