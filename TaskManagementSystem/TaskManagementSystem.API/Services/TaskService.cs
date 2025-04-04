
using AutoMapper;
using TaskManagementSystem.API.Models;
using TaskManagementSystem.API.Repositories;
using TaskStatus = TaskManagementSystem.API.Models.TaskStatus;

namespace TaskManagementSystem.API.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IServiceBusHandler _serviceBusHandler;
        private readonly IMapper _mapper;


        public TaskService(ITaskRepository taskRepository, IServiceBusHandler serviceBusHandler, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _serviceBusHandler = serviceBusHandler;
            _mapper = mapper;;
        }

        public async Task<TaskDto> AddTaskAsync(TaskDto taskDto)
        {
            var taskEntity = _mapper.Map<TaskEntity>(taskDto);
            var addedTask = await _taskRepository.AddAsync(taskEntity);
            return _mapper.Map<TaskDto>(addedTask);
        }

        public async Task<TaskDto> UpdateTaskStatusAsync(int id, string newStatus)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) return null;

            task.Status = Enum.Parse<TaskStatus>(newStatus);
            var updatedTask = await _taskRepository.UpdateAsync(task);

            if (updatedTask.Status == TaskStatus.Completed)
            {
                var eventMessage = new TaskCompletedEvent
                {
                    TaskId = updatedTask.Id,
                    Timestamp = DateTime.UtcNow
                };

                await _serviceBusHandler.SendTaskCompletedEventAsync(eventMessage);
            }

            return _mapper.Map<TaskDto>(updatedTask);
        }

        public async Task<IEnumerable<TaskDto>> GetTasksAsync()
        {
            var tasks = await _taskRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TaskDto>>(tasks);
        }

        public async Task<TaskDto> GetTaskByIdAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            return task == null ? null : _mapper.Map<TaskDto>(task);
        }
    }
}