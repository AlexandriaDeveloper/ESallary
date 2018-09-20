using System;
using API.Data.Interface;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace API.Data.Repository {
    public class UnitOfWork : IUnitOfWork {
        #region Variables

        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private bool _disposed;

        public IUserRepository _userRepository;
        public IValueRepository _valueRepository;
        private readonly IMapper _mapper;
        public IUserRepository UserRepository {
            get {
                return _userRepository = _userRepository ?? new UserRepository (_context, _mapper, _userManager, _signInManager);
            }
        }
        public IValueRepository ValueRepository {
            get {
                return _valueRepository = _valueRepository ?? new ValueRepository (_context);
            }
        }
        #endregion

        #region Constructor

        /// <param name="context"></param>
        public UnitOfWork (DataContext context, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager) {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;

        }

        #endregion

        /// <summary>
        /// İşlemlerin veritabanına kaydedilmesi için bu method tetikleniyor.
        /// </summary>
        /// <returns></returns>
        public int SaveChanges () {
            using (var transaction = _context.Database.BeginTransaction ()) {
                try {
                    //Context boş ise hata fırlatıyoruz
                    if (_context == null) {
                        throw new ArgumentException ("Context is null");
                    }
                    //Save changes metodundan dönen int result ı yakalayarak geri dönüyoruz.
                    int result = _context.SaveChanges ();

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