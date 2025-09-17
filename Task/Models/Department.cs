using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Task.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required]
        [MaxLength(100)]
        public string DepartementName { get; set; }

        // Navigation property (1 Department -> Many Employees)
        public ICollection<Employee> Employees { get; set; }
    }
}
