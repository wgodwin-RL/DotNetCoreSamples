using LD.Models.Interfaces;
using LD.Models.Messages;
using LDAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace LDAPI.Controllers
{
    [Route("api/Students")]
    [ApiController]
    public class StudentsController : Controller
    {
        public readonly IStudentData _studentData;

        public readonly ILogger<StudentsController> _logger;
        public StudentsController(IStudentData studentData, ILogger<StudentsController> logger) 
        {
            _studentData = studentData;
            _logger = logger;
        }

        
        [HttpGet("")]
        public async Task<StudentApiModel[]> Students()
        {
            try
            {
                var results = await _studentData.GetStudents();
                if (!results.Any())
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                var response = results.Select(result => new StudentApiModel()
                {
                    StudentId = result.StudentId
                     ,
                    CumulativeAverageScore = result.AverageScore
                     ,
                    ExamResults = result.Exams
                         .Select(x => new StudentExamResultsApiModel() { Exam = x.ExamId, Score = x.Score })
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

        [HttpGet("{id}")]
        public async Task<StudentApiModel> Students(int id)
        {
            try
            {
                var student = await _studentData.GetStudent(id);
                if (student == null) 
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                var response = new StudentApiModel()
                {
                    StudentId = id
                    ,
                    CumulativeAverageScore = student.AverageScore
                    ,
                    ExamResults = student.Exams
                        .Select(x => new StudentExamResultsApiModel() { Exam = x.ExamId, Score = x.Score })
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
