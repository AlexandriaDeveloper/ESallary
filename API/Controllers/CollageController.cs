using System.Threading.Tasks;
using API.Data.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [Authorize ()]
    [Route ("api/[controller]")]
    [ApiController]
    public class CollageController : ControllerBase {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CollageController (IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //TODO: Implement Realistic Implementation
        
            return Ok();
        }

    }
}