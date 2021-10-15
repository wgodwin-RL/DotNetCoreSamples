using System;
using System.Collections.Generic;
using System.Text;

namespace LD_Models
{
    public class Exam
    {
        public int Number { get; set; }

        public IEnumerable<StudentExamData> StudentData { get; set; }

        public decimal AverageScore { get; set; }
    }
}
