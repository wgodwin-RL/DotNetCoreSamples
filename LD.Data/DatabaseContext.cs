using LD.Models.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace LD.Data
{
    public class StudentExamMessageDatabaseContext : DbContext
    {
        private readonly ILogger _logger;
        public StudentExamMessageDatabaseContext(DbContextOptions<StudentExamMessageDatabaseContext> options, ILogger<StudentExamMessageDatabaseContext> logger) : base(options)
        {
            _logger = logger;
        }

        //public DbSet<LD.Models.Exam> Exams { get; set; }
        //public DbSet<LD.Models.Student> Students { get; set; }
        public DbSet<LD.Models.StudentExamData> StudentExamData { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            try
            {
                base.OnModelCreating(builder);
                builder.Entity<LD.Models.StudentExamData>()
                    .ToTable(TableNameConstants.StudentExamDataTableName)
                    .HasKey(x => new { x.Exam, x.StudentId });
            }
            catch (Exception e) 
            {
                _logger.LogError(e.Message);
            }
        }
    }
}
