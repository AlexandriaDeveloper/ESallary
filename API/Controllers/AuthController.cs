using System.Threading.Tasks;
using API.Data.Interface;
using API.DTOS;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [Route ("api/[controller]")]
    [ApiController]
    //[AllowAnonymous]
    public class AuthController : ControllerBase {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public AuthController (IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;

        }

        [HttpGet ("GetUsers")]
        public async Task<IActionResult> GetUsers () {
            var user = await _uow.UserRepository.Get ();
            return Ok (user);
        }

        [HttpPost ("Login")]
        public async Task<IActionResult> LoginUser (LoginUserDto login) {
            var result = await _uow.UserRepository.LoginUser (login);
            if (result.Result.Succeeded) {
                return Ok (result);
            }
            return Forbid ();
            // var result = await _uow.UserRepository.LoginUser (login);
            // if (result!= null)
            //     return Ok (result);
            // return Forbid ();
        }

        [HttpPost ("Signup")]
        public async Task<ActionResult> SignUpUser ([FromBody] SignupDto user) {
            if (user == null)
                return BadRequest ();

            await _uow.UserRepository.SignupUser (user);
            return Ok (user);
        }

    }
}