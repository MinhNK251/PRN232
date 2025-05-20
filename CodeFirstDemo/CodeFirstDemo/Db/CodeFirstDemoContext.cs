using CodeFirstDemo.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeFirstDemo.Db
{
    public class CodeFirstDemoContext : DbContext
    {
        public CodeFirstDemoContext(DbContextOptions<CodeFirstDemoContext> options) : base(options) { }
        public DbSet<Student> Students { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>();
            modelBuilder.Seed();
        }
    }
}
