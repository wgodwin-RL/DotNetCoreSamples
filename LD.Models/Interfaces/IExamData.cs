using System.Collections.Generic;
using System.Threading.Tasks;

namespace LD.Models.Interfaces
{
    public interface IExamData
    {
        Task<List<Exam>> GetExams();

        Task<Exam> GetExam(int id);
    }
}
