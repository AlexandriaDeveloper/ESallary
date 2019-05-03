using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace API.Models {
    public class Department {
        [Key]
        public int Id { get; set; }

        [StringLength (70)]
        [Required]
        public string Name { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public Department () {
            if (this.Employees == null) {
                this.Employees = new Collection<Employee> ();
            }

        }
    }
}