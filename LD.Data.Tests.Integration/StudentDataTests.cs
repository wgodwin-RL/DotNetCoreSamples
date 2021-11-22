using LD.BootstrapSetup;
using LD.Models;
using LD.Models.Interfaces;
using LD.Models.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LD.Data.Tests.Integration
{
    public class StudentDataTests
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly ILogger<StudentDataTests> _logger;
        private readonly IStudentData _studentData;
        private readonly IEventMessageData _eventMsgData;

        public StudentDataTests() 
        {
            var services = new ServiceCollection();

            DependencyInjection.InitServiceCollection(services);
            _serviceProvider = services.BuildServiceProvider();
            _logger = _serviceProvider.GetService<ILogger<StudentDataTests>>();
            _studentData = _serviceProvider.GetService<IStudentData>();
            _eventMsgData = _serviceProvider.GetService<IEventMessageData>();
            for (decimal x = 1; x < 10; x++)
            {
                var msg = new StudentExamEventMessage { Event = "score", Data = new StudentExamData() { ExamId = (int)x, Score = x, StudentId = (int)x } };
                _eventMsgData.UpsertEventMessage(msg).GetAwaiter().GetResult();
            }
        }

        [Fact]
        public async Task GetStudentsTest()
        {
            var students = await _studentData.GetStudents();
            
            await WriteMessage(JsonConvert.SerializeObject(students));
            Assert.True(students.Any());
        }

        [Fact]
        public async Task GetStudentTest()
        {
            var studentId = 2;
            var student = await _studentData.GetStudent(studentId);
            await WriteMessage(JsonConvert.SerializeObject(student));
            Assert.True(student != null);
        }

        private async Task WriteMessage(string obj)
        {
            _logger.LogInformation(obj);
        }
    }
}
