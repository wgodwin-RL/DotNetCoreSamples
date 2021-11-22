using LD.BootstrapSetup;
using LD.Models;
using LD.Models.Interfaces;
using LD.Models.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LD.Data.Tests.Integration
{
    public class ExamDataTests
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly ILogger<ExamDataTests> _logger;
        private readonly IExamData _examData;
        private readonly IEventMessageData _eventMsgData;

        public ExamDataTests()
        {
            var services = new ServiceCollection();

            DependencyInjection.InitServiceCollection(services);
            _serviceProvider = services.BuildServiceProvider();
            _logger = _serviceProvider.GetService<ILogger<ExamDataTests>>();
            _examData = _serviceProvider.GetService<IExamData>();
            _eventMsgData = _serviceProvider.GetService<IEventMessageData>();
            for (decimal x = 1; x < 10; x++)
            {
                var msg = new StudentExamEventMessage { Event = "score", Data = new StudentExamData() { ExamId = (int)x, Score = x, StudentId = (int)x } };
                _eventMsgData.UpsertEventMessage(msg).GetAwaiter().GetResult();
            }
        }

        [Fact]
        public async Task GetExams_Test() 
        {

            var exams = await _examData.GetExams();
            await WriteMessage(JsonConvert.SerializeObject(exams));
            Assert.True(exams.Any());
        }

        [Fact]
        public async Task GetExam_Test()
        {

            var exam = await _examData.GetExam(1);
            await WriteMessage(JsonConvert.SerializeObject(exam));
            Assert.True(exam != null);
        }

        private async Task WriteMessage(string obj)
        {
            _logger.LogInformation(obj);
        }
    }
}
