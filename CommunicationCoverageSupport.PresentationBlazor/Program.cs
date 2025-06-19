using Blazored.LocalStorage;

using CommunicationCoverageSupport.PresentationBlazor.Components;
using CommunicationCoverageSupport.PresentationBlazor.Services;
using CommunicationCoverageSupport.PresentationBlazor.Services.Acc;
using CommunicationCoverageSupport.PresentationBlazor.Services.SimCard;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISimCardService, SimCardService>();

builder.Services.AddHttpClient("ApiClient", (sp, client) =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var baseUrl = config.GetSection("ApiSettings:BaseUrl").Value;
    client.BaseAddress = new Uri(baseUrl!);
});

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IAccService, AccService>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
