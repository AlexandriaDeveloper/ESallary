using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data.Interface;
using API.Helper;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [Authorize ()]
    [Route ("api/[controller]")]
    [ApiController]
    public class DailyController : ControllerBase {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private object debug;

        public DailyController (IUnitOfWork uow, IMapper mapper) {
            this._uow = uow;
            this._mapper = mapper;
        }

        [HttpGet ()]
        public async Task<ActionResult> GetList ([FromQuery] EmployeeParams<Employee> param) {
            var dailies=await _uow.DailyRepository.Get("",x => x.OrderBy(t => t.Details));
            var empPagedList = await PagedList<Daily>.CreateAsync (dailies.AsQueryable (), param.PageNumber, param.PageSize);
            var empsToReturn = _mapper.Map<IEnumerable<DailyListToReturn>> (empPagedList);
            Response.AddPageination (empPagedList.CurrentPage, empPagedList.PageSize, empPagedList.TotalCount, empPagedList.TotalPages);

            return Ok (empsToReturn);
        }
    }
}