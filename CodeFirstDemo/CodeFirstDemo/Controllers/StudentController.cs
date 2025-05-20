using Azure.Core;
using CodeFirstDemo.Db.Models;
using CodeFirstDemo.DTOs;
using CodeFirstDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeFirstDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        // GET: api/Student
        [HttpGet]
        public async Task<ActionResult<List<Student>>> GetStudents()
        {
            return await _studentService.GetStudentsAsync();
        }

        // GET: api/Student/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            Student student = await _studentService.GetStudentAsync(id);
            if (student != null)
                return student;
            return NotFound(new { message = $"Student with ID {id} was not found." });
        }

        // POST: api/Student
        [HttpPost]
        public async Task<ActionResult> PostStudent(CreateStudentRequest request)
        {
            Student student = await _studentService.CreateStudentAsync(request);
            return CreatedAtAction(nameof(GetStudent), new { id = student.StudentId }, student);
        }

        // PUT: api/Student/5
        [HttpPut("{id}")]
        public async Task<ActionResult> PutStudent(int id, UpdateStudentRequest request)
        {
            bool result = await _studentService.UpdateStudentAsync(id, request);
            if (result)
                return Ok(new { message = $"Student with ID {id} has been successfully upated." }); ;
            return NotFound(new { message = $"Student with ID {id} was not found." }); ;
        }

        // DELETE: api/Student/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            bool result = await _studentService.DeleteStudentAsync(id);
            if (result)
                return Ok(new { message = $"Student with ID {id} has been successfully deleted." }); ;
            return NotFound(new { message = $"Student with ID {id} was not found." });
        }
    }
}
