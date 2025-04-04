﻿namespace TaskManagementSystem.API.Models
{
	public class TaskEntity
	{
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskStatus Status { get; set; } = TaskStatus.NotStarted;
        public string? AssignedTo { get; set; }
    }
}