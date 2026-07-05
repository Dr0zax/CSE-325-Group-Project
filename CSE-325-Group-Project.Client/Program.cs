using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CSE325project.Client;
using CSE325project.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped<RoomService>();
builder.Services.AddScoped<ReservationService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AmenityService>();
builder.Services.AddScoped<RoomAmenityService>();
builder.Services.AddScoped<ReservationService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped(_ => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5034/")
});



await builder.Build().RunAsync();
