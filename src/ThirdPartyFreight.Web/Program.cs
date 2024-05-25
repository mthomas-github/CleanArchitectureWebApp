using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using ThirdPartyFreight.Web;
using ThirdPartyFreight.Web.Pages;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
WebAssemblyHostConfiguration configuration = builder.Configuration;
string? apiBaseUrl = configuration["ApiSettings:BaseUrl"] ?? throw new NullReferenceException("ApiSettings:BaseUrl not configured");
string? metadataUrl = configuration["AuthSettings:MetadataUrl"] ?? throw new NullReferenceException("AuthSettings:MetadataUrl not configured");
string? authority = configuration["AuthSettings:Authority"] ?? throw new NullReferenceException("AuthSettings:Authority not configured");
string? clientId = configuration["AuthSettings:ClientId"] ?? throw new NullReferenceException("AuthSettings:ClientId not configured");
string? responseType = configuration["AuthSettings:ResponseType"] ?? throw new NullReferenceException("AuthSettings:ResponseType not configured");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });

builder.Services.AddOidcAuthentication(options =>
{
    options.ProviderOptions.MetadataUrl = metadataUrl;
    options.ProviderOptions.Authority = authority;
    options.ProviderOptions.ClientId = clientId;
    options.ProviderOptions.ResponseType = responseType;
    
    options.UserOptions.NameClaim = "preferred_username";
    options.UserOptions.RoleClaim = "roles";
    options.UserOptions.ScopeClaim = "scope";
});

builder.Services.AddSingleton<HubConnection>(_ => new HubConnectionBuilder()
        .WithUrl($"{apiBaseUrl}/approval_payloads")
        .WithAutomaticReconnect()
        .Build());


builder.Services.AddSingleton<Home.IssuesGenerator>();
builder.Services.AddTelerikBlazor();
builder.Services.AddBlazoredLocalStorage();
await builder.Build().RunAsync();
