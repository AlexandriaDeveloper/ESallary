using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace API.Models {
    public class File {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        //باب اول - باب ثانى - دائن  
        public string PaymentMethod { get; set; }
        // بطاقات حكومية - تحويلات بنكيه
        [Required]
        public string PaymentType { get; set; }
        public int? DailyId { get; set; }
        public int? FileTypeId { get; set; }
        [Required]
        public string CollageName { get; set; }
        [Required]
        [Column(TypeName = "decimal(9,2)")]
        public decimal TotalSum { get; set; }

        [Required]
       
        public string FileNum55 { get; set; }
        public string FileNum224 { get; set; }
        public string EntryName { get; set; }

        public Daily Daily { get; set; }
        //مرتبات - ساعات زائدة - اشراف و مناقشه - برنامج دولى 
        public FileType FileType { get; set; }

        public ICollection<FileDetail> FileDetails { get; set; }
        public File () {
            this.FileDetails = new Collection<FileDetail> ();
        }
    }
}