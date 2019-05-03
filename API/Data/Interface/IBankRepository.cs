using API.Models;

namespace API.Data.Interface {
    public interface IBankRepository : IRepository<Bank> {

    }
    public interface IBankBranchRepository : IRepository<BankBranch> {

    }
    public interface IOrderRepository : IRepository<EmployeeOrder> {

    }
    public interface IPostRepository : IRepository<EmployeePost> {

    }
}