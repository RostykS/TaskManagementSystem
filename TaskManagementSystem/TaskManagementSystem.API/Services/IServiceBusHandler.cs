using TaskManagementSystem.API.Models;

namespace TaskManagementSystem.API.Services
{
    public interface IServiceBusHandler
    {
        Task SendTaskCompletedEventAsync(TaskCompletedEvent completedEvent);
        Task ReceiveMessageAsync();
    }
}