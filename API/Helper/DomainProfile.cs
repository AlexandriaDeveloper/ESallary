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

            CreateMap<Employee, EmployeeDataListToReturnDto> ()
                .ForMember (x => x.Name, opt => opt.MapFrom (t => t.KnownAs ))
                .ForMember (x => x.EmployeePaymentType, opt => opt.MapFrom (t => new PaymentTypeDto () {
                    HasATM = t.HasATM,
                        HasBank = t.HasBank,
                        HasOrder = t.HasOrder,
                        HasPost = t.HasPost
                }))
                .ForMember (x => x.Department, opt => opt.MapFrom (t => t.Department.Name));
            CreateMap<EmployeeDataListToReturnDto, Employee> ()
                .ForMember (x => x.Name, opt => opt.MapFrom (t => t.Name.ToNormalizedString()));
            CreateMap<DepartmentDto, Department> ();
            CreateMap<FileTypeDTO, FileType> ().ReverseMap ();
            CreateMap<FileDetailsDto, FileDetail> ().ReverseMap ();
            CreateMap<FileDTO, File> ().ReverseMap ();

            CreateMap<Employee, EmployeDetailsToGetDto> ()
                .ForMember (x => x.KnownAs, opt => opt.MapFrom (t => t.KnownAs))
                .ForMember (x => x.Bank, opt => opt.MapFrom (t => t.EmployeeBank))
                .ForMember (x => x.Department, opt => opt.MapFrom (t => t.Department));

            CreateMap<EmployeeAddItemDto, Employee> ()
                .ForMember (x => x.Section, opt => opt.MapFrom (t => t.Collage))
                .ForMember (x => x.KnownAs, opt => opt.MapFrom (t => t.Name))
                .ForMember (x => x.DOB, opt => opt.ResolveUsing (t => t.NationalId.GetDOBFromNationalId ()))
                .ForMember (x => x.Gender, opt => opt.ResolveUsing (t => t.NationalId.CheckGenderByNationalId ()))
                .ForMember (x => x.HasATM, opt => opt.MapFrom (t => false))
                .ForMember (x => x.HasOrder, opt => opt.MapFrom (t => false))
                .ForMember (x => x.HasBank, opt => opt.MapFrom (t => false))
                .ForMember (x => x.HasPost, opt => opt.MapFrom (t => false))
                .ForMember (x => x.Deleted, opt => opt.MapFrom (t => false))

                .ForMember (x => x.SallaryOption, opt => opt.MapFrom (t => PaymentTypeConst.InternalPost))
                .ForMember (x => x.OtherOption, opt => opt.MapFrom (t => PaymentTypeConst.InternalPost));

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
            //from Model To Database

        }

    }

}