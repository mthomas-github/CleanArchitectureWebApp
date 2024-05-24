using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using ThirdPartyFreight.Web;
using ThirdPartyFreight.Web.Pages;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:28081") });

builder.Services.AddOidcAuthentication(options =>
{
    options.ProviderOptions.MetadataUrl = "http://localhost:18080/realms/csd-tpf/.well-known/openid-configuration";
    options.ProviderOptions.Authority = "http://localhost:18080/realms/csd-tpf";
    options.ProviderOptions.ClientId = "csd-tpf-web";
    options.ProviderOptions.ResponseType = "id_token token";
    
    options.UserOptions.NameClaim = "preferred_username";
    options.UserOptions.RoleClaim = "roles";
    options.UserOptions.ScopeClaim = "scope";
});

builder.Services.AddSingleton<HubConnection>(_ => new HubConnectionBuilder()
        .WithUrl("https://localhost:28081/approval_payloads")
        .WithAutomaticReconnect()
        .Build());


builder.Services.AddSingleton<Home.IssuesGenerator>();
builder.Services.AddTelerikBlazor();
builder.Services.AddBlazoredLocalStorage();
await builder.Build().RunAsync();
