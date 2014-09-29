using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace IMEIScanner
{
    /// <summary>
    /// Export to excel helper class
    /// </summary>
    
    internal class CreateExcelDoc
    {
        private Application _app;
        private Range _workSheetRange;
        private Workbook _workbook;
        private Worksheet _worksheet;

        /// <summary>
        /// Initializes a new instance of the "CreateExcelDoc" form.
        /// </summary>
        
        public CreateExcelDoc()
        {
            CreateDoc();
        }

        /// <summary>
        /// Creates the doc.
        /// </summary>
        public CreateExcelDoc(int i)
        {
            OpenExcelDoc();
        }
        public void OpenExcelDoc()
        {
            try
            {
                _app =(Application) System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");
            }catch(Exception ex)
            {
                _app = new Application { Visible = true };
            }

            if (!System.IO.File.Exists(@"D:\DailyReportIMEI.xlsx"))
            {
                _app = new Application { Visible = true };
        //        _workbook = _app.Workbooks.Add(1);
                _app.Save(@"D:\DailyReportIMEI.xlsx");
            }
//            excelApp.ActiveWorkbook.Close();
            const string workbookPath = (@"D:\DailyReportIMEI.xlsx");
            Workbook _workbook = _app.Workbooks.Open(workbookPath,
                    0, false, 5, "", "", false, XlPlatform.xlWindows, "",
                    true, false, 0, true, false, false);
            _worksheet = (Worksheet)_workbook.Sheets[1];
            _worksheet.Name = "Report " + DateTime.Today.ToShortDateString();
        }
        public void CreateDoc()
        {
            
            try
            {
                _app = new Application {Visible = true};
                _workbook = _app.Workbooks.Add(1);
                _worksheet = (Worksheet) _workbook.Sheets[1];
                _worksheet.Name = DateTime.Today.ToShortDateString().Replace("/","_");
                _worksheet.Cells.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            }
            catch (Exception ex)
            {
          //      LogError.Log("Create Excel File", ex.Message);
          //      LogError.Log("Create Excel File", ex.StackTrace);
            }
        }
        
        public void CreateHeaders(int row, int col, string htext, string cell1,
                                  string cell2, int mergeColumns, string b, bool font, int size, string
                                                                                                     fcolor)
        {
            _worksheet.Cells[row, col] = htext;
            _workSheetRange = _worksheet.get_Range(cell1, cell2);

            _workSheetRange.Merge(mergeColumns);
            switch (b)
            {
                case "YELLOW":
                    _workSheetRange.Interior.Color = Color.Yellow.ToArgb();
                    break;
                case "GRAY":
                    _workSheetRange.Interior.Color = Color.Gray.ToArgb();
                    break;
                case "GAINSBORO":
                    _workSheetRange.Interior.Color =
                        Color.Gainsboro.ToArgb();
                    break;
                case "Turquoise":
                    _workSheetRange.Interior.Color =
                        Color.Turquoise.ToArgb();
                    break;
                case "PeachPuff":
                    _workSheetRange.Interior.Color =
                        Color.PeachPuff.ToArgb();
                    break;
            }

            _workSheetRange.Borders.Color = Color.Black.ToArgb();
            _workSheetRange.Font.Bold = font;
            _workSheetRange.ColumnWidth = size;
            _workSheetRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            _workSheetRange.Font.Color = fcolor.Equals(String.Empty) ? Color.White.ToArgb() : Color.Black.ToArgb();
        }
        
        public void AddData(int row, int col, string data,
                            string cell1, string cell2, string format)
        {
            _workSheetRange = _worksheet.get_Range(cell1, cell2);
            _workSheetRange.Borders.Color = Color.Black.ToArgb();
            _workSheetRange.NumberFormat = format;
            _worksheet.Cells[row, col] = data;
        }
    }
}