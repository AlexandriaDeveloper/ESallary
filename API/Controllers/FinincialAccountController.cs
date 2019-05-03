using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data.Interface;
using API.DTOS;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers {
    [Authorize ()]
    [Route ("api/[controller]")]
    [ApiController]
    public class FinincialAccountController : ControllerBase {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public FinincialAccountController (IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get () {
            IEnumerable<Account> Accounts = await _uow.FinincialAccountRepository.Get ();
            if (Accounts == null)
                return NotFound ();

            IEnumerable<FinincialAccountDto> AccountToReturn = _mapper.Map<IEnumerable<FinincialAccountDto>> (Accounts);
            return Ok (AccountToReturn);
        }

        [HttpGet ("{id}")]
        public async Task<IActionResult> Get (string id) {
            var emp = await
            _uow.EmployeeRepository.Get (x => x.NationalId == id, "EmployeeFinincialData").ToListAsync ();

            if (emp.SingleOrDefault () == null)
                return NotFound ();

            var AccountToReturn = _mapper.Map<IEnumerable<EmployeeFinincialDataDto>> (emp.SingleOrDefault().EmployeeFinincialData);
            return Ok (AccountToReturn);
        }
    }
}