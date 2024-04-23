﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProjectManagementSystem.Models
{
    public class ApplicationDbContext: IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRoles(builder);
        }

        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData
                (
                    new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin"},
                    new IdentityRole() { Name = "Manager", ConcurrencyStamp = "2", NormalizedName = "Manager" },
                    new IdentityRole() { Name = "Employee", ConcurrencyStamp = "3", NormalizedName = "Employee" }

                );
        }
    }
}
