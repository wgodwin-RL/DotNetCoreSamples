using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

using System.Threading.Tasks;
using System.Linq;
using LD.BootstrapSetup;
using System.Diagnostics;
using Newtonsoft.Json;
using LD.Models;
using LD.Models.Messages;
using LD.Models.Interfaces;

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
            await WriteMessage(JsonConvert.SerializeObject(students));
            Assert.IsTrue(students.Any());
        }

        [TestMethod()]
        public async Task GetStudentTest()
        {
            var studentId = "2";
            var student = await _studentData.GetStudent(studentId);
            await WriteMessage(JsonConvert.SerializeObject(student));
            Assert.IsTrue(student != null);
        }

        private async Task WriteMessage(string obj)
        {
            Debug.WriteLine(obj);
        }
    }
}