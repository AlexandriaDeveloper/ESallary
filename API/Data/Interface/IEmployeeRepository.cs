using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using API.DTOS;
using API.Helper;
using API.Models;

namespace API.Data.Interface {
    public interface IEmployeeRepository : IRepository<Employee> {
        Task<bool> CheckEmployeeExistById (string nationalId);
        Task<List<string>> GetCollageList ();
        Task<List<string>> GetGradesList ();
        Task<List<Department>> GetDepartmentsList ();
        Task<List<Employee>> AddEmployeesFileAsync (EmployeesFileDto empData);
        Task<EmployeeFinincialData> GetEmployeeFinance (int id);
        Task<List<Employee>> FiltetByDepartment (string filterBy, List<Employee> emps);
    }
}