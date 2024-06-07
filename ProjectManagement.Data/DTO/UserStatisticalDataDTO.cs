using ProjectManagement.Data.Models;


namespace ProjectManagement.Data.DTO
{
    public class UserStatisticalDataDTO
    {
        public int CountOfTasks { get; set; }
        public int CountOfCompletedTasks { get; set; }
        public int CountOfProjects { get; set; }
        public int CountOfFinishedProjects { get; set; }

        public List<TaskMonth> CountOfCompletedTaskInMonth { get; set; }
        public List<TaskData>? LastestTasks { get; set; }
    }

    public class TaskMonth
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int CompletedTasks { get; set; }
    }

    public class TaskData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PriorityName { get; set; }
        public string StatusName { get; set; }
        public DateTime Deadline { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
}
