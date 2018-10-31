using System.Threading.Tasks;
using API.Data.Repository;
using API.DTOS;
using API.Models;
using Microsoft.AspNetCore.Identity;

namespace API.Data.Interface
{
    public interface IUserRepository :IRepository<User>
    {
        Task SignupUser (SignupDto entity);
        Task<Genereareted> LoginUser (LoginUserDto entity);
       Task<User> getUser (string username);
    }
}