//using Microsoft.Office.Interop.Excel;
using System;
using System.Web;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SSG = SpreadsheetGear  ;

namespace ZurichNA.AIS.ARMIS.Interface
{
	static class FastExportingMethod
	{

        //public static void ExportToExcel(DataSet dataSet, string outputPath)
        //{
        //	// Create the Excel Application object
        //	ApplicationClass excelApp = new ApplicationClass();

        //	// Create a new Excel Workbook
        //	Workbook excelWorkbook = excelApp.Workbooks.Add(Type.Missing);

        //	int sheetIndex = 0;

        //	// Copy each DataTable
        //	foreach (System.Data.DataTable dt in dataSet.Tables)
        //	{

        //		// Copy the DataTable to an object array
        //		object[,] rawData = new object[dt.Rows.Count + 1, dt.Columns.Count];

        //		// Copy the column names to the first row of the object array
        //		for (int col = 0; col < dt.Columns.Count; col++)
        //		{
        //			rawData[0, col] = dt.Columns[col].ColumnName;
        //		}

        //		// Copy the values to the object array
        //		for (int col = 0; col < dt.Columns.Count; col++)
        //		{
        //			for (int row = 0; row < dt.Rows.Count; row++)
        //			{
        //				rawData[row + 1, col] = dt.Rows[row].ItemArray[col];
        //			}
        //		}

        //		// Calculate the final column letter
        //		string finalColLetter = string.Empty;
        //		string colCharset = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        //		int colCharsetLen = colCharset.Length;

        //		if (dt.Columns.Count > colCharsetLen) {
        //			finalColLetter = colCharset.Substring(
        //				(dt.Columns.Count - 1) / colCharsetLen - 1, 1);
        //		}

        //		finalColLetter += colCharset.Substring(
        //				(dt.Columns.Count - 1) % colCharsetLen, 1);

        //		// Create a new Sheet
        //		Worksheet excelSheet = (Worksheet) excelWorkbook.Sheets.Add(
        //			excelWorkbook.Sheets.get_Item(++sheetIndex),
        //			Type.Missing, 1, XlSheetType.xlWorksheet);

        //		excelSheet.Name = dt.TableName;

        //		// Fast data export to Excel
        //		string excelRange = string.Format("A1:{0}{1}",
        //			finalColLetter, dt.Rows.Count + 1);

        //		excelSheet.get_Range(excelRange, Type.Missing).Value2 = rawData;

        //		// Mark the first row as BOLD
        //		((Range) excelSheet.Rows[1, Type.Missing]).Font.Bold = true;

        //	}

        //	// Save and Close the Workbook
        //	excelWorkbook.SaveAs(outputPath, XlFileFormat.xlWorkbookNormal, Type.Missing,
        //		Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlExclusive,
        //		Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        //	excelWorkbook.Close(true, Type.Missing, Type.Missing);
        //	excelWorkbook = null;

        //	// Release the Application object
        //	excelApp.Quit();
        //	excelApp = null;

        //	// Collect the unreferenced objects
        //	GC.Collect();
        //	GC.WaitForPendingFinalizers();

        //}

        public static String getColumnNamefromIndex(int column)
        {
            column--;
            String col = Convert.ToString((char)('A' + (column % 26)));
            while (column >= 26)
            {
                column = (column / 26) - 1;
                col = Convert.ToString((char)('A' + (column % 26))) + col;
            }
            return col;
        }

        public static void FormatHeaderTypeCell(SSG.IRange cell, SSG.HAlign align = SSG.HAlign.Center, bool isBold = true)
        {
            cell.Font.Bold = isBold;
            cell.HorizontalAlignment = align;

        }
        public static void ExportToExcelusingSpreadsheetGear(DataSet dataSet, string outputPath)
        {
            try
            {
                string ExcelFilePath = String.Empty;
                ExcelFilePath = outputPath;
                SSG.IWorkbook workbook = SSG.Factory.GetWorkbook();
                int sheetIndex = 0;
                int cellIndex = 0;

                foreach (System.Data.DataTable dataTable in dataSet.Tables)
                {

                    workbook.Worksheets.Add();
                    sheetIndex++;
                    SSG.IWorksheet worksheetARMIS = workbook.Worksheets["Sheet" + sheetIndex.ToString()];
                    worksheetARMIS.Name = dataTable.TableName;

                    // Get the worksheet cells reference. 
                    SSG.IRange cellsARMIS = worksheetARMIS.Cells;
                    cellIndex = 1;

                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        String columname = string.Empty;
                        int columnindexvalue = i + 1;
                        columname = getColumnNamefromIndex(columnindexvalue);
                        cellsARMIS[columname + cellIndex.ToString()].Value = dataTable.Columns[i].ColumnName;
                        FormatHeaderTypeCell(cellsARMIS[columname + cellIndex.ToString()]);
                    }
                    cellIndex = cellIndex + 1;

                    //rows
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        // to do: format datetime values before printing
                        for (int j = 0; j < dataTable.Columns.Count; j++)
                        {

                            String columname = string.Empty;
                            int columnindexvalue = j + 1;
                            columname = getColumnNamefromIndex(columnindexvalue);
                            cellsARMIS[columname + cellIndex.ToString()].Value = dataTable.Rows[i][j];

                        }

                        cellIndex = cellIndex + 1;
                    }

                    worksheetARMIS.UsedRange.Columns.AutoFit();

                }

                //if (sheetIndex >0)
                //{
                //    //Delete original blank empty sheet from report workbook.
                //    workbook.ActiveWorksheet.Delete();
                //}

                //if (sheetIndex == 0)
                //{
                //    workbook.Worksheets.Add();
                //}

                if (ExcelFilePath != null && ExcelFilePath != "")
                {

                    try
                    {
                        workbook.Worksheets[0].Select();
                        string strFileName = ExcelFilePath;
                        string strFileNameDecd = HttpUtility.HtmlDecode(HttpUtility.HtmlEncode(strFileName));
                        workbook.SaveAs(strFileNameDecd, SSG.FileFormat.OpenXMLWorkbook);
                        workbook.Close();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Excel file could not be saved! Check filepath.\n"
                            + ex.Message);
                    }


                }
                else
                {
                    throw new Exception("File path is not given!\n");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("ARMIS Excel file generation: \n" + ex.Message);
            }
        }

    }
}
