

namespace ProjectManagement.Data.DTO
{
    public class TasksBoardDTO
    {
        public ICollection<TasksDTO>? ToDo { get; set; }
        public ICollection<TasksDTO>? InProgress { get; set; }
        public ICollection<TasksDTO>? Done { get; set; }
        public int? TasksCount {get; set; }
        public int? CompletedTasksCount { get; set; }
    }
}
