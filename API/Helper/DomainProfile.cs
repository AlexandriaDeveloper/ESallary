using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using API.DTOS;
using API.Models;
using AutoMapper;

namespace API.Helper {
  public class DomainProfile : Profile {
    public DomainProfile () {
     CreateMap<DepartmentDto, Department> ();
      CreateMap<Employee, EmployeeDataListToReturnDto> ()
      .ForMember (x => x.HasATM, opt => opt.MapFrom (t => t.HasATM))
        .ForMember (x => x.Name, opt => opt.MapFrom (t => t.KnownAs))
        .ForMember (x => x.Department, opt => opt.MapFrom (t => t.Department));
      CreateMap<EmployeeDataListToReturnDto, Employee> ()        

        .ForMember (x => x.Name, opt => opt.MapFrom (t => t.Name.ToNormalizedString ()));
 
      CreateMap<FileTypeDTO, FileType> ().ReverseMap ();

      CreateMap<SuggestionsFileDetailsDto, Employee> ()
        .ForMember (x => x.Id, opt => opt.MapFrom (t => t.EmployeeId))
        .ForMember (x => x.KnownAs, opt => opt.MapFrom (t => t.Name))
        .ForMember (x => x.Grade, opt => opt.MapFrom (t => t.Position))
        .ReverseMap ();


     CreateMap<Employee,EmployeeDataListToReturnDto>()

        .ForMember (x => x.Id, opt => opt.AllowNull ())
        .ForMember (x => x.Name, opt => opt.MapFrom (t => t.KnownAs));       


      CreateMap<FileDetail,FileDetailsDto > ()
        .ForMember (x => x.SelectedPaymentMethod, opt => opt.MapFrom (t => t.PaymentMethod))
        .ForMember(x=> x.EmployeeData,opt => opt.MapFrom(t => t.Employee));
     
      CreateMap<FileDetailsDto,FileDetail > ()   
        .ForMember(x=> x.EmployeeName,opt => opt.MapFrom(t => t.EmployeeData.Name))
        .ForMember(x=> x.EmployeeId,opt => opt.MapFrom(t => t.EmployeeData.Id))
        .ForMember(x=> x.Code,opt => opt.MapFrom(t => t.EmployeeData.Code))
        .ForMember (x => x.PaymentMethod, opt => opt.MapFrom (t => t.SelectedPaymentMethod));
      CreateMap<FileDTO, File> ()
        .ForMember (x => x.FileDetails, opt => opt.MapFrom (t => t.FileDetails))
        
        .ReverseMap ();

      CreateMap<FileDetailsDto,Employee>()


      .ForMember(x=> x.BankOption  ,opt => opt.MapFrom (t => t.SelectedPaymentMethod==  PaymentTypeConst.Bank))
      .ForMember(x=> x.ATMOption  ,opt => opt.MapFrom (t => t.SelectedPaymentMethod== PaymentTypeConst.ATM))
      ;
      CreateMap<Employee,FileDetailsDto>()
      .ForMember(x=> x.EmployeeData , opt => opt.MapFrom(t => t  ));


      CreateMap<EmployeeAutoCompleteDto, Employee> ()
      .ForMember (x => x.KnownAs, opt => opt.MapFrom (t => t.Name))
      
      .ReverseMap ();
      CreateMap<Employee, EmployeDetailsToGetDto> ()
        .ForMember (x => x.KnownAs, opt => opt.MapFrom (t => t.KnownAs))
        .ForMember (x => x.Bank, opt => opt.MapFrom (t => t.EmployeeBank))
        .ForMember (x => x.Department, opt => opt.MapFrom (t => t.Department));

      CreateMap<EmployeeAddItemDto, Employee> ()
        .ForMember (x => x.Section, opt => opt.MapFrom (t => t.Collage))
        .ForMember (x => x.Code, opt => opt.MapFrom (t => string.Empty))
        .ForMember (x => x.KnownAs, opt => opt.MapFrom (t => t.Name.ToNormalizedString ().Trim()))
        .ForMember (x => x.DOB, opt => opt.ResolveUsing (t => t.NationalId.GetDOBFromNationalId ()))
        .ForMember (x => x.Gender, opt => opt.ResolveUsing (t => t.NationalId.CheckGenderByNationalId ()))
        .ForMember (x => x.HasATM, opt => opt.MapFrom (t => false))
        .ForMember (x => x.HasOrder, opt => opt.MapFrom (t => false))
        .ForMember (x => x.HasBank, opt => opt.MapFrom (t => false))
        .ForMember (x => x.HasPost, opt => opt.MapFrom (t => false))
        .ForMember (x => x.Deleted, opt => opt.MapFrom (t => false))

        .ForMember (x => x.ATMOption, opt => opt.MapFrom (t => PaymentTypeConst.InternalPost))
        .ForMember (x => x.BankOption, opt => opt.MapFrom (t => PaymentTypeConst.InternalPost));

      CreateMap<BankBranch, BankBranchDto> ();
      CreateMap<Account, FinincialAccountDto> ();

      CreateMap<EmployeeFinincialData, EmployeeFinincialDataDto> ().ReverseMap ();

      CreateMap<Bank, BankDto> ()
        .ForMember (x => x.BankId, opt => opt.MapFrom (t => t.Id))
        .ForMember (x => x.BankName, opt => opt.MapFrom (t => t.BankName))
        .ForMember (x => x.AccountMinLength, opt => opt.MapFrom (t => t.AccountMinLength))
        .ForMember (x => x.AccountMaxLength, opt => opt.MapFrom (t => t.AccountMaxLength))
        .ForMember (x => x.Branches, opt => opt.MapFrom (t => t.Branches))
        .ReverseMap ();

    }

  }

}