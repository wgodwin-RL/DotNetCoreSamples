using System.Collections.Generic;

namespace LD.Models
{
    public class Exam
    {
        public int ExamId { get; set; }

        public IEnumerable<StudentExamData> Students { get; set; }

        public decimal AverageScore { get; set; }


        public override string ToString()
        {
            return $"{this.ExamId}, {this.AverageScore}";
        }
    }
}
