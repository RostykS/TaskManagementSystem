using TaskManagementSystem.API.Models;

namespace TaskManagementSystem.API.Repositories
{
    public interface ITaskRepository
    {
        Task<TaskEntity> AddAsync(TaskEntity task);
        Task<TaskEntity> UpdateAsync(TaskEntity task);
        Task<TaskEntity> GetByIdAsync(int id);
        Task<IEnumerable<TaskEntity>> GetAllAsync();
        Task DeleteAsync(int id);
    }
}