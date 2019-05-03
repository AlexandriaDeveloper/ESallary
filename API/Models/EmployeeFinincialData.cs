using System.Collections;
using System.ComponentModel.DataAnnotations;
using API.Helper;

namespace API.Models {
    public class EmployeeFinincialData {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public short Year { get; set; }
        public int AccountId { get; set; }
        public float Value { get; set; } = 0;
        [StringLength (50)]
        public string AccountState { get; set; } = "None"; //Crediti- Debit -None
        [StringLength (200)]
        public string AdditionalValue { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Account Account { get; set; }
        public EmployeeFinincialData () {

        }

    }

}