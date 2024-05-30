
namespace ProjectManagement.Data.Models
{
    public class Tasks
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PriorityId { get; set; }
        public int StatusId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime Deadline { get; set; }
        public string? ExecutorId { get; set; }
        public int ProjectId { get; set; }
        public string? Description { get; set; }

        public Priority Priority { get; set; }
        public Status Status { get; set; }
        public User? Executor { get; set; }
        public Project Project { get; set; }
    }
}
