using System.Collections;
using System.Threading.Tasks;
using API.Data.Interface;
using API.Models;

namespace API.Data.Repository
{
    public class ValueRepository : Repository<Value>, IValueRepository
    {
        public ValueRepository(DataContext context) : base(context)
        {
        }
    }
}