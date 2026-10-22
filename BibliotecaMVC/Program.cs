using BibliotecaMVC.Services;
using BibliotecaMVC.Respositories;
using Microsoft.EntityFrameworkCore;
using BibliotecaMVC.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<NumericRoundabortConnectionInterceptor>();
builder.Services.AddDbContext<BibliotecaContext>((serviceProvider, options) => {
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("BibliotecaDB"));

    options.AddInterceptors(
        serviceProvider.GetRequiredService<NumericRoundabortConnectionInterceptor>());
});

// Configuración de Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.AllowedForNewUsers = true;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<BibliotecaContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Registro de servicios
builder.Services.AddScoped<IAutorService, AutorService>();
builder.Services.AddSingleton<IRepositorioLibro, RepositorioEnMemoria>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
   var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
   var usuarios = await userManager.Users.ToListAsync();
   foreach (var usuario in usuarios)
    {
       if((await userManager.GetRolesAsync(usuario)).Count == 0)
        {
            var result = await userManager.AddToRoleAsync(usuario, "Usuario");
            if(!result.Succeeded)
            {
                throw new InvalidOperationException(
                    $"No se pudo asignar el rol 'Usuario' al usuario {usuario.UserName}. Errores: {string.Join(", ", result.Errors.Select(error => error.Description))}"
                    );

            }
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Creacion automatica de roles y asignacion
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    string[] roles = { "Admin", "Usuario" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Reemplaza con el correo de tu usuario administrador existente
    // Nombre: Admin Email : Admin@biblioteca.com y contra Admin123_
    var adminEmail = "Admin@biblioteca.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser != null && !await userManager.IsInRoleAsync(adminUser, "Admin"))
    {
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

app.Run();