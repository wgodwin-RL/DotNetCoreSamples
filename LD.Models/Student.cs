using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LD_Models
{
    public class Student
    {
        public string StudentId { get; set; }
        public IEnumerable<StudentExamData> ExamData { get; set; }
        public decimal AverageScore { get; set; }
        public Student() 
        { 
        }
    }
}
