namespace API.Data.Interface {
    public interface IUnitOfWork {

        /// <summary>
        /// Gerektiğinde repository instance oluşturmak için kullanılacak.
        /// </summary>
        /// <typeparam name="T">Hangi entity miz için repository oluşmasını istiyorsak o sınıfı gönderiyoruz</typeparam>
        /// <returns></returns>
        IRepository<T> GetRepository<T> () where T : class;
        IUserRepository  UserRepository { get; }
        IValueRepository  ValueRepository { get; }
        /// <summary>
        /// Veritabanında işlemlerin yapılması emrini veren kısım olacak
        /// Repository içerisinde kuyruğa aldığımız tüm işlemler bu metot çalıştırıldığı anda sırası ile veritabanında değişikliğe uğrayacak
        /// </summary>
        /// <returns></returns>
        int SaveChanges ();
    }
}