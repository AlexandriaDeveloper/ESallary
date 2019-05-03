using System.ComponentModel.DataAnnotations;

namespace API.Models {
    public class EmployeePost {
        [Key]
        public int Id { get; set; }
        public int? EmployeeId { get; set; }

        [StringLength (100)]
        public string PostTo { get; set; }

        [StringLength (100)]
        public string PostAddress { get; set; }

        [StringLength (11)]
        public string PostPhone { get; set; }
        public virtual Employee Employee { get; set; }
    }
}