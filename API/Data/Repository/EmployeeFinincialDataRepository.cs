using API.Data.Interface;
using API.Models;

namespace API.Data.Repository
{
    public class EmployeeFinincialDataRepository : Repository<EmployeeFinincialData>, IEmployeeFinincialDataRepository
    {
        public EmployeeFinincialDataRepository(DataContext context) : base(context)
        {
        }
    }
}