using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
     public class FileType {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public Collection<File> Files { get; set; }
        public FileType () {
            this.Files = new Collection<File> ();
        }
    }
}