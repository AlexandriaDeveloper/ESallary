using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class EmployeePartTime
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        [StringLength(150)]
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Employee Employee { get; set; }

    }
}