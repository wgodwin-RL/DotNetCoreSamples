using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using LDAPI.Models;
using System;
using LD.Models.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Web.Http;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace LDAPI.Controllers
{
    [Route("api/Exams")]
    [ApiController]
    public class ExamsController : Controller
    {
        private readonly IExamData _examData;
        private readonly ILogger<ExamsController> _logger;
        public ExamsController(IExamData examData, ILogger<ExamsController> logger) 
        {
            _examData = examData;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<ExamApiModel[]> Exams()
        {
            try
            {
                var results = await _examData.GetExams();
                if (!results.Any())
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                var response = results.Select(x => new ExamApiModel()
                {
                    Exam = x.ExamId
                    ,
                    CumulativeAverageScore = x.AverageScore
                    ,
                    StudentExamResults = x.Students
                        .Select(x => new ExamStudentsResultsApiModel() { Score = x.Score, StudentId = x.StudentId })
                        .ToArray()
                }).ToArray();

                return response;
            }
            catch (HttpResponseException hrex) 
            {
                _logger.LogError(hrex.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{number}")]
        public async Task<ExamApiModel> Exams(int number)
        {
            try
            {
                var result = await _examData.GetExam(number);

                var response = new ExamApiModel()
                {
                    Exam = result.ExamId,
                    CumulativeAverageScore = result.AverageScore,
                    StudentExamResults = result.Students
                        .Select(x => new ExamStudentsResultsApiModel() { Score = x.Score, StudentId = x.StudentId })
                        .ToArray()
                };

                return response;
            }
            catch (HttpResponseException hrex)
            {
                _logger.LogError(hrex.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }
    }
}
