using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using API.Data.Interface;
using API.DTOS;
using API.Helper;
using API.Models;
using API.Service;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace API.Controllers {
    [Authorize ()]
    [Route ("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase {
        private object thread;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public EmployeesController (IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet ("getAllEmployees")]
        public async Task<IActionResult> GetAllEmployees ([FromQuery] EmpsParams param) {
            string gender = "";
            string[] splitedName = param.Name.ToNormalizedString ().Split (" ");
            if (param.Male == true && param.Female == false) { gender = "Male"; } else if (param.Female == true && param.Male == false) { gender = "Female"; } else if (param.Female == false && param.Male == false) { gender = "None"; }
            var emps = await _uow.EmployeeRepository.Get (param, "Department", x => x.OrderBy (t => t.Name), emp =>
                splitedName.All (p => emp.KnownAs.Contains (p)) &&
                emp.KnownAs.StartsWith (splitedName[0]) &&
                emp.NationalId.StartsWith (param.NationalId == null ? string.Empty : param.NationalId) &&
                emp.Grade.Contains (param.Grade == null ? string.Empty : param.Grade) &&
                emp.Department.Name.Contains (param.Department) &&
                emp.Section.Contains (param.Section == null ? string.Empty : param.Section) &&
                emp.Collage.Contains (param.Collage == null ? string.Empty : param.Collage) &&
                emp.Code.Contains ("") &&
                emp.Gender.StartsWith (gender) &&
                emp.Deleted == param.Deleted
            );

            var empPagedList = await PagedList<Employee>.CreateAsync (emps.AsQueryable (), param.PageNumber, param.PageSize);
            var empsToReturn = _mapper.Map<IEnumerable<EmployeeDataListToReturnDto>> (empPagedList);
            Response.AddPageination (empPagedList.CurrentPage, empPagedList.PageSize, empPagedList.TotalCount, empPagedList.TotalPages);
            return Ok (empsToReturn);

        }

        [HttpGet ("employeesByName/{empName}")]
        public async Task<IActionResult> EmployeesByName (string empName) {
            if (!string.IsNullOrEmpty (empName))
                empName = empName.ToNormalizedString ();

            var empFromDB = await _uow.EmployeeRepository.Get (x => x.KnownAs.Contains (empName.ToNormalizedString ()) && (x.Code == string.Empty || x.Code == null));

            var empToReturn = _mapper.Map<EmployeeAutoCompleteDto[]> (empFromDB);

            return Ok (empToReturn);

        }

        [HttpGet ("employeesByCode/{code}")]
        public async Task<IActionResult> EmployeesByCode (string code) {
            if (!string.IsNullOrEmpty (code))
                return Ok (await _uow.EmployeeRepository.Get (x => x.Code.Equals (code)));
            return Ok ();

        }

        [HttpGet ("getDeletedEmployees")]
        public async Task<IActionResult> GetDeletedEmployees ([FromQuery] EmpsParams param) {
            param.Deleted = true;
            var result =await GetAllEmployees (param);
            return Ok (result);

        }

        [HttpGet ("getEmployeeDetails/{NationalId}")]
        public async Task<IActionResult> GetEmployeeDetails (string NationalId) {
            var empFromDb = await _uow.EmployeeRepository.Get (x => x.NationalId == NationalId, "EmployeeBank,EmployeeOrder,EmployeePost,Department,EmployeeOrder.BankBranch")
                .FirstOrDefaultAsync ();

            if (empFromDb == null) {
                return NotFound ();
            }
            var empToReturn = _mapper.Map<EmployeDetailsToGetDto> (empFromDb);
            empToReturn = await GetEmployeeOrder (empToReturn, empFromDb);
            empToReturn =  GetEmployeePost (empToReturn, empFromDb);
            empToReturn.CollageList = GetCollages ();
            empToReturn.GradeList = GetGrades ();
            // var departments = await _uow.EmployeeRepository.GetDepartmentsList ();
            // empToReturn.DepartmentList = _mapper.Map<List<EmployeeDepartmentDto>> (departments);

            return Ok (empToReturn);
        }

        [HttpPost ("getEmployeeSearchMethod")]
        public async Task<IActionResult> GetEmployeeSearchMethod (EmployeeAutoCompleteDto model) {

            if (model.Id.HasValue &&  !string.IsNullOrEmpty (model.Code)) {
                return BadRequest (" من فضلك ابحث اما بالكود او بالاسم فى حالة الموظف ليس له كود فقط");
            }
             if (! model.Id.HasValue &&  string.IsNullOrEmpty (model.Code)) {
                return BadRequest (" من فضلك ابحث اما بالكود او بالاسم فى حالة الموظف ليس له كود فقط");
            }
            Employee emp = null;
            if (model.Id.HasValue) {
                emp = await _uow.EmployeeRepository.Get (model.Id.Value);
            }
            if (!string.IsNullOrEmpty (model.Code)) {
                emp = _uow.EmployeeRepository.Get (x => x.Code == model.Code).Result.SingleOrDefault ();
            }
            if (emp == null)
                return BadRequest ();
            var empToReturn = _mapper.Map<EmployeeDataListToReturnDto> (emp);
            
            return Ok (empToReturn);

        }

        [HttpPut ("putEmployeeDetails")]
        public async Task<IActionResult> PutEmployeeDetails (EmployeDetailsToGetDto model) {

            var modelToDb = await _uow.EmployeeRepository.Get (x => x.NationalId == model.NationalId, includes: "EmployeeOrder").SingleOrDefaultAsync ();
            if (modelToDb == null) {
                return NotFound ();
            }
            modelToDb = UpdateEmployeeBasicData (modelToDb, model);
            var orderResult = await UpdateEmployeeOrder (modelToDb, model);
            if (orderResult == null) {
                return BadRequest ();
            } else {
                modelToDb = orderResult;
            }
            var postResult = await UpdateEmployeePost (modelToDb, model);
            if (postResult == null) {
                return BadRequest ();
            } else {
                modelToDb = postResult;
            }
            _uow.EmployeeRepository.Update (modelToDb);
            await _uow.SaveChangesAsync ();
            return Ok ();
        }

        [HttpPost ("postEmployeeFinincialData")]
        public async Task<IActionResult> PostEmployeeFinincialData ([FromBody] EmployeeFinincialDto model) {
            if (model == null) {
                return BadRequest ();
            }
            var emp = await _uow.EmployeeRepository.Get (x => x.NationalId == model.EmployeeId, "EmployeeFinincialData").AsTracking ().SingleOrDefaultAsync ();

            if (emp == null) {
                return NotFound ();
            }
            if (emp == null) {
                emp.EmployeeFinincialData = new Collection<EmployeeFinincialData> ();
            }

            foreach (var item in model.EmployeeFinincialDataDto) {
                //check exist 
                var acc = emp.EmployeeFinincialData.Where (x => x.AccountId == item.AccountId && x.Year == model.Year).SingleOrDefault ();
                if (acc != null) {
                    if (acc.Value != item.Value && item.Value > 0) {
                        acc.AccountState = item.AccountState;
                        acc.AccountId = item.AccountId;
                        acc.Value = item.Value;
                        acc.AdditionalValue = item.AdditionalValue;
                    }
                    if (item.Value == 0) {
                        emp.EmployeeFinincialData.Remove (acc);
                    }

                } else {
                    if (item.Value > 0) {
                        // emp.EmployeeFinincialData.Add (new EmployeeFinincialData () {
                        //     AccountId = item.AccountId,
                        //         Value = item.Value,
                        //         EmployeeId = model.Id,
                        //         Year = model.Year,
                        //         AccountState = item.AccountState

                        // });

                    }
                }

            }
            _uow.EmployeeRepository.Update (emp);
            await _uow.SaveChangesAsync ();
            return Created ("", null);
        }

        [HttpPatch ("putEmployeeBank")]
        public async Task<IActionResult> PutEmployeeBank (EmployeDetailsToGetDto model) {

            var modelToDb = await _uow.EmployeeRepository.Get (model.Id);
            if (modelToDb == null) {
                return NotFound ();
            }
            if (modelToDb.HasOrder) {
                modelToDb.EmployeeOrder = new EmployeeOrder () {

                };
            } else {

            }
            await _uow.SaveChangesAsync ();
            return Ok ();
        }

        [HttpPatch ("putAddEmployeeOrderBank/{id}")]
        public async Task<IActionResult> putAddEmployeeOrderBank (string id, EmployeDetailsToGetDto model) {

            var modelToDb = await _uow.EmployeeRepository.Get (model.Id);
            if (modelToDb == null) {
                return NotFound ();
            }
            if (modelToDb.HasOrder) {
                modelToDb.EmployeeOrder = new EmployeeOrder () {

                };
            } else {

            }
            await _uow.SaveChangesAsync ();
            return Ok ();
        }

        [HttpPost ("UploadEmployees")]
        public async Task<IActionResult> UploadEmployees ([FromForm] EmployeesFileDto empFile) {
            await _uow.EmployeeRepository.AddEmployeesFileAsync (empFile);
            return Ok ("file uploaded");
        }

        [HttpPost ("addEmployee")]
        public async Task<IActionResult> AddEmployee (EmployeeAddItemDto model) {
            if (!model.NationalId.ValidateNtionalId ()) {
                return BadRequest ("الرقم القومى غير صحيح ");
            }
            Employee emp = _uow.EmployeeRepository.Get (x => x.NationalId == model.NationalId).Result.FirstOrDefault ();
            if (emp != null) {
                return BadRequest ("عفوا الرقم القومى مسجل من قبل ");
            } else {
                emp = _uow.EmployeeRepository.Get (x => x.Name == model.Name).Result.FirstOrDefault ();
                if (emp != null) { return BadRequest ("عفوا هذا الأسم مسجل من قبل "); }
            }
            var empToDb = _mapper.Map<Employee> (model);

            _uow.EmployeeRepository.Add (empToDb);
            await _uow.SaveChangesAsync ();
            return Ok (empToDb);
        }

        [HttpPatch ("deleteEmployee/{id}/{state}")]
        public async Task<IActionResult> DeleteEmployee (int id, bool State) {
            var employee = await _uow.EmployeeRepository.Get (id);
            if (employee == null) {
                return NotFound ();
            }
            employee.Deleted = State;
            await _uow.SaveChangesAsync ();

            return Ok ();
        }

        [HttpDelete ("deleteEmployeePermenatly/{id}")]
        public async Task<IActionResult> DeleteEmployeePermenatly (int id) {
            var employee = await _uow.EmployeeRepository.Get (id);
            if (employee == null) {
                return NotFound ();
            }
            _uow.EmployeeRepository.Delete (employee);
            await _uow.SaveChangesAsync ();

            return Ok ();
        }

        [HttpGet ("getCollages")]
        public IEnumerable<CollageDto> GetCollages () {
            var collageFromDb = _uow.EmployeeRepository.GetCollageList ().Result;
            List<CollageDto> collageToReturn = new List<CollageDto> ();
            foreach (var collage in collageFromDb) {
                collageToReturn.Add (new CollageDto () {
                    Name = collage
                });
            }
            return collageToReturn;
        }

        [HttpGet ("getGrades")]
        public IEnumerable<GradeDto> GetGrades () {
            var gradeFromDb = _uow.EmployeeRepository.GetGradesList ().Result;
            List<GradeDto> gradeToReturn = new List<GradeDto> ();
            foreach (var grade in gradeFromDb) {
                gradeToReturn.Add (new GradeDto () {
                    Name = grade
                });
            }
            return gradeToReturn;
        }

        [HttpGet ("getDepartments")]
        public IActionResult GetCollage () {
            var departmentFromDb = _uow.EmployeeRepository.GetDepartmentsList ().Result;
            List<DepartmentDto> departmentToReturn = new List<DepartmentDto> ();
            foreach (var department in departmentFromDb) {
                departmentToReturn.Add (new DepartmentDto () {
                    Name = department.Name,
                        Id = department.Id
                });
            }
            return Ok (departmentToReturn);
        }
        private Employee UpdateEmployeeBasicData (Employee modelToDb, EmployeDetailsToGetDto model) {
            modelToDb.KnownAs = model.KnownAs;
            modelToDb.DepartmentId = model.Department.Id;
            modelToDb.BankOption = model.BankOption;
            modelToDb.ATMOption = model.ATMOption;
            modelToDb.Phone = model.Phone;
            modelToDb.Email = model.Email;
            return modelToDb;
        }
        private async Task<Employee> UpdateEmployeeOrder (Employee modelToDb, EmployeDetailsToGetDto model) {
            if (modelToDb.HasATM == false && modelToDb.HasBank == false) {
                modelToDb.NationalId = model.NationalId;
                modelToDb.Name = model.Name;

            }
            if (modelToDb.HasOrder && model.HasOrder) {
                var orderEntery = _uow.OrderRepository.Get (x => x.EmployeeId == model.Id).Result.SingleOrDefault ();
                if (orderEntery == null) {
                    return null;
                }
                orderEntery.BranchCode = model.Order.orderBranchCode;
                orderEntery.OrderAccountNumber = model.Order.OrderAccountNum;
                _uow.OrderRepository.Update (orderEntery);

            }

            if (!modelToDb.HasOrder && model.HasOrder) {
                if (model.Order == null) {
                    return null;
                }
                modelToDb.HasOrder = true;
                modelToDb.EmployeeOrder = new EmployeeOrder () {
                    EmployeeId = model.Id,
                    BranchCode = model.Order.orderBranchCode,
                    OrderAccountNumber = model.Order.OrderAccountNum,

                };
            } else if (!model.HasOrder && modelToDb.HasOrder) {
                var modelToDelete = await _uow.OrderRepository.Get (x => x.Id == modelToDb.Id);
                if (modelToDelete == null) {
                    return null;
                }
                modelToDb.HasOrder = false;
                if (modelToDb.BankOption == PaymentTypeConst.PaymentOrder) {
                    modelToDb.BankOption = string.Empty;
                }
                if (modelToDb.ATMOption == PaymentTypeConst.PaymentOrder) {
                    modelToDb.ATMOption = string.Empty;
                }
                _uow.OrderRepository.Delete (modelToDelete.SingleOrDefault ());
            }
            return modelToDb;
        }

        private async Task<Employee> UpdateEmployeePost (Employee modelToDb, EmployeDetailsToGetDto model) {
            if (modelToDb.HasATM == false && modelToDb.HasBank == false) {
                modelToDb.NationalId = model.NationalId;
                modelToDb.Name = model.Name;

            }
            if (modelToDb.HasPost && model.HasPost) {
                var postEntery = _uow.PostRepository.Get (x => x.EmployeeId == model.Id).Result.SingleOrDefault ();
                if (postEntery == null) {
                    return null;
                }
                postEntery.PostTo = model.Post.PostTo;
                postEntery.PostAddress = model.Post.PostAddress;
                postEntery.PostPhone = model.Post.PostPhone;
                _uow.PostRepository.Update (postEntery);

            }

            if (!modelToDb.HasPost && model.HasPost) {
             
                modelToDb.HasPost = true;
                modelToDb.EmployeePost = new EmployeePost () {
                    EmployeeId = model.Id,
                    PostTo = model.Post.PostTo,
                    PostAddress = model.Post.PostAddress,
                    PostPhone = model.Post.PostPhone,

                };
            } else if (!model.HasPost && modelToDb.HasPost) {
                var modelToDelete = await _uow.PostRepository.Get (x => x.EmployeeId == modelToDb.Id);
                if (modelToDelete == null) {
                    return null;
                }
                modelToDb.HasPost = false;
                if (modelToDb.BankOption == PaymentTypeConst.PersonalPost) {
                    modelToDb.BankOption = string.Empty;
                }
                if (modelToDb.ATMOption == PaymentTypeConst.PersonalPost) {
                    modelToDb.ATMOption = string.Empty;
                }
                _uow.PostRepository.Delete (modelToDelete.SingleOrDefault ());
            }
            return modelToDb;
        }
        private async Task<EmployeDetailsToGetDto> GetEmployeeOrder (EmployeDetailsToGetDto empToReturn, Employee empFromDb) {
            if (empToReturn.HasOrder) {
                BankBranch branch = await _uow.BankBranchRepository.Get (empFromDb.EmployeeOrder.BranchCode.Value);
                Models.Bank bank = _uow.BankRepository.Get (x => x.Id == branch.BankId).Result.SingleOrDefault ();

                empToReturn.Order = new EmployeeOrderDto () {
                    Id = empFromDb.EmployeeOrder.Id,
                    OrderAccountNum = empFromDb.EmployeeOrder.OrderAccountNumber,
                    orderBranchCode = empFromDb.EmployeeOrder.BranchCode.Value,
                    OrderBankName = bank.BankName,
                    OrderBranchName = branch.BranchName
                };
            }
            return empToReturn;
        }
        private  EmployeDetailsToGetDto GetEmployeePost (EmployeDetailsToGetDto empToReturn, Employee empFromDb) {
            if (empToReturn.HasPost) {
                empToReturn.Post = new EmployeePostDto () {
                    Id = empFromDb.EmployeeOrder.Id,
                    PostTo = empFromDb.EmployeePost.PostTo,
                    PostAddress = empFromDb.EmployeePost.PostAddress,
                    PostPhone = empFromDb.EmployeePost.PostPhone

                };
            }
            return empToReturn;
        }

    }

}