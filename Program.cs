var builder = WebApplication.CreateBuilder(args);
// inyeccion de dependencias:
builder.Services.AddSingleton<IClienteRepository, ClientesRepository>(); 
builder.Services.AddSingleton<IProductoRepository, ProductoRepository>();
builder.Services.AddSingleton<IPresupuestosRepository, PresupuestosRepository>(); //singleton porque se usa por cada consulta http
builder.Services.AddScoped<IUserRepository, UserRepository>(); // scoped porque su uso se da hasta que la app finaliza
var CadenaDeConexion= builder.Configuration.GetConnectionString("SqliteConexion")!.ToString(); //definido en appsetting.json
builder.Services.AddSingleton(CadenaDeConexion);
//agregamos el uso de sesiones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiración de la sesión
    options.Cookie.HttpOnly = true; // Solo accesible desde HTTP, no JavaScript
    options.Cookie.IsEssential = true; // Necesario incluso si el usuario no acepta cookies
});
// Add services to the container.
builder.Services.AddControllersWithViews();
var app = builder.Build();
app.UseSession();
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
    pattern: "{controller=Login}/{action=Index}/{id?}")
    // pattern: "{controller=Productos}/{action=Index}/{id?}") esto hace que se inicie la pagina aqui
    .WithStaticAssets();


app.Run();
