using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models {
    public class BankBranch {
        [Key]
        public int BranchCode { get; set; }

        [StringLength (150)]
        [Required]
        public string BranchName { get; set; }

        public int BankId { get; set; }
        public Bank Bank { get; set; }
        public ICollection<EmployeeOrder> EmployeesOrder { get; set; }
        public BankBranch () {

            this.Bank = new Bank ();
            this.EmployeesOrder = new Collection<EmployeeOrder> ();
        }
    }
}