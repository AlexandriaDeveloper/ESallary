using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace API.Models {
    public class Account {
        [Key]
        public int Id { get; set; }

        [StringLength (200)]
        [Required]
        public string Name { get; set; }
      public  ICollection<EmployeeFinincialData> EmployeeFinincialData { get; set; }
        public Account () {
            this.EmployeeFinincialData = new Collection<EmployeeFinincialData> ();
        }
    }
}