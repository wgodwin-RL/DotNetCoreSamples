using LD.BootstrapSetup;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LD.Models;
using LD.Models.Messages;
using LD.Models.Interfaces;

namespace LD.Data.Tests
{
    [TestClass()]
    public class ExamDataTests
    {
        private static TestContext _context = null;
        private static IExamData _examData = null;
        private static IEventMessageData _eventMsgData;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _context = context;
            Console.WriteLine("ClassInitialize");

            var services = new ServiceCollection();
            DependencyInjection.InitServiceCollection(services);
            var serviceProvider = services.BuildServiceProvider();

            _examData = serviceProvider.GetService<IExamData>();
            _eventMsgData = serviceProvider.GetService<IEventMessageData>();
            for (decimal x = 1; x < 10; x++)
            {
                var msg = new StudentExamEventMessage { Event="score", Data = new StudentExamData() { Exam = (int)x, Score = x, StudentId = x.ToString() } };
                _eventMsgData.UpsertEventMessage(msg).GetAwaiter().GetResult();
            }
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Console.WriteLine("ClassCleanup");
        }

        [TestMethod()]
        public async Task GetExamsTest()
        {
            var exams = await _examData.GetExams();
            await WriteMessage(JsonConvert.SerializeObject(exams));
            Assert.IsTrue(exams.Any());
        }

        [TestMethod()]
        public async Task GetExamTest()
        {
            int examId = 3;
            var exam = await _examData.GetExam(examId);
            await WriteMessage(JsonConvert.SerializeObject(exam));
            Assert.IsTrue(exam != null);
        }


        private async Task WriteMessage(string obj) 
        {
            Debug.WriteLine(obj);
        }
    }
}