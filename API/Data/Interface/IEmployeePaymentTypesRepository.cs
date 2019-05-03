using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using API.DTOS;
using API.Models;

namespace API.Data.Interface {
    public interface IEmployeePaymentTypesRepository : IRepository<Employee> {
        Task<List<Employee>> AddEmployeesFileAsync (EmployeesFileDto empData);
        //      Task<List<string>> GetDistinict (Expression<Func<Employee, string>> query) ;
    }
}