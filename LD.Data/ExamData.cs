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
        private readonly DatabaseContext _dBContext;
        public ExamData(DatabaseContext dBContext)
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
                .Where(x => x.Exam == id)
                .ToList();
            
            return await ConvertEventDataMessagesToExam(id, data);
        }

        private async Task<List<Exam>> ConvertEventDataMessagesToExams(List<StudentExamData> eventMsgData)
        {
            List<Exam> exams = new List<Exam>();
            var examIds = eventMsgData.Select(x => x.Exam).Distinct();

            foreach (var examId in examIds)
            {
                var examMsgs = eventMsgData
                    .Where(x=> x.Exam == examId)
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
                Number = examId
                , StudentData = examMsgs
                , AverageScore = Math.Round(examMsgs.Average(x => x.Score), 2)
            };
        }
    }
}
