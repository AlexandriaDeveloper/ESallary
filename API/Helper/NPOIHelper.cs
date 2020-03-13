using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
namespace API.Helper {
    public class NPOIHelper {
        string file;
        private IWorkbook workbook;
        private IFormulaEvaluator helper = null;
        public NPOIHelper () {

        }
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
        public string WritePrintedXLSFile (List<FileDetail> Employees, string fileType, string fileNum55, string FileName, string DataEntryName, string SheetName = "Sheet1") {
            string filePath = "";
            string filePaymentType = fileType;

            string temp = "";
            if (fileType == PaymentTypeConst.ATM || fileType == PaymentTypeConst.Bank) {
                temp = Path.GetTempPath () + "PrintTemplate.xls";
                bool fileExist = System.IO.File.Exists ("SRC/PrintTemplate.xls");
                filePath = "SRC/PrintTemplate.xls";
            }

            if (fileType == PaymentTypeConst.PaymentOrder) {
                temp = Path.GetTempPath () + "PrintOrderTemplate.xls";
                bool fileExist = System.IO.File.Exists ("SRC/PrintOrderTemplate.xls");
                filePath = "SRC/PrintOrderTemplate.xls";
            }
            if (fileType == PaymentTypeConst.PersonalPost) {
                temp = Path.GetTempPath () + "PrintPersonalPostTemplate.xls";
                bool fileExist = System.IO.File.Exists ("SRC/PrintPersonalPostTemplate.xls");
                filePath = "SRC/PrintPersonalPostTemplate.xls";
            }

            if (fileType == PaymentTypeConst.InternalPost) {
                temp = Path.GetTempPath () + "InternalPostTemplate.xls";
                bool fileExist = System.IO.File.Exists ("SRC/InternalPostTemplate.xls");
                filePath = "SRC/InternalPostTemplate.xls";
            }
            if (System.IO.File.Exists (temp)) {
                System.IO.File.Delete (temp);
            }
            System.IO.File.Copy (filePath, temp);

            HSSFWorkbook wb = null;

            using (FileStream Stream = new FileStream (temp, FileMode.Open, FileAccess.ReadWrite)) {
                wb = new HSSFWorkbook (Stream);

                Stream.Close ();
            }
            ISheet sheet = wb.GetSheet ("Sheet1");

            sheet.AutoSizeColumn (24);
            //styling

            ICellStyle boldStyle = BodyStyle (wb);

            var titleStyle = this.TitleStyle (wb);
            CreateCell (sheet.GetRow (0), 2, fileNum55, titleStyle);

            CreateCell (sheet.GetRow (1), 2, FileName, titleStyle);

            CreateCell (sheet.GetRow (2), 2, filePaymentType, titleStyle);

            CreateCell (sheet.GetRow (3), 2, Employees.Sum (x => x.Net), titleStyle, CellType.String);

            CreateCell (sheet.GetRow (4), 2, DataEntryName, titleStyle);

            int row = 6;
            foreach (var emp in Employees) {
                IRow currenRow = sheet.CreateRow (row);
                if (fileType == PaymentTypeConst.ATM || fileType == PaymentTypeConst.Bank)
                    BuildEmployeeAtmOrBank (currenRow, row, emp, boldStyle);
                if (fileType == PaymentTypeConst.PaymentOrder) {

                    BuildEmployeeOrder (currenRow, row, emp, boldStyle);
                }      
                if (fileType == PaymentTypeConst.PersonalPost) {

                    BuildEmployeePost (currenRow, row, emp, boldStyle);
                }
                if (fileType == PaymentTypeConst.InternalPost) {

                    BuildEmployeeIntenalPost (currenRow, row, emp, boldStyle);
                }
                row++;
            }

            using (FileStream Stream1 = new FileStream (temp, FileMode.Create, FileAccess.ReadWrite)) {
                wb.Write (Stream1);
                wb.Close ();
                Stream1.Close ();
            }
            return temp;
        }

        private void BuildEmployeeAtmOrBank (IRow currenRow, int row, FileDetail emp, ICellStyle boldStyle) {

            currenRow.Height = 20 * 20;
            CreateCell (currenRow, 0, row - 5, boldStyle);

            CreateCell (currenRow, 1, emp.Employee.Section, boldStyle);

            CreateCell (currenRow, 2, emp.Employee.Grade, boldStyle);

            CreateCell (currenRow, 3, emp.Employee.Code, boldStyle);

            CreateCell (currenRow, 4, emp.Employee.Name, boldStyle);

            CreateCell (currenRow, 5, emp.Net, boldStyle, CellType.String);
        }
        private void BuildEmployeeOrder (IRow currenRow, int row, FileDetail emp, ICellStyle boldStyle) {

            currenRow.Height = 20 * 20;
            CreateCell (currenRow, 0, row - 5, boldStyle);

            CreateCell (currenRow, 1, emp.Employee.Section, boldStyle);

            CreateCell (currenRow, 2, emp.Employee.EmployeeOrder.BankBranch.Bank.BankName + "  فرع  " + emp.Employee.EmployeeOrder.BankBranch.BranchName, boldStyle);

            CreateCell (currenRow, 3, emp.Employee.EmployeeOrder.OrderAccountNumber, boldStyle);

            CreateCell (currenRow, 4, emp.Employee.Name, boldStyle);

            CreateCell (currenRow, 5, emp.Net, boldStyle, CellType.String);
        }

        private void BuildEmployeePost (IRow currenRow, int row, FileDetail emp, ICellStyle boldStyle) {

            currenRow.Height = 20 * 20;
            CreateCell (currenRow, 0, row - 5, boldStyle);

            CreateCell (currenRow, 1, emp.Employee.Section, boldStyle);


            CreateCell (currenRow, 2, emp.Employee.EmployeePost.PostTo, boldStyle);
            
            CreateCell (currenRow, 3, emp.Employee.NationalId, boldStyle);

            CreateCell (currenRow, 4, emp.EmployeeName, boldStyle);

            CreateCell (currenRow, 5, emp.Net, boldStyle, CellType.String);
        }
        private void BuildEmployeeIntenalPost (IRow currenRow, int row, FileDetail emp, ICellStyle boldStyle) {

            currenRow.Height = 20 * 20;
            CreateCell (currenRow, 0, row - 5, boldStyle);

            CreateCell (currenRow, 1, emp.EmployeeName, boldStyle);

            CreateCell (currenRow, 2, emp.Net, boldStyle, CellType.String);
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
        private ICellStyle TitleStyle (IWorkbook wb) {
            IFont boldFont = wb.CreateFont ();
            boldFont.FontHeightInPoints = 14;
            boldFont.Underline = FontUnderlineType.Single;

            ICellStyle boldStyle = wb.CreateCellStyle ();
            boldStyle.SetFont (boldFont);
            boldStyle.Alignment = HorizontalAlignment.Right;
            boldStyle.WrapText = true;

            return boldStyle;
        }

        private ICellStyle BodyStyle (IWorkbook wb) {
            IFont boldFont = wb.CreateFont ();
            boldFont.FontHeightInPoints = 14;

            ICellStyle boldStyle = wb.CreateCellStyle ();
            boldStyle.SetFont (boldFont);
            boldStyle.Alignment = HorizontalAlignment.Center;
            boldStyle.WrapText = false;
            boldStyle.BorderBottom = BorderStyle.Double;
            boldStyle.BorderRight = BorderStyle.Double;
            boldStyle.BorderLeft = BorderStyle.Double;

            return boldStyle;
        }
        private ICell CreateCell (IRow currenRow, int cellIndex, dynamic cellValue, ICellStyle style = null, CellType cellType = CellType.String) {
            ICell cell = currenRow.CreateCell (cellIndex);
            cell.SetCellType (cellType);
            if (cell.CellType != CellType.Numeric) { cell.SetCellValue ("\u200F" + cellValue); } else {
                cell.SetCellValue ((double) cellValue);
            };
            cell.CellStyle = style;
            return cell;
        }
        public int SheetsNumber { get { return workbook != null ? workbook.NumberOfSheets : 0; } }
    }
}