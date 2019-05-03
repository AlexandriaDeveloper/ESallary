using API.Data.Interface;
using API.Models;

namespace API.Data.Repository {
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository {
        public DepartmentRepository (DataContext context) : base (context) { }
    }
}