using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using API.DTOS;
using API.Helper;
using Microsoft.AspNetCore.Http;

namespace API.DTOS {
    public class EmployeesFileDto {
        public int Length { get; set; }
        public string Name { get; set; }
        public IFormFile File { get; set; }
        public DateTime Date { get; set; }
        public string ContentType { get; set; }

    }

    public class EmployeeDataListToReturnDto {

        public int Id { get; set; }
        public string NationalId { get; set; }
        public string Name { get; set; }
        public string Collage { get; set; }
        public string Department { get; set; }
        public string Grade { get; set; }
        public string Gender { get; set; }
        public string Code { get; set; }
        public PaymentTypeDto EmployeePaymentType { get; set; }

        public EmployeeDataListToReturnDto () {
            this.EmployeePaymentType = new PaymentTypeDto ();
        }
    }
    public class PaymentTypeDto {
        public bool HasATM { get; set; }
        public bool HasOrder { get; set; }
        public bool HasBank { get; set; }
        public bool HasPost { get; set; }
    }
    // public class EmployeeAddToReturnDto {
    //     public List<string> CollageList { get; set; }
    //     public List<string> DepartmentList { get; set; }
    //     public EmployeeAddItemDto EmployeeData { get; set; }
    // }
    public class EmployeeAddItemDto {
        public int Id { get; set; }
        
        public string NationalId { get; set; }
        public string KnownAs { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Collage { get; set; }
        public string Grade { get; set; }
        public int? DepartmentId { get; set; }
        public string Email { get; set; }

        public EmployeeAddItemDto () {

        }

    }
    public class EmployeDetailsToGetDto {
        public int Id { get; set; }
        public string NationalId { get; set; }
        public string Name { get; set; }
        public string KnownAs { get; set; }
        public string Phone { get; set; }
        public string Collage { get; set; }
        public string Section { get; set; }
        public string SallaryOption { get; set; }
        public string OtherOption { get; set; }
        public string Grade { get; set; }
        public EmployeeDepartmentDto Department { get; set; }
        public string Email { get; set; }
        public bool HasATM { get; set; }
        public bool HasBank { get; set; }
        public bool HasOrder { get; set; }
        public bool HasPost { get; set; }

        public EmployeeBankDto Bank { get; set; }
        public EmployeePostDto Post { get; set; }
        public EmployeeOrderDto Order { get; set; }
        public IEnumerable<CollageDto> CollageList { get; set; }
        public IEnumerable<GradeDto> GradeList { get; set; }
        // public List<EmployeeDepartmentDto> DepartmentList { get; set; }
        public EmployeDetailsToGetDto () {
            this.Bank = new EmployeeBankDto ();

        }

      //  public bool HaveData { get { return this.Bank!= null ? true : false; } }
    }
    public class EmployeeBankDto {

        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }

    }
    public class EmployeeOrderDto {
        public int Id { get; set; }
        public int orderBankCode { get; set; }
        public string OrderBankName { get; set; }
        public int orderBranchCode { get; set; }
        public string OrderBranchName { get; set; }
        public string OrderAccountNum { get; set; }

    }

    public class EmployeePostDto {
        public int Id { get; set; }
        public string PostTo { get; set; }
        public string PostAddress { get; set; }
        public string PostPhone { get; set; }
    }
    public class EmployeeDepartmentDto {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class DepartmentDto {
        public int Id { get; set; }
        public string Name { get; set; }

    }
    public class CollageDto {
        public string Name { get; set; }
    }
    public class GradeDto {
        public string Name { get; set; }
    }

    public class EmployeeFinincialDto {
        public string EmployeeId { get; set; }
        public short Year { get; set; }

        public ICollection<EmployeeFinincialDataDto> EmployeeFinincialDataDto { get; set; }
        public EmployeeFinincialDto () {
            this.EmployeeFinincialDataDto = new Collection<EmployeeFinincialDataDto> ();

        }
     
    }

    public class EmployeeFinincialDataDto {
        public int AccountId { get; set; }
        public float Value { get; set; } = 0;
        public string AccountState { get; set; } //Crediti- Debit -None
        public string AdditionalValue { get; set; }
    }
}