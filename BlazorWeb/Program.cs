using BlazorWeb.Components;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
// Dodavanje SQLite baze podataka
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlite(connectionString, sqliteOptions =>
    {
        sqliteOptions.CommandTimeout(60); // Povećan timeout
    });
});
// Dodavanje Identity s GUID-om
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
// Dodavanje autentifikacije i autorizacije
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
// Registracija servisa u Program.cs
builder.Services.AddScoped<Domain.Interfaces.IUserRepository, Infrastructure.Repositories.UserRepository>();
builder.Services.AddScoped<Application.Interfaces.IUserService, Application.Services.UserService>();
var app = builder.Build();
// Automatsko kreiranje baze podataka pri pokretanju
// Automatsko kreiranje baze podataka i uloga pri pokretanju
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        // Ovo će kreirati bazu ako ne postoji
        context.Database.EnsureCreated();
        // Inicijalizacija uloga
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        // Provjera i kreiranje uloga
        string[] roles = new[] { "Admin", "User", "Trainer" };
        // Koristite Task.Run umjesto await
        Task.Run(async () =>
        {
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }
        }).GetAwaiter().GetResult(); // Sinhrono čekanje na rezultat
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Došlo je do greške prilikom stvaranja baze podataka ili inicijalizacije uloga.");
    }
}
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
// Dodavanje Authentication i Authorization Middleware
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.Run();