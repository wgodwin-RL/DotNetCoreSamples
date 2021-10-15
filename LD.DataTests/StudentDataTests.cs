using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

using System.Threading.Tasks;
using System.Linq;
using LD_BootstrapSetup;
using System.Diagnostics;
using Newtonsoft.Json;
using LD_Models;
using LD_Models.Messages;
using LD_Models.Interfaces;

namespace LD.Data.Tests
{
    [TestClass()]
    public class StudentDataTests
    {
        private static TestContext _context = null;
        private static IStudentData _studentData = null;
        private static IEventMessageData _eventMsgData;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _context = context;
            Console.WriteLine("ClassInitialize");

            var services = new ServiceCollection();
            //services.AddTransient<IStudentData, StudentData>();
            //services.AddTransient<IExamData, ExamData>();
            //services.AddTransient<IEventMessageData, EventMessageData>();

            DependencyInjection.InitServiceCollection(services);
            var serviceProvider = services.BuildServiceProvider();

            _studentData = serviceProvider.GetService<IStudentData>();
            _eventMsgData = serviceProvider.GetService<IEventMessageData>();
            for (decimal x = 1; x < 10; x++)
            {
                var msg = new StudentExamEventMessage { Event = "score", Data = new StudentExamData() { Exam = (int)x, Score = x, StudentId = x.ToString() } };
                _eventMsgData.UpsertEventMessage(msg).GetAwaiter().GetResult();
            }
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Console.WriteLine("ClassCleanup");
        }

        [TestMethod()]
        public async Task GetStudentsTest()
        {
            var students = await _studentData.GetStudents();
            JsonConvert.SerializeObject(students);
            Assert.IsTrue(students.Any());
        }

        [TestMethod()]
        public async Task GetStudentTest()
        {
            var studentId = "2";
            var student = await _studentData.GetStudent(studentId);
            JsonConvert.SerializeObject(student);
            Assert.IsTrue(student != null);
        }

        private async Task WriteMessage(string obj)
        {
            Debug.WriteLine(obj);
        }
    }
}