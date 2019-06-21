using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Data.Interface;
using API.DTOS;
using API.Helper;
using API.Models;
using API.Service;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository {
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository {
        private readonly DataContext _context;

        public EmployeeRepository (DataContext context) : base (context) {
            this._context = context;
        }

        public async Task<bool> CheckEmployeeExistById (string nationalId) {
            var emp = await _context.Employees.SingleOrDefaultAsync (x => x.NationalId == nationalId);
            if (emp == null) {
                return false;
            }
            return true;
        }

        public async Task<List<string>> GetCollageList () {

            return await _context.Employees.Where (x => x.Collage != "")
                .OrderBy (x => x.Collage)
                .Select (x => x.Collage).Distinct ().ToListAsync ();
        }
        public async Task<List<string>> GetGradesList () {

            return await _context.Employees.Where (x => x.Grade != "")
                .OrderBy (x => x.Grade)
                .Select (x => x.Grade).Distinct ().ToListAsync ();
        }
        public async Task<List<Department>> GetDepartmentsList () {

            return await _context.Departments
                .OrderBy (x => x.Name).ToListAsync ();

        }
        public async Task<EmployeeFinincialData> GetEmployeeFinance (int id) {

            return await _context.EmployeeFinincialData.FindAsync (id);

        }
        public async Task<List<Employee>> AddEmployeesFileAsync (EmployeesFileDto empData) {
            // get file
            var filePath = Path.GetTempFileName ();
            using (var stream = new FileStream (filePath, FileMode.Create)) {
                await empData.File.CopyToAsync (stream);
            }

            //read Table 
            var npoiHelper = new ExcelBuileder ();
            var result = npoiHelper.BuildEmployeesTabel (filePath);

            List<Employee> emps = new List<Employee> ();

            //check paymentType 
            //  string paymentTypeName = result.Rows[0].ItemArray[1].ToString ();

            var empList = await _context.Employees.Include (x => x.EmployeeBank).ToListAsync ();
            var dep = _context.Departments.SingleOrDefault (x => x.Name == "1-غير مسجل");

            foreach (DataRow row in result.Rows) {
                bool propertyChanged = false;
                //get Employee
                string empNationalId = row.ItemArray[0].ToString ();

                bool validate = empNationalId.ValidateNtionalId ();
                if (!validate) {
                    throw new Exception ("رقم البطاقه غير صحيح من فضلك تأكد من رقم البطاقه ");
                }
                Employee emp = empList
                    .SingleOrDefault (x => x.NationalId == empNationalId);

                if (emp == null) {
                    var tempEmp =  InserEmployeeData (row, dep);
                    _context.Employees.Add (tempEmp);
                    propertyChanged = true;
                } else {
                    emp = _context.Employees.SingleOrDefault (x => x.Id == emp.Id);
                    if ( UpdateEmployeeData (emp, row) != null) {

                        propertyChanged = true;
                    }
                }
                if (propertyChanged)
                    try {
                        await _context.SaveChangesAsync ();
                    }
                catch (Exception ex) {
                    throw new Exception (ex.InnerException.ToString ());
                }
            }

            return emps;

        }
        public async Task<IEnumerable<Employee>> SuggestEmployeeByName (string name, string paymentType, bool all) {
            string[] SplitedName = name.Split (" ");
            IEnumerable<Employee> emps;
            if (!all) {
                emps =await  _context.Employees
                    .Where (x => SplitedName.All (s => x.KnownAs.Contains (s)) && x.KnownAs.StartsWith (SplitedName[0]) && x.Deleted == false).ToListAsync();
            } else {
                emps =await  _context.Employees
                    .Where (x => SplitedName.Any (s => x.KnownAs.Contains (s)) && x.KnownAs.StartsWith (SplitedName[0]) && x.Deleted == false).ToListAsync();
            }
            return emps;
        }
        private Employee InserEmployeeData (DataRow row, Department dep) {
            //
            string payment = row.ItemArray[1].ToString ();
            var emp = new Employee () {
                Name = row.ItemArray[5].ToString (),
                KnownAs = row.ItemArray[5].ToString ().ToNormalizedString (),
                ATMOption = PaymentTypeConst.ATM,
                BankOption = PaymentTypeConst.ATM,
                NationalId = row.ItemArray[0].ToString (),
                Grade = row.ItemArray[3].ToString (),
                DepartmentId = dep.Id,
                Section = row.ItemArray[2].ToString (),
                Collage = row.ItemArray[2].ToString (),
                Gender = row.ItemArray[0].ToString ().CheckGenderByNationalId (),
                DOB = row.ItemArray[0].ToString ().GetDOBFromNationalId (),
                Code = row.ItemArray[4].ToString (),
                Deleted = false,
            };
            if (payment == "2-اخرى بطاقات حكومية") {
                emp.HasATM = true;

            } else if (payment == "3-مرتب تحويلات بنكية") {

                emp.HasBank = true;
                emp.EmployeeBank = new EmployeeBank ();
                emp.BankOption = PaymentTypeConst.Bank;
                emp.EmployeeBank.BankCode = row.ItemArray[6].ToString ();
                emp.EmployeeBank.BankName = row.ItemArray[7].ToString ();
                emp.EmployeeBank.BranchCode = row.ItemArray[8].ToString ();
                emp.EmployeeBank.BranchName = row.ItemArray[9].ToString ();

            }

            return emp;
        }
        private Employee UpdateEmployeeData (Employee emp, DataRow row) {
            string payment = row.ItemArray[1].ToString ();
            bool propertyChanged = false;
         
            if (payment == "2-اخرى بطاقات حكومية") {
                if (string.IsNullOrEmpty (emp.ATMOption)) {
                    emp.ATMOption = PaymentTypeConst.ATM;                 
                    propertyChanged = true;
                }
                if (string.IsNullOrEmpty (emp.ATMOption)) {
                    emp.ATMOption = PaymentTypeConst.ATM;                
                    propertyChanged = true;
                }
                if (emp.HasATM == false) { emp.HasATM = true; propertyChanged = true; }
            } else if (payment == "3-مرتب تحويلات بنكية") {
                if (emp.EmployeeBank == null) {
                    emp.EmployeeBank = new EmployeeBank ();
                }
                if (emp.EmployeeBank.BankCode != row.ItemArray[6].ToString ().ToString ()) {
                    emp.EmployeeBank.BankCode = row.ItemArray[6].ToString ();
                    propertyChanged = true;
                }
                if (emp.EmployeeBank.BankName != row.ItemArray[7].ToString ().ToString ()) {
                    emp.EmployeeBank.BankName = row.ItemArray[7].ToString ();
                    propertyChanged = true;
                }
                if (emp.EmployeeBank.BranchCode != row.ItemArray[8].ToString ().ToString ()) {
                    emp.EmployeeBank.BranchCode = row.ItemArray[8].ToString ();
                    propertyChanged = true;
                }
                if (emp.EmployeeBank.BranchName != row.ItemArray[9].ToString ().ToString ()) {
                    emp.EmployeeBank.BranchName = row.ItemArray[9].ToString ();
                    propertyChanged = true;
                }
                if (emp.BankOption == PaymentTypeConst.ATM || string.IsNullOrEmpty (emp.BankOption)) {
                    emp.BankOption = PaymentTypeConst.Bank;
                    propertyChanged = true;
                }

                if (emp.HasBank == false) { emp.HasBank = true; propertyChanged = true; }
            }
            if (emp.Name != row.ItemArray[5].ToString ()) {
                emp.Name = row.ItemArray[5].ToString ();
                propertyChanged = true;
            }
            if (emp.NationalId != row.ItemArray[0].ToString ()) {
                emp.NationalId = row.ItemArray[0].ToString ();
                propertyChanged = true;
            }
            if (emp.Grade != row.ItemArray[3].ToString ()) {
                emp.Grade = row.ItemArray[3].ToString ();
                propertyChanged = true;
            }
            if (emp.Section != row.ItemArray[2].ToString ()) {
                emp.Section = row.ItemArray[2].ToString ();
                propertyChanged = true;
            }
            if (emp.Collage != row.ItemArray[2].ToString ()) {
                emp.Collage = row.ItemArray[2].ToString ();
                propertyChanged = true;
            }
            if (emp.Gender != row.ItemArray[0].ToString ().CheckGenderByNationalId ()) {
                emp.Gender = row.ItemArray[0].ToString ().CheckGenderByNationalId ();
                propertyChanged = true;
            }
            if (emp.DOB != row.ItemArray[0].ToString ().GetDOBFromNationalId ()) {
                emp.DOB = row.ItemArray[0].ToString ().GetDOBFromNationalId ();
                propertyChanged = true;
            }
            if (emp.Code != row.ItemArray[4].ToString ().ToString ()) {
                emp.Code = row.ItemArray[4].ToString ();
                propertyChanged = true;
            }

            if (propertyChanged)
                return emp;
            return null;
        }
        public  List<Employee> FiltetByDepartment (string filterBy, List<Employee> emps) {
            var result = emps.Where (x => x.Department.Name.Contains (filterBy));

            return result.ToList ();
        }

        Task<List<Employee>> IEmployeeRepository.FiltetByDepartment(string filterBy, List<Employee> emps)
        {
            throw new NotImplementedException();
        }
    }
}