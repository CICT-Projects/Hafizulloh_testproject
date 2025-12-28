using Microsoft.EntityFrameworkCore;
using System.Linq;

public class PartRepository : IPartRepository
{
    private readonly AppDbContext _context;

    public PartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Part>> GetAllPartsAsync()
    {
        return await _context.Parts.ToListAsync();
    }

    public async Task<Part?> GetPartByIdAsync(int id)
    {
        return await _context.Parts.FindAsync(id);
    }

    public async Task AddPartAsync(Part part)
    {
        _context.Parts.Add(part);
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePartAsync(Part part)
    {
        _context.Parts.Update(part);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePartAsync(int id)
    {
        var part = await _context.Parts.FindAsync(id);
        if (part != null)
        {
            _context.Parts.Remove(part);
            await _context.SaveChangesAsync();
        }
    }
}