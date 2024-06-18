
namespace ProjectManagement.Data.DTO
{
    public class ReportDTO
    {
        public string ProjectName { get; set; }
        public bool ProjectStatus { get; set; }
        public string? DateTime { get; set; }
        public ICollection<Statistic> Statistics { get; set; }
    }

    public class Statistic
    {
        public string EmployeeName { get; set;}
        public int TasksCompleted { get; set; }
        public int TasksAssigned { get; set; }
    }
}
