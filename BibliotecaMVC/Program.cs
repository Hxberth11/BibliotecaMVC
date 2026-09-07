using BibliotecaMVC.Services;
using BibliotecaMVC.Respositories; // Importamos el namespace del repositorio

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Registro de servicios
builder.Services.AddScoped<IAutorService, AutorService>();

// Registramos el repositorio de libros como Singleton para mantener los datos en memoria
builder.Services.AddSingleton<IRepositorioLibro, RepositorioEnMemoria>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();