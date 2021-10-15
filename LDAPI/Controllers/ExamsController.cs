using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using LDAPI.Models;
using System;
using LD_Models.Interfaces;
using Microsoft.Extensions.Logging;

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
        public async Task<ActionResult> Exams()
        {
            try
            {
                var results = await _examData.GetExams();
                if (!results.Any())
                {
                    return NotFound();
                }

                var response = results.Select(x => new ExamApiModel()
                {
                    Exam = x.Number
                    ,
                    CumulativeAverageScore = x.AverageScore
                    ,
                    StudentExamResults = x.StudentData
                        .Select(x => new ExamStudentsResultsApiModel() { Score = x.Score, StudentId = x.StudentId })
                        .ToArray()
                }).ToArray();

                return Ok(response);
            }
            catch (Exception e) 
            {
                return BadRequest(e);
            }
        }

        [HttpGet("{number}")]
        public async Task<ActionResult> Exams(int number)
        {
            try
            {
                var result = await _examData.GetExam(number);

                var response = new ExamApiModel()
                {
                    Exam = result.Number,
                    CumulativeAverageScore = result.AverageScore,
                    StudentExamResults = result.StudentData
                        .Select(x => new ExamStudentsResultsApiModel() { Score = x.Score, StudentId = x.StudentId })
                        .ToArray()
                };

                return Ok(response);
            }
            catch (Exception e) 
            {
                return BadRequest(e);
            }
        }
    }
}
