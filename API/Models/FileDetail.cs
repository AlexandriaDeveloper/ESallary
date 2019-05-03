using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class FileDetail {
        [Key]
        public int Id { get; set; }
        public int? EmployeeId { get; set; }

        public string EmployeeName { get; set; }


        [Required]
        public int FileId { get; set; }

 
        public string PaymentMethod { get; set; }
        [Column(TypeName = "decimal(9,2)")]
        public decimal Net { get; set; }
        public File File { get; set; }
        public Employee Employee { get; set; }

    }
}