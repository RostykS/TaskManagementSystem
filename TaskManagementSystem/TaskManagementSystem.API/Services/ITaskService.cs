using TaskManagementSystem.API.Models;

namespace TaskManagementSystem.API.Services
{
    public interface ITaskService
    {
        Task<TaskDto> AddTaskAsync(TaskDto taskDto);
        Task<TaskDto> UpdateTaskStatusAsync(int id, string newStatus);
        Task<IEnumerable<TaskDto>> GetTasksAsync();
        Task<TaskDto> GetTaskByIdAsync(int id);
    }
}