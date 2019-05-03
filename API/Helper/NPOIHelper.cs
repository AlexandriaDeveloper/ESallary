using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace API.Helper {
    public class NPOIHelper {
        string file;
        private IWorkbook workbook;
        private IFormulaEvaluator helper = null;
        public NPOIHelper (string _file) {
            this.file = _file;
            using (FileStream fileStream = new FileStream (this.file, FileMode.Open, FileAccess.Read)) {
                workbook = WorkbookFactory.Create (fileStream);
                this.helper = this.workbook.GetCreationHelper ().CreateFormulaEvaluator ();
                this.helper.EvaluateAll ();

            }
            if (this.workbook.GetType () == typeof (HSSFWorkbook)) {
                this.helper = new HSSFFormulaEvaluator (this.workbook);

            } else {
                this.helper = new XSSFFormulaEvaluator (this.workbook);
            }
        }
        public string[] GetFileSheets () {
            int NumberOfSheets = SheetsNumber;
            string[] sheetsName = new string[NumberOfSheets];
            for (int i = 0; i < NumberOfSheets; i++) {
                string sheetName = workbook.GetSheetName (i);
                sheetsName[i] = sheetName;
            }
            return sheetsName;
        }

        public DataTable ReadSheet (string SheetName = "Sheet1") {
            ISheet sheet = workbook.GetSheet (SheetName);
            if (sheet != null)
                return ReadSheetFunction (sheet);
            return null;
        }
        public DataTable ReadSheet (int SheetIndex = 0) {
            ISheet sheet = workbook.GetSheetAt (SheetIndex);
            if (sheet != null)
                return ReadSheetFunction (sheet);
            return null;
        }
        public int GetFirstRowIndex (string SheetName) {
            ISheet sheet = workbook.GetSheet (SheetName);
            return sheet.FirstRowNum;
        }
        private DataTable ReadSheetFunction (ISheet Sheet) {
            if (Sheet != null) {

                return SheetToDataTable (sheet: Sheet);
            }
            return null;

        }

        private DataTable SheetToDataTable (ISheet sheet) {
            DataTable dt = new DataTable (sheet.SheetName);
            string[] bans = { "الأجمالى", "الاجمالى", "الإجمالى", "total", "Total", "", " " };
            // write header row
            IRow headerRow = sheet.GetRow (sheet.FirstRowNum);
            for (int i = 0; i < headerRow.LastCellNum; i++) {
                ICell headerCell = headerRow.GetCell (i);

                // int colIndex = headerCell.ColumnIndex;
                if (headerCell == null) {
                    headerCell = headerRow.CreateCell (i);
                    headerCell.SetCellValue ("Blank_" + i);
                }
                dt.Columns.Add (headerCell.ToString ());
            }

            // write the rest
            int rowIndex = 0;
            foreach (IRow row in sheet) {
                // skip header row 
                if (rowIndex++ == 0) continue;
                DataRow dataRow = dt.NewRow ();

                for (int i = 0; i < row.LastCellNum; i++) {

                    var currentCell = row.GetCell (i);
                    if (currentCell == null) {
                        currentCell = row.CreateCell (i);
                    }
                    if (currentCell.CellType == CellType.String) {
                        currentCell.SetCellValue (currentCell.StringCellValue.Trim ());
                        if (!bans.Contains (currentCell.StringCellValue))
                            dataRow[i] = currentCell.StringCellValue;
                    }
                    if (currentCell.CellType == CellType.Numeric)
                        dataRow[i] = currentCell.NumericCellValue.ToString ();
                    if (currentCell.CellType == CellType.Formula) {
                        dataRow[i] = this.helper.EvaluateInCell (currentCell);

                    }
                }

                dt.Rows.Add (dataRow);
            }
            return dt;
        }

        private string CheckCellValue (ICell cell) {
            string value = "";
            switch (cell.CellType) {
                case CellType.String:
                    value = cell.StringCellValue.ToString ();
                    break;
                case CellType.Numeric:
                    value = cell.NumericCellValue.ToString ();
                    break;
            }
            return value;
        }
        private List<ICell> GetSheetRow (IRow Row) {
            List<ICell> cells = Row.Cells;
            return cells;
        }

        public int SheetsNumber { get { return workbook != null ? workbook.NumberOfSheets : 0; } }
    }
}