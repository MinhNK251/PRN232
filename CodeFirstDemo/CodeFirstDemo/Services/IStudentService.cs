using CodeFirstDemo.Db.Models;
using CodeFirstDemo.DTOs;

namespace CodeFirstDemo.Services
{
    public interface IStudentService
    {
        Task<List<Student>> GetStudentsAsync();
        Task<Student> GetStudentAsync(int id);
        Task<Student> CreateStudentAsync(CreateStudentRequest request);
        Task<bool> UpdateStudentAsync(int id, UpdateStudentRequest request);
        Task<bool> DeleteStudentAsync(int id);
    }
}
