using System.Threading.Tasks;

namespace API.Data.Interface {
    public interface IUnitOfWork {

        /// <summary>
        /// Gerektiğinde repository instance oluşturmak için kullanılacak.
        /// </summary>
        /// <typeparam name="T">Hangi entity miz için repository oluşmasını istiyorsak o sınıfı gönderiyoruz</typeparam>
        /// <returns></returns>
        IRepository<T> GetRepository<T> () where T : class;
        IUserRepository UserRepository { get; }

        IEmployeeRepository EmployeeRepository { get; }
        IEmployeeFinincialDataRepository EmployeeFinincialDataRepository { get; }
        IBankRepository BankRepository { get; }
        IBankBranchRepository BankBranchRepository { get; }
        IOrderRepository OrderRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }
        IFinincialAccountRepository FinincialAccountRepository { get; }
        IFileRepository FileRepository { get; }
        IFileDetailRepository FileDetailRepository { get; }
        IDailyRepository DailyRepository { get; }
        IFileTypeRepository FileTypeRepository { get; }

        IPostRepository PostRepository { get; }
        //   DataContext context { get; }
        /// <summary>
        /// Veritabanında işlemlerin yapılması emrini veren kısım olacak
        /// Repository içerisinde kuyruğa aldığımız tüm işlemler bu metot çalıştırıldığı anda sırası ile veritabanında değişikliğe uğrayacak
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync ();
    }
}