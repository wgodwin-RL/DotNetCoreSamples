using System.Collections.Generic;

namespace LD.Models
{
    public class Exam
    {
        public int Number { get; set; }

        public IEnumerable<StudentExamData> StudentData { get; set; }

        public decimal AverageScore { get; set; }
    }
}
