using System.Data;
using API.Helper;

namespace API.Service {
    public class ExcelBuileder {


     
        public DataTable BuildEmployeesTabel (string file) {
            NPOIHelper npoi = new NPOIHelper (file);
            var Sheet = npoi.ReadSheet ("Sheet1");
            string fileType=Sheet.Rows[1].ItemArray[1].ToString ();
            if (fileType.StartsWith ("1") ||fileType.StartsWith ("2"))
                Sheet = BuildAtmColumn (Sheet);
            else if (fileType.StartsWith ("3") ||fileType.StartsWith ("4")) {
                Sheet = BuildBankColumn (Sheet);
            }
            else{
                return null;
            }

            return Sheet;
        }
        public DataTable BuildAtmColumn (DataTable dt) {

            dt.Columns[0].ColumnName = "NationalId";
            dt.Columns[1].ColumnName = "PaymentType";
            dt.Columns[2].ColumnName = "Section";
            dt.Columns[3].ColumnName = "Department";
            dt.Columns[4].ColumnName = "Code";
            dt.Columns[5].ColumnName = "Name";
            dt.Columns[6].ColumnName = "Sallary";
            return dt;
        }
        public DataTable BuildBankColumn (DataTable dt) {

            dt.Columns[0].ColumnName = "NationalId";
            dt.Columns[1].ColumnName = "PaymentType";
            dt.Columns[2].ColumnName = "Section";
            dt.Columns[3].ColumnName = "Department";
            dt.Columns[4].ColumnName = "Code";
            dt.Columns[5].ColumnName = "Name";
            dt.Columns[6].ColumnName = "BankCode";
            dt.Columns[7].ColumnName = "BankName";
            dt.Columns[8].ColumnName = "BranchCode";
            dt.Columns[9].ColumnName = "BranchName";
            dt.Columns[10].ColumnName = "Sallary";
            return dt;
        }
    }
}