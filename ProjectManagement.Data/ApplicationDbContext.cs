using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Data.Models;

namespace ProjectManagement.Data
{
    public class ApplicationDbContext: IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectEmployee> ProjectEmployees { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Priority> Priorities { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region ProjectEmployeee
            builder.Entity<ProjectEmployee>()
                .HasOne(pe => pe.User)
                .WithMany(u => u.ProjectEmployees)
                .HasForeignKey(pe => pe.UserId);

            builder.Entity<ProjectEmployee>()
                .HasOne(pe => pe.Project)
                .WithMany(p => p.ProjectEmployees)
                .HasForeignKey(pe => pe.ProjectId);
            #endregion

            #region Tasks
            builder.Entity<Tasks>()
                .HasOne(t => t.Project)
                .WithMany()
                .HasForeignKey(t => t.ProjectId);

            builder.Entity<Tasks>()
                .HasOne(t => t.Executor)
                .WithMany()
                .HasForeignKey(t => t.ExecutorId)
            .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Tasks>()
                .HasOne(t => t.Status)
                .WithMany()
                .HasForeignKey(t => t.StatusId)
            .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Tasks>()
                .HasOne(t => t.Priority)
                .WithMany()
                .HasForeignKey(t => t.PriorityId)
            .OnDelete(DeleteBehavior.NoAction);
            #endregion
        }
    }
}
