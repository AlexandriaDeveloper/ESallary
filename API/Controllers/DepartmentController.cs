using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data.Interface;
using API.DTOS;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [Authorize ()]
    [Route ("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public DepartmentController (IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartments () {
            //TODO: Implement Realistic Implementation
            var departments = await _uow.DepartmentRepository.Get ();
            if (departments == null) {
                return NotFound ();
            }
            var departmentsToReturn = _mapper.Map<IEnumerable<DepartmentDto>> (departments);

            return Ok (departmentsToReturn);
        }

        [HttpGet ("{id}")]
        public IActionResult GetDepartment (int Id) {
            //TODO: Implement Realistic Implementation
            var department = _uow.DepartmentRepository.Get (Id);
            if (department == null) {
                return NotFound ();
            }
            var departmentToReturn = _mapper.Map<DepartmentDto> (department);
            return Ok (departmentToReturn);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DepartmentDto model)
        {
            //TODO: Implement Realistic Implementation
            await Task.Yield();
            return Created("", null);
        }
    }
}