using LD_Models.Interfaces;
using LD_Models.Messages;
using LDAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<ActionResult> Students()
        {
            try
            {
                var results = await _studentData.GetStudents();
                if (!results.Any())
                {
                    return NotFound();
                }

                var response = results.Select(result => new StudentApiModel()
                {
                    StudentId = result.StudentId
                     ,
                    CumulativeAverageScore = result.AverageScore
                     ,
                    ExamResults = result.ExamData
                         .Select(x => new StudentExamResultsApiModel() { Exam = x.Exam, Score = x.Score })
                         .ToArray()
                }).ToArray();

                return Ok(response);
            }
            catch (Exception e) 
            {
                return BadRequest(e);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Students(string id)
        {
            try
            {
                var result = await _studentData.GetStudent(id);

                if (result == null) 
                {
                    return NotFound(id);
                }

                var response = new StudentApiModel()
                {
                    StudentId = id
                    ,
                    CumulativeAverageScore = result.AverageScore
                    ,
                    ExamResults = result.ExamData
                        .Select(x => new StudentExamResultsApiModel() { Exam = x.Exam, Score = x.Score })
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
