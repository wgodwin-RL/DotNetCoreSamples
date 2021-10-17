using System.Collections.Generic;

namespace LD.Models
{
    public class Student
    {
        public string StudentId { get; set; }
        public IEnumerable<StudentExamData> ExamData { get; set; }
        public decimal AverageScore { get; set; }
        public Student() 
        { 
        }

        public override string ToString()
        {
            return $"{this.StudentId}, {this.AverageScore}";
        }
    }
}
