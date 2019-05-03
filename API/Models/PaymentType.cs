using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace API.Models {
    public class PayemntType {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public Collection<File> Files { get; set; }
        public PayemntType () {
            this.Files = new Collection<File> ();
        }
    }
}