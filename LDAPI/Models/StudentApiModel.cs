using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace LDAPI.Models
{
    public class StudentApiModel //: IEquatable<StudentApiModel>
    {
        public string StudentId { get; set; }
        public decimal CumulativeAverageScore { get; set; }

        public StudentExamResultsApiModel[] ExamResults{ get; set; }
    }

    public class StudentApiModelEqualityComparer : IEqualityComparer<StudentApiModel>
    {
        public bool Equals([AllowNull] StudentApiModel x, [AllowNull] StudentApiModel y)
        {
            if (x == null && y == null)
                return true;

            else if (x == null || y == null)
                return false;
            else if (x.CumulativeAverageScore == y.CumulativeAverageScore
                && x.StudentId == y.StudentId)
            {
                var serComparer = new StudentExamResultsApiModelEqualityComparer();
                if (x.ExamResults.Except(y.ExamResults, serComparer).Any())
                    return false;
                else if (y.ExamResults.Except(x.ExamResults, serComparer).Any())
                    return false;

                return true;
            }
            else
                return false;
        }

        public int GetHashCode([DisallowNull] StudentApiModel obj)
        {
            return obj.StudentId.GetHashCode() ^ obj.CumulativeAverageScore.GetHashCode() ^ obj.ExamResults.GetHashCode();
        }
    }
}
