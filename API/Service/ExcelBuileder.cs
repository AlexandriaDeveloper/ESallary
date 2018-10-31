using System.Data;
using API.Helper;

namespace API.Service {
    public class ExcelBuileder {
    

        public DataTable BuildTabel (string file) {
            NPOIHelper npoi = new NPOIHelper(file);
            var colHeader = npoi.ReadSheet("Sheet1");
            
            return null;
        }
    }
}