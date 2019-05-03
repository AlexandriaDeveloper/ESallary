using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models {
    public class Bank {
        [Key]
        public int Id { get; set; }

        [StringLength (150)]
        [Required]
        public string BankName { get; set; }

        [Range (4, 44)][Required]
        public int AccountMinLength { get; set; }

        [Range (4, 44)][Required]
        public int AccountMaxLength { get; set; }
        public ICollection<BankBranch> Branches { get; set; }
        public Bank () {
            this.Branches = new Collection<BankBranch> ();
        }
    }
}