using LD_Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LD_Models.Interfaces
{
    public interface IExamData
    {
        Task<List<Exam>> GetExams();

        Task<Exam> GetExam(int id);
    }
}
