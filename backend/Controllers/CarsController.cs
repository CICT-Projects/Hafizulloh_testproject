using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    private readonly AppDbContext _db;

    public CarsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetCars() => Ok(await _db.Cars.ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCar(int id)
    {
        var car = await _db.Cars.FindAsync(id);
        return car is not null ? Ok(car) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateCar(Car car)
    {
        _db.Cars.Add(car);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCar), new { id = car.Id }, car);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCar(int id, Car inputCar)
    {
        var car = await _db.Cars.FindAsync(id);
        if (car is null) return NotFound();
        car.Make = inputCar.Make;
        car.Model = inputCar.Model;
        car.Year = inputCar.Year;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCar(int id)
    {
        var car = await _db.Cars.FindAsync(id);
        if (car is null) return NotFound();
        _db.Cars.Remove(car);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}