using API.DTOS;
using API.Models;
using AutoMapper;

namespace API.Helper
{
    public class DomainProfile :Profile
    {
        public DomainProfile()
        {
            CreateMap<SignupDto, User>();


            CreateMap<User, UserToReturnDto>();
        }
    }
}