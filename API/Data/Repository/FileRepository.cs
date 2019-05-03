using System.Linq;
using API.Data.Interface;
using API.Models;

namespace API.Data.Repository {
    public class FileRepository : Repository<File>, IFileRepository {
        private readonly DataContext context;
        public FileRepository (DataContext context) : base (context) {
            this.context = context;
        }

        public bool CheckFile55 (string file55) {
            return context.Files.Any (x => x.FileNum55 == file55);
        }
        
        public bool CheckFile224 (string file224) {
            return context.Files.Any (x => x.FileNum224 == file224);
        }
    }
    public class FileDetailRepository : Repository<FileDetail>, IFileDetailRepository {
        public FileDetailRepository (DataContext context) : base (context) { }
    }
    public class DailyRepository : Repository<Daily>, IDailyRepository {
        public DailyRepository (DataContext context) : base (context) { }
    }
       public class FileTypeRepository : Repository<FileType>, IFileTypeRepository {
        public FileTypeRepository (DataContext context) : base (context) { }
    }
}