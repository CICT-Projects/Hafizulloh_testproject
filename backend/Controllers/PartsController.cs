using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PartsController : ControllerBase
{
    private readonly IPartRepository _partRepository;

    public PartsController(IPartRepository partRepository)
    {
        _partRepository = partRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetParts()
    {
        var parts = await _partRepository.GetAllPartsAsync();
        return Ok(parts);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePart(Part part)
    {
        await _partRepository.AddPartAsync(part);
        return CreatedAtAction(nameof(GetPart), new { id = part.Id }, part);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPart(int id)
    {
        var part = await _partRepository.GetPartByIdAsync(id);
        return part is not null ? Ok(part) : NotFound();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePart(int id, Part inputPart)
    {
        var part = await _partRepository.GetPartByIdAsync(id);
        if (part is null) return NotFound();

        part.Name = inputPart.Name;
        part.Category = inputPart.Category;
        part.Price = inputPart.Price;

        await _partRepository.UpdatePartAsync(part);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePart(int id)
    {
        var part = await _partRepository.GetPartByIdAsync(id);
        if (part is null) return NotFound();

        await _partRepository.DeletePartAsync(id);
        return NoContent();
    }
}