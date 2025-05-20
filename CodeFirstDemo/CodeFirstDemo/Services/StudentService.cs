using Azure.Core;
using CodeFirstDemo.Db;
using CodeFirstDemo.Db.Models;
using CodeFirstDemo.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CodeFirstDemo.Services
{
    public class StudentService : IStudentService
    {
        private readonly CodeFirstDemoContext _context;
        public StudentService(CodeFirstDemoContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> GetStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            return student;
        }

        public async Task<Student> CreateStudentAsync(CreateStudentRequest request)
        {
            Student student = new Student
            {
                Name = request.Name
            };
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<bool> UpdateStudentAsync(int id, UpdateStudentRequest request)
        {
            var existingStudent = await _context.Students.FindAsync(id);
            if (existingStudent == null)
                return false;
            existingStudent.Name = request.Name;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var existingStudent = await _context.Students.FindAsync(id);
            if (existingStudent == null)
                return false;
            _context.Students.Remove(existingStudent);
            await _context.SaveChangesAsync();
            return true;
        }        
    }
}
