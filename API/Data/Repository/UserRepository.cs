using System.Threading.Tasks;
using API.Data.Interface;
using API.DTOS;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace API.Data.Repository {
    public class UserRepository : Repository<User>, IUserRepository {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        public UserRepository (DataContext context, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager) : base (context) {

            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task SignupUser (SignupDto entity) {
            var userToDb = _mapper.Map<SignupDto, User> (entity);
            if(userToDb != null)
            await _userManager.CreateAsync (userToDb,entity.Password );
        }
        public async Task LoginUser (User entity) {
            await _signInManager.SignInAsync (entity, true);
        }

    }
}