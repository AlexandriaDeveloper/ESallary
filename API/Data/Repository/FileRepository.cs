using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Data.Interface;
using API.Helper;
using API.Models;
using API.Service;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace API.Data.Repository {

    public interface IFileRepository : IRepository<Models.File> {
        bool CheckFile55 (string file55);
        bool CheckFile224 (string file55);

    }
    public class FileRepository : Repository<Models.File>, IFileRepository {
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

    public interface IFileDetailRepository : IRepository<FileDetail> {
        System.Threading.Tasks.Task<string> DownloadFile (List<FileDetail> Employees, string fileType,string fileNum55,string FileName,string DataEntryName);

    }
    public class FileDetailRepository : Repository<FileDetail>, IFileDetailRepository {
        public FileDetailRepository (DataContext context) : base (context) { }

        public async Task<string> DownloadFile (List<FileDetail> Employees, string fileType,string fileNum55,string FileName,string DataEntryName) {
            // get file
        var    NPOI = new NPOIHelper();
       return  NPOI.WritePrintedXLSFile(Employees,fileType,fileNum55,FileName,DataEntryName);
        }

    }

    public class DailyRepository : Repository<Daily>, IDailyRepository {
        public DailyRepository (DataContext context) : base (context) { }
    }
    public class FileTypeRepository : Repository<FileType>, IFileTypeRepository {
        public FileTypeRepository (DataContext context) : base (context) { }

    }
}