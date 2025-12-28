using System.Collections.Generic;
using System.Threading.Tasks;

public interface IPartRepository
{
    Task<List<Part>> GetAllPartsAsync();
    Task<Part?> GetPartByIdAsync(int id);
    Task AddPartAsync(Part part);
    Task UpdatePartAsync(Part part);
    Task DeletePartAsync(int id);
}