using System.ComponentModel.DataAnnotations;

namespace API.Models {
    public class EmployeeOrder {
        [Key]
        public int Id { get; set; }
        public int? EmployeeId { get; set; }

        public int? BranchCode { get; set; }

        [StringLength (20)]
        public string OrderAccountNumber { get; set; }
        public virtual Employee Employee { get; set; }

        public BankBranch BankBranch { get; set; }

    }
}