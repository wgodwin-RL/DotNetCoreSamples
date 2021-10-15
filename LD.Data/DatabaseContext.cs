using LD_Models.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace LD_Data
{
    public class DatabaseContext : DbContext
    {
        private readonly ILogger _logger;
        public DatabaseContext(DbContextOptions<DatabaseContext> options, ILogger<DatabaseContext> logger) : base(options)
        {
            _logger = logger;
        }

        //public DbSet<LD_Models.Exam> Exams { get; set; }
        //public DbSet<LD_Models.Student> Students { get; set; }
        public DbSet<LD_Models.StudentExamData> StudentExamData { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            try
            {
                base.OnModelCreating(builder);
                builder.Entity<LD_Models.StudentExamData>()
                    .ToTable(Constants.StudentExamDataTableName)
                    .HasKey(x => new { x.Exam, x.StudentId });
            }
            catch (Exception e) 
            {
                _logger.LogError(e.Message);
            }
        }
    }
}
