using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class EmployeeDeductions
    {
        [Key]
        public int Id { get; set; }
        [StringLength(14)]
        [Required]
        public int EmployeeId { get; set; }
        [StringLength(6)]
        public string OrderNum { get; set; }
        public float? Sanctions { get; set; }
        public float? EWazifi { get; set; }
        public float? EMokamel { get; set; }
        public float? MWazifi { get; set; }
        public float? MMokamel { get; set; }
        public int? Vacation { get; set; }
        public int Mokafaa { get; set; }
        public Employee Employee { get; set; }
    }
}