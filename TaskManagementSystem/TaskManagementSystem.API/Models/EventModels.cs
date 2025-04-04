namespace TaskManagementSystem.API.Models
{
    public class TaskCompletedEvent
    {
        public int TaskId { get; set; }
        public string EventType { get; set; } = "TaskCompleted";
        public DateTime Timestamp { get; set; }
    }
}