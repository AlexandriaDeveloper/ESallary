using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models {
    public class EmployeeBank {
        [Key]
        public int Id { get; set; }
        public int? EmployeeId { get; set; }

        [MaxLength (10)]
        public string BankCode { get; set; }

        [StringLength (50)]
        public string BankName { get; set; }
    [MaxLength (10)]
      public string BranchCode { get; set; }

        [StringLength (50)]
        public string BranchName { get; set; }

        [StringLength (10)]
              public virtual Employee Employee { get; set; }
  
        public EmployeeBank () {
          
        }
    }
}