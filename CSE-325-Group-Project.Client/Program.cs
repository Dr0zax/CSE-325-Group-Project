using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CSE325project.Client;
using CSE325project.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => 
    new HttpClient { BaseAddress = new Uri("http://localhost:5034") });
builder.Services.AddScoped<UserService>();

await builder.Build().RunAsync();
