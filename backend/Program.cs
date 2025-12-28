using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IPartRepository, PartRepository>();
builder.Services.AddControllersWithViews();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:5173")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.MapGet("/hello", () => Results.Content("<html><body><h1>Hello World!</h1></body></html>", "text/html"));

// CRUD for Cars via minimal API (keeping for now)
app.MapGet("/cars", async (AppDbContext db) => await db.Cars.ToListAsync());

app.MapGet("/cars/{id}", async (int id, AppDbContext db) =>
    await db.Cars.FindAsync(id) is Car car ? Results.Ok(car) : Results.NotFound());

app.MapPost("/cars", async (Car car, AppDbContext db) =>
{
    db.Cars.Add(car);
    await db.SaveChangesAsync();
    return Results.Created($"/cars/{car.Id}", car);
});

app.MapPut("/cars/{id}", async (int id, Car inputCar, AppDbContext db) =>
{
    var car = await db.Cars.FindAsync(id);
    if (car is null) return Results.NotFound();
    car.Make = inputCar.Make;
    car.Model = inputCar.Model;
    car.Year = inputCar.Year;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/cars/{id}", async (int id, AppDbContext db) =>
{
    var car = await db.Cars.FindAsync(id);
    if (car is null) return Results.NotFound();
    db.Cars.Remove(car);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.UseCors("AllowSpecificOrigin");

app.MapControllers();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
