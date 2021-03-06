using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using API.DTOS;
using API.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
/*
public class FileDTO {

    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string Code { get; set; }

    [Required]
    //باب اول - باب ثانى - دائن  
    public string PaymentMethod { get; set; }
    // بطاقات حكومية - تحويلات بنكيه
    [Required]
    public string PaymentType { get; set; }
    public int? DailyId { get; set; }

    [Required]
    public int? FileTypeId { get; set; }

    [Required]
    public string CollageName { get; set; }

    [Required]

    public decimal TotalSum { get; set; }

    [Required]
    public string FileNum55 { get; set; }
    public string FileNum224 { get; set; }
    public string EntryName { get; set; }
    public DateTime CreatedDate { get; set; }

    //مرتبات - ساعات زائدة - اشراف و مناقشه - برنامج دولى 
    //public FileType FileType { get; set; }
    //public PayemntType PaymentType { get; set; }
    public ICollection<FileDetailsDto> FileDetails { get; set; }
    public FileDTO () {
        //this.FileDetails = new Collection<FileDetail> ();

    }

} */
public class FileTypeDTO {
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }
}
public class FileDetailsDto {
    [Key]
    public int Id { get; set; }

    [Required]
    public int FileId { get; set; }

    public bool Checked { get; set; } = false;
    public string State { get; set; }
    public string Warrning { get; set; }

    public string SelectedPaymentMethod { get; set; }
    public List<string> PaymentMethod { get; set; }

    public decimal Net { get; set; }

    public List<SuggestionsFileDetailsDto> SuggestedEmployee { get; set; }
    public EmployeeDataListToReturnDto EmployeeData { get; set; }
    public FileDetailsDto () {
        this.PaymentMethod = new List<string> ();
        if (EmployeeData == null) {
            EmployeeData = new EmployeeDataListToReturnDto ();
        }
    }
}
public class ImportedSheetDto {

    public string[] Sheets { get; set; }
    public string Path { get; set; }
    public string PaymentType { get; set; }
    //  public string[] SelectedSheets { get; set; }
}
public class SuggestionsFileDetailsDto {

    public int? EmployeeId { get; set; }
    public string Code { get; set; }
    public string AtmOption { get; set; }
    public string BankOption { get; set; }
    public string Name { get; set; }
    public string Collage { get; set; }
    public string Position { get; set; }
    private List<string> paymentMethod;
    public List<string> PaymentMethod {
        get {

            if (HasATM) {
                paymentMethod.Add (PaymentTypeConst.ATM);
            };
            if (HasBank) {
                paymentMethod.Add (PaymentTypeConst.Bank);
            };
            if (HasOrder) {
                paymentMethod.Add (PaymentTypeConst.PaymentOrder);
            };
            if (HasPost) {
                paymentMethod.Add (PaymentTypeConst.PersonalPost);
            };
            return paymentMethod;
        }
        set {

        }
    }
    public bool Checked { get; set; } = false;
    public string SelectedPaymentMethod { get; set; }
    public bool HasATM { get; set; }
    public bool HasBank { get; set; }
    public bool HasOrder { get; set; }
    public bool HasPost { get; set; }
    public SuggestionsFileDetailsDto () {
        this.PaymentMethod = new List<string> ();
        this.paymentMethod = new List<string> ();
    }

}
public class FilesDTO {
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
    public string FileTypeName { get; set; }

    [Required]
    public string CollageName { get; set; }

    [Required]
    [Column (TypeName = "decimal(9,2)")]
    public decimal TotalSum { get; set; }

    [Required]

    public string FileNum55 { get; set; }
    public string FileNum224 { get; set; }
    public string EntryName { get; set; }
    public DateTime? ReservedDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool Deleted { get; set; } = false;
    public bool Open { get; set; } = true;
    public ICollection<FileDetailsDto> FileDetails { get; set; }
    public FilesDTO () {
        this.ReservedDate = new DateTime ();
    }

}
public class DailyListToReturn{
    
        public int Id { get; set; }
        public string Details { get; set; }

        public decimal? Total { get; set; }
        public string EntryNumber { get; set; }
        public DateTime CreatedDate { get; set; }=DateTime.Now;
        public DateTime DailyDate { get; set; }
        public DateTime? MaturityDate { get; set; }
        public bool Open { get; set; }=true;

}