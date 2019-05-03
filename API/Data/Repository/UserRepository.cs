using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Data.Interface;
using API.DTOS;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace API.Data.Repository {
    public class UserRepository : Repository<User>, IUserRepository {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public UserRepository (DataContext context, IConfiguration config, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager) : base (context) {

            _context = context;
            _mapper = mapper;
            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task SignupUser (SignupDto entity) {
            var userToDb = _mapper.Map<SignupDto, User> (entity);
            if (userToDb != null)
                await _userManager.CreateAsync (userToDb, entity.Password);
        }
        public async Task<Genereareted> LoginUser (LoginUserDto entity) {
            var user = await getUser (entity.UserName);
            var result = await _signInManager.CheckPasswordSignInAsync (user, entity.Password, entity.Remember);
            var Genereareted = new Genereareted ();
            Genereareted.Result = result;
            if (user != null && result.Succeeded) {

                var userToReturn = _mapper.Map<User, UserToReturnDto> (user);
                Genereareted.Token = await GenerateJwtToken (user);
                Genereareted.User = JsonConvert.SerializeObject (userToReturn);
              

            }
          return Genereareted;
        }

        public async Task<string> GenerateJwtToken (User user) {

            var claims = new List<Claim> {
                new Claim (ClaimTypes.Name, user.UserName),
                new Claim (ClaimTypes.NameIdentifier, user.Id.ToString ())
            };
            var roles = await _userManager.GetRolesAsync (user);

            foreach (var role in roles) {
                claims.Add (new Claim (ClaimTypes.Role, role));

            }
            var signingKey = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (_config.GetSection ("AppSettings:Token").Value));
            var token = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity (claims),
                Expires = DateTime.Now.AddDays (1),
                SigningCredentials = new SigningCredentials (signingKey, SecurityAlgorithms.HmacSha512)
            };
            var tokenHandler = new JwtSecurityTokenHandler ();
            var createdToken = tokenHandler.CreateToken (token);
            return tokenHandler.WriteToken (createdToken);
        }
        public async Task<User> getUser (string username) {
            var user = await _userManager.FindByNameAsync (username);
            if (user != null)
                return user;
            return null;
        }

    }

    public class Genereareted {
        public string Token { get; set; }
        public string User { get; set; }
        public SignInResult Result { get; set; }
    }
}