using LD.Models;
using LD.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LD.Data
{
    public class StudentData : IStudentData
    {
        StudentExamMessageDatabaseContext _dBContext;
        public StudentData(StudentExamMessageDatabaseContext dBContext) 
        {
            _dBContext = dBContext;
        }

        public async Task<List<Student>> GetStudents()
        {

            var data = _dBContext.StudentExamData.ToList();
            return await ConvertEventDataMessagesToStudents(data);
        }

        public async Task<Student> GetStudent(string studentId)
        {
            var data = _dBContext.StudentExamData
                .Where(x => x.StudentId == studentId)
                .ToList();
         

            return await ConvertEventDataMessagesToStudent(studentId, data);
        }

        private async Task<List<Student>> ConvertEventDataMessagesToStudents(List<StudentExamData> eventMsgData) 
        {
            List<Student> students = new List<Student>();
            var studentIds = eventMsgData.Select(x => x.StudentId).Distinct();
            
            foreach (var studentId in studentIds)
            {
                var studentExamData = eventMsgData
                    .Where(x => x.StudentId == studentId)
                    .ToList();
                    
                students.Add(await ConvertEventDataMessagesToStudent(studentId, studentExamData));
            }

            return students;
        }

        private async Task<Student> ConvertEventDataMessagesToStudent(string studentId, List<StudentExamData> studentExamData)
        {
            return new Student()
            {
                StudentId = studentId,
                ExamData = studentExamData,
                AverageScore = Math.Round(studentExamData.Average(x=> x.Score), 2)
            };
        }
    }
}
