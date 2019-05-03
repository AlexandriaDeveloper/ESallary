using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models {
    public class Employee {
        [Key]
        public int Id { get; set; }
        [MaxLength (14)]
        [Required]
        public string NationalId { get; set; }

        [StringLength (150)]       
         [Required]
        public string Name { get; set; }

        [StringLength (150)]      
          [Required]
        public string KnownAs { get; set; }

        [StringLength (11)]
        public string Phone { get; set; }

        [StringLength (50)]
        public string Collage { get; set; }

        [StringLength (150)]
        public string Section { get; set; }

        [StringLength (150)]
        public string Grade { get; set; }
        public int? DepartmentId { get; set; }

        [StringLength (150)]
        public string Email { get; set; }

        [StringLength (10)]
        public string Gender { get; set; }
        public DateTime? DOB { get; set; }

        [StringLength (10)]
        public string Code { get; set; }
        public virtual Department Department { get; set; }
        public virtual EmployeeBank EmployeeBank { get; set; }
        public virtual EmployeeOrder EmployeeOrder { get; set; }
        public virtual EmployeePost EmployeePost { get; set; }

        public bool HasATM { get; set; } = false;

        public bool HasBank { get; set; } = false;
        public bool HasOrder { get; set; } = false;

        public bool HasPost { get; set; } = false;

        [StringLength (150)]
        public string SallaryOption { get; set; }

        [StringLength (150)]
        public string OtherOption { get; set; }

        public bool Deleted { get; set; }
        public virtual ICollection<EmployeeFinincialData> EmployeeFinincialData { get; set; }
        public virtual ICollection<FileDetail> FileDetails { get; set; }
        public virtual ICollection<EmployeeDeductions> EmployeeDeductions { get; set; }
        public virtual ICollection<EmployeeDeductionsPaied> EmployeeDeductionsPaied { get; set; }
        public virtual ICollection<EmployeePartTime> EmployeePartTime { get; set; }
             public Employee () {
            // if (EmployeeBank == null) {
            //     EmployeeBank = new EmployeeBank ();
            // }
            // Department = new Department ();
        }
    }
}