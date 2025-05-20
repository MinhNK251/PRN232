using CodeFirstDemo.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeFirstDemo.Db
{
    public static class DbSeeder
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(
                new Student { StudentId = 1, Name = "Minh" },
                new Student { StudentId = 2, Name = "Dat" },
                new Student { StudentId = 3, Name = "Diem" },
                new Student { StudentId = 4, Name = "Quynh" }
            );
        }
    }
}
