using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Data.Interface;
using API.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers {
    [Route ("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase {

        private readonly IUnitOfWork _uow;

        public ValuesController (IUnitOfWork uow) {
           _uow = uow;
           
        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get () {
            var values = await this._uow.ValueRepository.Get();
            return Ok (values);
        }

        // GET api/values/5
        [HttpGet ("{id}")]
        public async Task<ActionResult<string>> Get (int id) {
            var value = await this._uow.ValueRepository.Get ( id);
            return Ok (value);
        }

        // POST api/values
        [HttpPost]
        public void Post ([FromBody] string value) { }

        // PUT api/values/5
        [HttpPut ("{id}")]
        public void Put (int id, [FromBody] string value) { }

        // DELETE api/values/5
        [HttpDelete ("{id}")]
        public void Delete (int id) { }
    }
}