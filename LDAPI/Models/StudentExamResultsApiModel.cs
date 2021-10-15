using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LDAPI.Models
{
    public class StudentExamResultsApiModel //: IEquatable<StudentExamResultsApiModel>
    {
        public int Exam { get; set; }
        public decimal Score { get; set; }

        //public bool Equals([AllowNull] StudentExamResultsApiModel other)
        //{
        //    if (this.Exam == other.Exam && this.Score == other.Score)
        //        return true;
        //    else
        //        return false;
        //}
    }

    public class StudentExamResultsApiModelEqualityComparer : IEqualityComparer<StudentExamResultsApiModel>
    {
        public bool Equals([AllowNull] StudentExamResultsApiModel x, [AllowNull] StudentExamResultsApiModel y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;
            if (x.Exam == y.Exam && x.Score == y.Score)
                return true;


            return false;
        }

        public int GetHashCode([DisallowNull] StudentExamResultsApiModel obj)
        {
            return obj.Exam.GetHashCode() ^ obj.Score.GetHashCode();
        }
    }
}
