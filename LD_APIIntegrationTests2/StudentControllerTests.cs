using LD.Models;
using LD.Models.Interfaces;
using LD.BootstrapSetup;
using LDAPI.Controllers;
using LDAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LD_APIIntegrationTests2
{
    public class StudentControllerTests
    {
        private const string _studentIdLookup1 = "willtest1";
        private const string _studentIdLookup2 = "willtest2";
        private const int _examLookup1 = 1;
        private const int _examLookup2 = 2;

        public Mock<IStudentData> mock = new Mock<IStudentData>();
        public readonly IServiceProvider _serviceProvider;
        private ILogger<StudentControllerTests> _logger;
        private ILogger<StudentsController> _controllerLogger;


        public StudentControllerTests() 
        {

            var services = new ServiceCollection();

            DependencyInjection.InitServiceCollection(services);
            _serviceProvider = services.BuildServiceProvider();
            _logger = _serviceProvider.GetService<ILogger<StudentControllerTests>>();
        }


        [Fact]
        public async void GetAllStudents_Test()
        {
            var resultingCheck = new StudentApiModel[2]
            {
                new StudentApiModel()
                {
                    CumulativeAverageScore = new List<decimal>(){ .80M, .90M }.Average()
                    , ExamResults = new StudentExamResultsApiModel[2]
                    {
                        new StudentExamResultsApiModel { Exam = _examLookup1, Score = .80M }
                        ,new StudentExamResultsApiModel { Exam = _examLookup2, Score = .90M }
                    }
                    , StudentId = _studentIdLookup1
                },
                new StudentApiModel()
                {
                    CumulativeAverageScore = new List<decimal>(){ .75M, .80M }.Average()
                    , ExamResults = new StudentExamResultsApiModel[2]
                    {
                        new StudentExamResultsApiModel { Exam = _examLookup1, Score = .75M }
                        ,new StudentExamResultsApiModel { Exam = _examLookup2, Score = .80M }
                    }
                    , StudentId = _studentIdLookup2
                }
            };

            var studentDataMock = new List<Student>()
            {
                new Student()
                {
                    AverageScore = new List<decimal>(){ .80M, .90M }.Average()
                    , ExamData = new List<StudentExamData>()
                    {
                        new StudentExamData { Exam = _examLookup1, Score = .80M, StudentId = _studentIdLookup1 }
                        , new StudentExamData { Exam = _examLookup2, Score = .90M, StudentId = _studentIdLookup1 }
                    }
                    , StudentId = _studentIdLookup1 }
                ,new Student()
                {
                    AverageScore = new List<decimal>(){ .80M, .75M }.Average()
                    , ExamData = new List<StudentExamData>()
                    {
                        new StudentExamData { Exam = _examLookup1, Score = .75M, StudentId = _studentIdLookup2 }
                        , new StudentExamData { Exam = _examLookup2, Score = .80M, StudentId = _studentIdLookup2 }
                    }
                    , StudentId = _studentIdLookup2 }
            };

            mock.Setup(p => p.GetStudents()).ReturnsAsync(studentDataMock);

            StudentsController students = new StudentsController(mock.Object, _controllerLogger);
            var apiResults = (StudentApiModel[])(((await students.Students()) as OkObjectResult).Value);

            Assert.NotNull(apiResults);
            Assert.True(await StudentResultsAreEqual(resultingCheck, apiResults));
        }

        [Fact]
        public async void GetStudent_Test()
        {
            var resultingCheck =
                new StudentApiModel()
                {
                    CumulativeAverageScore = new List<decimal>() { .80M, .90M }.Average()
                    ,
                    ExamResults = new List<StudentExamResultsApiModel>
                    {
                        new StudentExamResultsApiModel { Exam = _examLookup1, Score = .80M }
                        ,new StudentExamResultsApiModel { Exam = _examLookup2, Score = .90M }
                    }.ToArray()
                    ,
                    StudentId = _studentIdLookup1
                };

            var studentDataMock=
                new Student()
                {
                    AverageScore = new List<decimal>() { .80M, .90M }.Average()
                    , ExamData = new List<StudentExamData>()
                    {
                        new StudentExamData { Exam = _examLookup1, Score = .80M, StudentId = _studentIdLookup1 }
                        , new StudentExamData { Exam = _examLookup2, Score = .90M, StudentId = _studentIdLookup1 }
                    }
                    , StudentId = _studentIdLookup1
                };

            mock.Setup(p => p.GetStudent(_studentIdLookup1)).ReturnsAsync(studentDataMock);

            StudentsController students = new StudentsController(mock.Object, _controllerLogger);
            var apiResults = (StudentApiModel)(((await students.Students(_studentIdLookup1)) as OkObjectResult).Value);

            Assert.NotNull(apiResults);
            var isEqual = apiResults.Equals(resultingCheck);
            Assert.True(await StudentResultsAreEqual(new StudentApiModel[1] { resultingCheck }, new StudentApiModel[1] { apiResults }));
        }

        private async Task<bool> StudentResultsAreEqual(StudentApiModel[] resultingCheck, StudentApiModel[] apiResults)
        {
            StudentExamResultsApiModelEqualityComparer c = new StudentExamResultsApiModelEqualityComparer();
            var areExamResultsDifferent = apiResults.SelectMany(x => x.ExamResults).Except(resultingCheck.SelectMany(x => x.ExamResults), c).Any();

            var isEqual = apiResults.Equals(resultingCheck);

            var areDifferentStudent = apiResults.Select(x => x.StudentId)
                .Except(resultingCheck.Select(x => x.StudentId))
                .ToList().Any();

            var areDifferentScores = apiResults.Select(x => x.CumulativeAverageScore)
                .Except(resultingCheck.Select(x => x.CumulativeAverageScore))
                .ToList().Any();

            return !areExamResultsDifferent && !areDifferentStudent && !areDifferentScores;
        }
    }
}
