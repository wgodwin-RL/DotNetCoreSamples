using System.Collections.Generic;
using System.Threading.Tasks;

namespace LD.Models.Interfaces
{
    public interface IStudentData
    {
        Task<List<Student>> GetStudents();
        Task<Student> GetStudent(string studentId);
    }
}
