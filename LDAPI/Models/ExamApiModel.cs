using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace LDAPI.Models
{
    public class ExamApiModel : IEquatable<ExamApiModel>
    {
        public int Exam { get; set; }
        public decimal CumulativeAverageScore { get; set; }

        public ExamStudentsResultsApiModel[] StudentExamResults { get; set; }

        public bool Equals([AllowNull] ExamApiModel other)
        {
            if (other == null)
                return false;

            return this.Exam.Equals(other.Exam) && this.CumulativeAverageScore.Equals(other.CumulativeAverageScore) && this.StudentExamResults.Equals(other.StudentExamResults);
        }

        public override int GetHashCode() => HashCode.Combine(Exam, CumulativeAverageScore, StudentExamResults);
    }

    public class ExamStudentsResultsApiModel : IEquatable<ExamStudentsResultsApiModel>
    {
        public string StudentId { get; set; }
        public decimal Score { get; set; }

        public bool Equals([AllowNull] ExamStudentsResultsApiModel other)
        {
            if (other == null)
                return false;

            return this.StudentId.Equals(other.StudentId) && this.Score.Equals(other.Score);
        }

        public override int GetHashCode() => HashCode.Combine(StudentId, Score);
    }




    public class ExamApiModelEqualityComparer : IEqualityComparer<ExamApiModel>
    {
        public bool Equals([AllowNull] ExamApiModel x, [AllowNull] ExamApiModel y)
        {
            ExamStudentsResultsApiModelEqualityComparer examStudentComparer = new ExamStudentsResultsApiModelEqualityComparer();


            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;
            else if (x.CumulativeAverageScore == y.CumulativeAverageScore
                && x.Exam == y.Exam)
                if (!x.StudentExamResults
                    .Except(y.StudentExamResults, examStudentComparer)
                    .Any())
                    return true;
                else
                    return false;
            else
                return false;
        }

        public int GetHashCode([DisallowNull] ExamApiModel obj)
        {
            int hCode = obj.Exam.GetHashCode() ^ obj.CumulativeAverageScore.GetHashCode() ^ obj.StudentExamResults.GetHashCode();
            return hCode.GetHashCode();
        }
    }

    public class ExamStudentsResultsApiModelEqualityComparer : IEqualityComparer<ExamStudentsResultsApiModel>
    {
        public bool Equals([AllowNull] ExamStudentsResultsApiModel x, [AllowNull] ExamStudentsResultsApiModel y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;
            else if (x.StudentId == y.StudentId && x.Score == y.Score)
                return true;
            else
                return false;
        }

        public int GetHashCode([DisallowNull] ExamApiModel obj)
        {
            int hCode = obj.Exam.GetHashCode() ^ obj.CumulativeAverageScore.GetHashCode() ^ obj.StudentExamResults.GetHashCode();
            return hCode.GetHashCode();
        }

        public int GetHashCode([DisallowNull] ExamStudentsResultsApiModel obj)
        {
            return obj.StudentId.GetHashCode() ^ obj.Score.GetHashCode();
        }
    }
}
