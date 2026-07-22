using BlazorApp4.Components;
using BlazorApp4.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContextFactory<PubsDbContext>(opt => {
    if (builder.Environment.IsDevelopment())
    {
        opt = opt.EnableSensitiveDataLogging().EnableDetailedErrors();
    }
    opt.UseSqlServer(
        builder.Configuration.GetConnectionString("PubsDbContext"),
        providerOptions =>
        {
            providerOptions.EnableRetryOnFailure();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
