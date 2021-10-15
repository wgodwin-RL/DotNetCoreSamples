using LD_Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LD_Models.Interfaces
{
    public interface IStudentData
    {
        Task<List<Student>> GetStudents();
        Task<Student> GetStudent(string studentId);
    }
}
