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

namespace API.Controllers {
    [Authorize ()]
    [Route ("api/[controller]")]
    [ApiController]
    public class BankController : ControllerBase {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public BankController (IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet ("getBank")]
        public async Task<IActionResult> GetBank () {
            var banks = await _uow.BankRepository.Get ( "", x => x.OrderBy (b => b.BankName));

            var bankToReturn = _mapper.Map<IEnumerable<BankDto>> (banks);
            return Ok (bankToReturn);
        }

        [HttpGet ("getBankBranch/{id}")]
        public async Task<IActionResult> GetBankBranch (int id) {
            var bankBranches = await _uow.BankBranchRepository.Get ( "", x => x.OrderBy (b => b.BranchName), x => x.BankId == id);
            return Ok (bankBranches);
        }

        [HttpPost ("newBank")]
        public async Task<IActionResult> NewBank (BankDto model) {
            var bankExist = await _uow.BankRepository.Get (x => x.BankName == model.BankName);
            if (bankExist.SingleOrDefault () != null) {
                return BadRequest ("عفوا انت تقوم بتسجيل بنك سبق تسجيله من قبل ");
            }
            var modelToDb = _mapper.Map<Bank> (model);
            _uow.BankRepository.Add (modelToDb);
            await _uow.SaveChangesAsync ();
            var modelToReturn = _mapper.Map<BankDto> (modelToDb);
            return Ok (modelToReturn);
        }

        [HttpPost ("newBranch")]
        public async Task<IActionResult> NewBranch (BankBranchDto model) {
            var bank = _uow.BankRepository.Get (x => x.Id == model.BankId, includes: "Branches").SingleOrDefault ();

            if (bank == null) {
                return NotFound ();
            }
            var branchExist = bank.Branches.SingleOrDefault (x => x.BranchName == model.BranchName);
            if (branchExist != null) {
                return BadRequest (" هذا الفرع مسجل من قبل ");
            }
            var branchtoDb = new BankBranch () {
                BankId = model.BankId,
                BranchName = model.BranchName,
                Bank = bank
            };
            bank.Branches.Add (branchtoDb);
            _uow.BankRepository.Update (bank);
            await _uow.SaveChangesAsync ();
            return Ok (bank);
        }
    }
}