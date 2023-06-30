using System;
using Microsoft.EntityFrameworkCore;
using ProjectManagementAPI.Models;

namespace ProjectManagementAPI
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)

        {
        }

        public DbSet<ProjectItem> Projects { get; set; }

        public DbSet<EpicItem> Epics { get; set; }

        public DbSet<SubTaskItem> SubTasks { get; set; }

    }
}

