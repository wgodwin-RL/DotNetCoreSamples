using LD_BootstrapSetup;
using LD_Models;
using LD_Models.Interfaces;
using LDAPI.Controllers;
using LDAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LD_APIIntegrationTests2
{
    public class ExamControllerTests
    {
        public const string _studentId1Lookup = "Willtest1";
        public const string _studentId2Lookup = "Willtest2";
        public const int _exam1Lookup = 1;
        public const int _exam2Lookup = 2;
        public Mock<IExamData> mock = new Mock<IExamData>();
        public readonly IServiceProvider _serviceProvider;
        private ILogger<ExamControllerTests> _logger;
        private ILogger<ExamsController> _controllerLogger;

        public ExamControllerTests()
        {
            var services = new ServiceCollection();

            DependencyInjection.InitServiceCollection(services);
            _serviceProvider = services.BuildServiceProvider();
            _logger = _serviceProvider.GetService<ILogger<ExamControllerTests>>();
            _controllerLogger = _serviceProvider.GetService<ILogger<ExamsController>>();
        }

        [Fact]
        public async void GetAllStudents_Test()
        {
            

            var resultingCheck = new List<ExamApiModel>() {
                new ExamApiModel()
                {
                    CumulativeAverageScore = new List<decimal>() { .80M, .90M }.Average()
                    ,
                    StudentExamResults = new List<ExamStudentsResultsApiModel>
                    {
                        new ExamStudentsResultsApiModel {StudentId = _studentId1Lookup, Score = .80M }
                        ,new ExamStudentsResultsApiModel {StudentId= _studentId2Lookup, Score = .90M }
                    }.ToArray()
                    ,
                    Exam = _exam1Lookup
                }
                ,new ExamApiModel()
                {
                    CumulativeAverageScore = new List<decimal>() { .80M, .75M }.Average()
                    ,
                    StudentExamResults = new List<ExamStudentsResultsApiModel>
                    {
                        new ExamStudentsResultsApiModel {StudentId = _studentId1Lookup, Score = .75M }
                        ,new ExamStudentsResultsApiModel {StudentId= _studentId2Lookup, Score = .80M }
                    }.ToArray()
                    ,
                    Exam = _exam2Lookup
                }
            };

            var examDataMock = new List<Exam> {
                new Exam()
                {
                    AverageScore = new List<decimal>() { .80M, .90M }.Average()
                    ,
                    StudentData = new List<StudentExamData>()
                    {
                        new StudentExamData { Exam = _exam1Lookup, Score = .80M, StudentId = _studentId1Lookup }
                        , new StudentExamData { Exam = _exam1Lookup, Score = .90M, StudentId = _studentId2Lookup }
                    }
                    ,
                    Number = _exam1Lookup
                }
                ,new Exam()
                {
                    AverageScore = new List<decimal>() { .80M, .75M }.Average()
                    ,
                    StudentData = new List<StudentExamData>()
                    {
                        new StudentExamData { Exam = _exam2Lookup, Score = .75M, StudentId = _studentId1Lookup }
                        , new StudentExamData { Exam = _exam2Lookup, Score = .80M, StudentId = _studentId2Lookup }
                    }
                    ,
                    Number = _exam2Lookup
                }
            };    

            mock.Setup(p => p.GetExams())
                .ReturnsAsync(examDataMock);


            var exams = new ExamsController(mock.Object, _controllerLogger);
            var apiResults = (ExamApiModel[])(((await exams.Exams()) as OkObjectResult).Value);

            Assert.NotNull(apiResults);
            var isEqual = apiResults.Equals(resultingCheck);   
            Assert.True(await ExamsResultsAreEqual(resultingCheck.ToArray(), apiResults.ToArray()));
        }

        [Fact]
        public async void GetExam1_Test()
        {


            var resultingCheck =
                new ExamApiModel()
                {
                    CumulativeAverageScore = new List<decimal>() { .80M, .90M }.Average()
                    ,
                    StudentExamResults = new List<ExamStudentsResultsApiModel>
                    {
                        new ExamStudentsResultsApiModel {StudentId = _studentId1Lookup, Score = .80M }
                        ,new ExamStudentsResultsApiModel {StudentId= _studentId2Lookup, Score = .90M }
                    }.ToArray()
                    ,
                    Exam = _exam1Lookup
                };

            var examDataMock =
                new Exam()
                {
                    AverageScore = new List<decimal>() { .80M, .90M }.Average()
                    ,
                    StudentData = new List<StudentExamData>()
                    {
                        new StudentExamData { Exam = _exam1Lookup, Score = .80M, StudentId = _studentId1Lookup }
                        , new StudentExamData { Exam = _exam1Lookup, Score = .90M, StudentId = _studentId2Lookup }
                    }
                    ,
                    Number = _exam1Lookup
                };

            mock.Setup(p => p.GetExam(_exam1Lookup)).ReturnsAsync(examDataMock);


            var exams = new ExamsController(mock.Object, _controllerLogger);
            var apiResults = (ExamApiModel)(((await exams.Exams(_exam1Lookup)) as OkObjectResult).Value);

            Assert.NotNull(apiResults);
            var isEqual = apiResults.Equals(resultingCheck);
            Assert.True(await ExamsResultsAreEqual(new ExamApiModel[1] { resultingCheck }, new ExamApiModel[1] { apiResults }));
        }

        private async Task<bool> ExamsResultsAreEqual(ExamApiModel[] resultingCheck, ExamApiModel[] apiResults)
        {
            var c = new ExamStudentsResultsApiModelEqualityComparer();
            
            var areExamResultsDifferent = apiResults
                .SelectMany(x => x.StudentExamResults)
                .Except(resultingCheck.SelectMany(x => x.StudentExamResults), c)
                .Any();

             var areDifferentStudent = apiResults
                .Select(x => x.Exam)
                .Except(resultingCheck.Select(x => x.Exam))
                .Any();

            var areDifferentScores = apiResults
                .Select(x => x.CumulativeAverageScore)
                .Except(resultingCheck.Select(x => x.CumulativeAverageScore))
                .Any();
            return !areExamResultsDifferent && !areDifferentStudent && !areDifferentScores;
        }
    }
}
