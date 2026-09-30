using backend.Models;

namespace backend.Data;
//interface der hvilke metoder repositoryet skal have
public interface IProjectRepository
{
    Task<IEnumerable<Project>> GetAllAsync();
    Task<Project?> GetByIdAsync(int id);
    Task<int> CreateAsync(Project project);
    Task<bool> UpdateAsync(Project project);
    Task<bool> DeleteAsync(int id);
}
