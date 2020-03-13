using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace API.Models {
    public class Daily {
        [Key]
        public int Id { get; set; }
        public string Details { get; set; }
        [Column(TypeName = "decimal(9,2)")]
        public decimal? Total { get; set; }
        public string EntryNumber { get; set; }
        public DateTime CreatedDate { get; set; }=DateTime.Now;
        public DateTime DailyDate { get; set; }
        public DateTime? MaturityDate { get; set; }
        public bool Open { get; set; }=true;

        public Collection<File> Files { get; set; }
        public Daily()
        {
            this.Files= new Collection<File>();
        }
    }



 
    
}