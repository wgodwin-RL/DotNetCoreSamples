using LD.Models;
using LD.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LD.Data
{
    public class ExamData : IExamData
    {
        private readonly StudentExamMessageDatabaseContext _dBContext;
        public ExamData(StudentExamMessageDatabaseContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task<List<Exam>> GetExams()
        {
            var data = _dBContext.StudentExamData
                .ToList();

            return await ConvertEventDataMessagesToExams(data);
        }

        public async Task<Exam> GetExam(int id)
        {
            var data = _dBContext.StudentExamData
                .Where(x => x.ExamId == id)
                .ToList();
            
            return await ConvertEventDataMessagesToExam(id, data);
        }

        private async Task<List<Exam>> ConvertEventDataMessagesToExams(List<StudentExamData> eventMsgData)
        {
            List<Exam> exams = new List<Exam>();
            var examIds = eventMsgData.Select(x => x.ExamId).Distinct();

            foreach (var examId in examIds)
            {
                var examMsgs = eventMsgData
                    .Where(x=> x.ExamId == examId)
                    .ToList();

                var exam = await ConvertEventDataMessagesToExam(examId, examMsgs);
                exams.Add(exam);
            }

            return exams;
        }

        private async Task<Exam> ConvertEventDataMessagesToExam(int examId, List<StudentExamData> examMsgs)
        {
            return new Exam()
            {
                ExamId = examId
                , Students = examMsgs
                , AverageScore = Math.Round(examMsgs.Average(x => x.Score), 2)
            };
        }
    }
}
