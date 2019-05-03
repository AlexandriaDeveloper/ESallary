using API.Data.Interface;
using API.Models;

namespace API.Data.Repository
{
    public class FinincialAccountRepository : Repository<Account> , IFinincialAccountRepository
    {
        public FinincialAccountRepository(DataContext context) : base(context)
        {
        }
    }
}