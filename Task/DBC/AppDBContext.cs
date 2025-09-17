using Microsoft.EntityFrameworkCore;

namespace Task.DBC
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions options): base(options)
        {
            
        }
        public DbSet<Models.Department> Departments { get; set; }
        public DbSet<Models.Employee> Employees { get; set; }
    }
}
