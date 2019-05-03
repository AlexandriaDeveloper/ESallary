using System;
using System.Threading.Tasks;
using API.Data.Interface;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace API.Data.Repository {
    public class UnitOfWork : IUnitOfWork {
        #region Variables

        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config;
        private bool _disposed;

        public IUserRepository _userRepository;
        public IEmployeePaymentTypesRepository _employeePaymentRepository;
        public IEmployeeRepository _employeeRepository;
        public IEmployeeFinincialDataRepository _employeeFinincialDataRepository;
        public IBankRepository _bankRepository;
        public IBankBranchRepository _bankbranchRepository;
        public IDepartmentRepository _departmentRepository;
        public IOrderRepository _orderRepository;
        public IPostRepository _postRepository;
        public IFinincialAccountRepository _finincialAccountRepository;
        public IFileRepository _fileRepository;
        public IFileDetailRepository _fileDetailRepository;
        public IDailyRepository _dailyRepository;
        public IFileTypeRepository _fileTypeRepository;

        private readonly IMapper _mapper;

        public DataContext context {
            get {
                return _context;
            }
        }
        public IUserRepository UserRepository {
            get {
                return _userRepository = _userRepository ?? new UserRepository (_context, _config, _mapper, _userManager, _signInManager);
            }
        }

        public IEmployeeRepository EmployeeRepository {
            get {
                return _employeeRepository = _employeeRepository ?? new EmployeeRepository (_context);
            }
        }
        public IEmployeeFinincialDataRepository EmployeeFinincialDataRepository {
            get {
                return _employeeFinincialDataRepository = _employeeFinincialDataRepository ?? new EmployeeFinincialDataRepository (_context);
            }
        }
        public IBankRepository BankRepository {
            get {
                return _bankRepository = _bankRepository ?? new BankRepository (_context);
            }
        }
        public IBankBranchRepository BankBranchRepository {
            get {
                return _bankbranchRepository = _bankbranchRepository ?? new BankBranchRepository (_context);
            }
        }
        public IOrderRepository OrderRepository {
            get {
                return _orderRepository = _orderRepository ?? new OrderRepository (_context);
            }
        }
        public IDepartmentRepository DepartmentRepository {
            get {
                return _departmentRepository = _departmentRepository ?? new DepartmentRepository (_context);
            }
        }
        public IPostRepository PostRepository {
            get {
                return _postRepository = _postRepository ?? new PostRepository (_context);
            }
        }
        public IFinincialAccountRepository FinincialAccountRepository {
            get {
                return _finincialAccountRepository = _finincialAccountRepository ?? new FinincialAccountRepository (_context);
            }
        }
        public IFileRepository FileRepository {
            get {
                return _fileRepository = _fileRepository ?? new FileRepository (_context);
            }
        }
        public IFileDetailRepository FileDetailRepository {
            get {
                return _fileDetailRepository = _fileDetailRepository ?? new FileDetailRepository (_context);
            }
        }
        public IDailyRepository DailyRepository {
            get {
                return _dailyRepository = _dailyRepository ?? new DailyRepository (_context);
            }
        }
        public IFileTypeRepository FileTypeRepository {
            get {
                return _fileTypeRepository = _fileTypeRepository ?? new FileTypeRepository (_context);
            }
        }
        #endregion

        #region Constructor

        /// <param name="context"></param>
        public UnitOfWork (DataContext context, IConfiguration config, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager) {

            _context = context;
            _mapper = mapper;
            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;

        }

        #endregion

        /// <summary>
        /// İşlemlerin veritabanına kaydedilmesi için bu method tetikleniyor.
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync () {
            using (var transaction = _context.Database.BeginTransaction ()) {
                try {
                    //Context boş ise hata fırlatıyoruz
                    if (_context == null) {
                        throw new ArgumentException ("Context is null");
                    }
                    //Save changes metodundan dönen int result ı yakalayarak geri dönüyoruz.
                    int result = await _context.SaveChangesAsync ();

                    //Sorun yok ise kuyruktaki tüm işlemleri commit ederek bitiriyoruz.
                    transaction.Commit ();
                    return result;
                } catch (Exception ex) {
                    //Hata ile karşılaşılır ise işlemler geri alınıyor 
                    transaction.Rollback ();
                    throw new Exception ("Error on save changes ", ex);
                }
            }
        }
        #region DisposingSection

        /// <summary>
        /// Context ile işimiz bittiğinde dispose edilmesini sağlıyoruz
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose (bool disposing) {
            if (!this._disposed) {
                if (disposing) {
                    _context.Dispose ();
                }
            }
            this._disposed = true;
        }

        public void Dispose () {
            Dispose (true);
            GC.SuppressFinalize (this);
        }

        public IRepository<T> GetRepository<T> () where T : class {
            throw new NotImplementedException ();
        }

        #endregion

    }
}