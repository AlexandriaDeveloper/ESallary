using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data.Interface;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [Authorize ()]
    [Route ("api/[controller]")]
    [ApiController]
    public class FileTypeController : ControllerBase {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public FileTypeController (IUnitOfWork uow, IMapper mapper) {
            this._uow = uow;
            this._mapper = mapper;
        }
        public async Task<IActionResult> Get () {
            var filesType = await _uow.FileTypeRepository.Get ();
            if (filesType == null)
                return BadRequest ();
            var fileTypeToReturn = _mapper.Map<IEnumerable<FileTypeDTO>> (filesType);
            return Ok (fileTypeToReturn.OrderBy (x => x.Name));
        }

        [HttpPost]
        public async Task<IActionResult> Post (FileTypeDTO fileType) {
            fileType.Name = fileType.Name.Trim ().ToLower ();
            var fileExist = await _uow.FileTypeRepository.Get (x => x.Name == fileType.Name);
            if (fileExist.Count () > 0) {
                ModelState.AddModelError ("", "هذا الملف مسجل من قبل ");
            }
            if (!ModelState.IsValid)
                return BadRequest (ModelState.Keys.SelectMany (key => ModelState[key].Errors));

            var fileToDb = _mapper.Map<FileType> (fileType);
            _uow.FileTypeRepository.Add (fileToDb);

            await _uow.SaveChangesAsync ();
            var fileTypeToReturn = _mapper.Map<FileTypeDTO> (fileToDb);
            return Ok (fileTypeToReturn);
        }
    }
}