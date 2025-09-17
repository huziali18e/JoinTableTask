using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        [MaxLength(100)]
        public string EmployeeName { get; set; }

        // Foreign Key
        [ForeignKey("Department")]
        [Column("DepartmentID")] // 👈 Match EXACT column name from your DB
        public int DepartmentId { get; set; }

        // Navigation property
        public Department Department { get; set; }
    }
}
