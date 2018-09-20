using System.Threading.Tasks;
using API.DTOS;
using API.Models;

namespace API.Data.Interface
{
    public interface IUserRepository :IRepository<User>
    {
        Task SignupUser (SignupDto entity);
       Task LoginUser (User entity);
    }
}