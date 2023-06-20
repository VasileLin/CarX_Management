using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarX.Classes
{
    public class ExportData
    {
        public string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public void ExportToExcel(DataGridView dataGridView, string filePath)
        {
            // Se creeaza un nou fișier Excel
            SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook);

            // Se adauga o foaie de lucru în fișierul Excel
            WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());

            // Se adauga foaia de lucru la lista de foi din fișierul Excel
            Sheet sheet = new Sheet()
            {
                Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = "Raport"
            };
            sheets.Append(sheet);

            // Se obtin datele din DataGridView
            DataTable dataTable = new DataTable();

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                dataTable.Columns.Add(column.HeaderText);
            }

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                DataRow dataRow = dataTable.NewRow();
                for (int i = 0; i < dataGridView.Columns.Count - 2; i++)
                {
                    dataRow[i] = row.Cells[i].Value;
                }
                dataTable.Rows.Add(dataRow);
            }

            // Se exporta datele în fișierul Excel
            SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            // Se adauga header-ul în fișierul Excel
            Row headerRow = new Row();
            foreach (DataColumn column in dataTable.Columns)
            {
                Cell cell = new Cell();
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue(column.ColumnName);
                headerRow.AppendChild(cell);
            }
            sheetData.AppendChild(headerRow);

            // Se adauga rândurile de date în fișierul Excel
            foreach (DataRow row in dataTable.Rows)
            {
                Row dataRow = new Row();
                foreach (var item in row.ItemArray)
                {
                    Cell cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue(item.ToString());
                    dataRow.AppendChild(cell);
                }
                sheetData.AppendChild(dataRow);
            }
            spreadsheetDocument.Dispose();
            MessageBox.Show("Data has been successfully exported to Excel file on Desktop");
        }



        public void ExportToExcelReport(DataGridView dataGridView, string filePath)
        {
            // Se creeaza un nou fișier Excel
            SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook);

            // Se adauga o foaie de lucru în fișierul Excel
            WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());

            // Se adauga foaia de lucru la lista de foi din fișierul Excel
            Sheet sheet = new Sheet()
            {
                Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = "Raport"
            };
            sheets.Append(sheet);

            // Se obtin datele din DataGridView
            DataTable dataTable = new DataTable();

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                dataTable.Columns.Add(column.HeaderText);
            }

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                DataRow dataRow = dataTable.NewRow();
                for (int i = 0; i < dataGridView.Columns.Count; i++)
                {
                    dataRow[i] = row.Cells[i].Value;
                }
                dataTable.Rows.Add(dataRow);
            }

            // Se exporta datele în fișierul Excel
            SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            // Se adauga headerul în fișierul Excel
            Row headerRow = new Row();
            foreach (DataColumn column in dataTable.Columns)
            {
                Cell cell = new Cell();
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue(column.ColumnName);
                headerRow.AppendChild(cell);
            }
            sheetData.AppendChild(headerRow);

            //Se adauga rândurile de date în fișierul Excel
            foreach (DataRow row in dataTable.Rows)
            {
                Row dataRow = new Row();
                foreach (var item in row.ItemArray)
                {
                    Cell cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue(item.ToString());
                    dataRow.AppendChild(cell);
                }
                sheetData.AppendChild(dataRow);
            }

            spreadsheetDocument.Dispose();
            MessageBox.Show("Data has been successfully exported to Excel file on Desktop");
        }
    }
}
