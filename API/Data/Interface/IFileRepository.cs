using API.Models;

namespace API.Data.Interface {
    public interface IFileRepository : IRepository<File> {
        bool CheckFile55(string file55) ;
        bool CheckFile224(string file55) ;
    }

    public interface IFileDetailRepository : IRepository<FileDetail> {

    }
    public interface IDailyRepository : IRepository<Daily> {

    }
    public interface IFileTypeRepository : IRepository<FileType> {

    }
}