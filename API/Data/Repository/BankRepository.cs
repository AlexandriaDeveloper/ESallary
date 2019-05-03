using API.Data.Interface;
using API.Models;

namespace API.Data.Repository
{
    public class BankRepository : Repository<Bank>, IBankRepository
    {
        public BankRepository(DataContext context) : base(context)
        {
        }
    }

    public class BankBranchRepository : Repository<BankBranch>, IBankBranchRepository
    {
        public BankBranchRepository(DataContext context) : base(context)
        {
        }
    }
        public class OrderRepository : Repository<EmployeeOrder>, IOrderRepository
    {
        public OrderRepository(DataContext context) : base(context)
        {
        }
    }
         public class PostRepository : Repository<EmployeePost>, IPostRepository
    {
        public PostRepository(DataContext context) : base(context)
        {
        }
    }
}