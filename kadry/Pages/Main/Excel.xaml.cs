using Newtonsoft.Json;
using Npoi.Mapper.Attributes;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace kadry.Pages.Main
{
    /// <summary>
    /// Logika interakcji dla klasy Excel.xaml
    /// </summary>
    public partial class Excel : Page
    {
        DataTable dataTable = null;
        List<PremFormat> Prems = new List<PremFormat>();
        public Excel()
        {
            InitializeComponent();

            foreach (var item in ImportPlik.headerRow.Cells)
            {
                ComboboxItem items = new ComboboxItem();
                items.Text = item.ToString();
                comboBoxKwota.Items.Add(items);
                comboBoxPracownik.Items.Add(items);
                comboBoxNazwisko.Items.Add(items);
            }
        }

        public class PremFormat
        {
            [Column(0)]
            public string Kwota { get; set; }

            [Column(1)]
            public string Pracownik { get; set; }

            [Column(2)]
            public string Nazwisko { get; set; }

            public override string ToString()
            {
                return $"Kwota: {Kwota}, Pracownik: {Pracownik}, Nazwisko: {Nazwisko}";
            }
        }
        
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Excel_To_DataTable(ImportPlik.LoadedFilePath, 0);
                Prems = WczytajInterpretujPlik();
                MessageBox.Show("Interpreter wczytał poprawnie dane: " + JsonConvert.SerializeObject(Prems));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public class ComboboxItem
        {
            public string Text { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Tools.Goto(new Excel());
        }

        private void ComboBoxKwota_DropDownClosed(object sender, EventArgs e)
        {
            if (comboBoxKwota.Text != "" && comboBoxPracownik.Text != "" || comboBoxNazwisko.Text != "")
            {
                Start.IsEnabled = true;
            }
        }

        private void ComboBoxPracownik_DropDownClosed(object sender, EventArgs e)
        {
            if (comboBoxKwota.Text != "" && comboBoxPracownik.Text != "" || comboBoxNazwisko.Text != "")
            {
                Start.IsEnabled = true;
            }
        }

        private void ComboBoxNazwisko_DropDownClosed(object sender, EventArgs e)
        {
            if (comboBoxKwota.Text != "" && comboBoxPracownik.Text != "" || comboBoxNazwisko.Text != "")
            {
                Start.IsEnabled = true;
            }
        }

        public static IRow headerRow;
        private List<PremFormat> WczytajInterpretujPlik()
        {
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                if (dataTable.Columns[i].ColumnName != comboBoxKwota.Text && dataTable.Columns[i].ColumnName != comboBoxPracownik.Text && dataTable.Columns[i].ColumnName != comboBoxNazwisko.Text)
                    dataTable.Columns.RemoveAt(i);
            }

            return Prems = (from rw in dataTable.AsEnumerable()
                select new PremFormat()
                {
                    Kwota = rw[comboBoxKwota.Text].ToString(),
                    Pracownik = rw[comboBoxPracownik.Text].ToString(),
                    Nazwisko = rw[comboBoxNazwisko.Text].ToString()
                }).ToList();
        }

        private DataTable Excel_To_DataTable(string path, int sheetIndex)
        {
            try
            {
                if (System.IO.File.Exists(path))
                {

                    IWorkbook workbook = null;
                    ISheet worksheet = null;
                    string first_sheet_name = "";

                    using (FileStream FS = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        workbook = WorkbookFactory.Create(FS);
                        worksheet = workbook.GetSheetAt(sheetIndex);
                        first_sheet_name = worksheet.SheetName;

                        dataTable = new DataTable(first_sheet_name);
                        dataTable.Rows.Clear();
                        dataTable.Columns.Clear();

                        for (int rowIndex = 0; rowIndex <= worksheet.LastRowNum; rowIndex++)
                        {
                            DataRow NewReg = null;
                            IRow row = worksheet.GetRow(rowIndex);
                            IRow row2 = null;

                            if (rowIndex == 0)
                            {
                                row2 = worksheet.GetRow(rowIndex + 1);
                            }

                            if (row != null)
                            {
                                if (rowIndex > 0) NewReg = dataTable.NewRow();

                                int colIndex = 0;
                                foreach (ICell cell in row.Cells)
                                {
                                    object valorCell = null;
                                    string cellType = "";
                                    string[] cellType2 = new string[2];

                                    if (rowIndex == 0)
                                    {
                                        for (int i = 0; i < 2; i++)
                                        {
                                            ICell cell2 = null;
                                            if (i == 0) { cell2 = row2.GetCell(cell.ColumnIndex); }

                                            if (cell2 != null)
                                            {
                                                switch (cell2.CellType)
                                                {
                                                    case CellType.Blank: break;
                                                    case CellType.Boolean: cellType2[i] = "System.Boolean"; break;
                                                    case CellType.String: cellType2[i] = "System.String"; break;
                                                    case CellType.Numeric:
                                                        if (HSSFDateUtil.IsCellDateFormatted(cell2)) { cellType2[i] = "System.DateTime"; }
                                                        else
                                                        {
                                                            cellType2[i] = "System.Double";
                                                        }
                                                        break;

                                                    case CellType.Formula:
                                                        bool continuar = true;
                                                        switch (cell2.CachedFormulaResultType)
                                                        {
                                                            case CellType.Boolean: cellType2[i] = "System.Boolean"; break;
                                                            case CellType.String: cellType2[i] = "System.String"; break;
                                                            case CellType.Numeric:
                                                                if (HSSFDateUtil.IsCellDateFormatted(cell2)) { cellType2[i] = "System.DateTime"; }
                                                                else
                                                                {
                                                                    try
                                                                    {
                                                                        if (cell2.CellFormula == "TRUE()") { cellType2[i] = "System.Boolean"; continuar = false; }
                                                                        if (continuar && cell2.CellFormula == "FALSE()") { cellType2[i] = "System.Boolean"; continuar = false; }
                                                                        if (continuar) { cellType2[i] = "System.Double"; continuar = false; }
                                                                    }
                                                                    catch { }
                                                                }
                                                                break;
                                                        }
                                                        break;
                                                    default:
                                                        cellType2[i] = "System.String"; break;
                                                }
                                            }
                                        }

                                        if (cellType2[0] == cellType2[1]) { cellType = cellType2[0]; }
                                        else
                                        {
                                            if (cellType2[0] == null) cellType = cellType2[1];
                                            if (cellType2[1] == null) cellType = cellType2[0];
                                            if (cellType == "") cellType = "System.String";
                                        }

                                        string colName = "Column_{0}";
                                        try { colName = cell.StringCellValue; }
                                        catch { colName = string.Format(colName, colIndex); }

                                        foreach (DataColumn col in dataTable.Columns)
                                        {
                                            if (col.ColumnName == colName) colName = string.Format("{0}_{1}", colName, colIndex);
                                        }

                                        DataColumn codigo = new DataColumn(colName, System.Type.GetType(cellType));
                                        dataTable.Columns.Add(codigo); colIndex++;
                                    }
                                    else
                                    {
                                        switch (cell.CellType)
                                        {
                                            case CellType.Blank: valorCell = DBNull.Value; break;
                                            case CellType.Boolean: valorCell = cell.BooleanCellValue; break;
                                            case CellType.String: valorCell = cell.StringCellValue; break;
                                            case CellType.Numeric:
                                                if (HSSFDateUtil.IsCellDateFormatted(cell)) { valorCell = cell.DateCellValue; }
                                                else { valorCell = cell.NumericCellValue; }
                                                break;
                                            case CellType.Formula:
                                                switch (cell.CachedFormulaResultType)
                                                {
                                                    case CellType.Blank: valorCell = DBNull.Value; break;
                                                    case CellType.String: valorCell = cell.StringCellValue; break;
                                                    case CellType.Boolean: valorCell = cell.BooleanCellValue; break;
                                                    case CellType.Numeric:
                                                        if (HSSFDateUtil.IsCellDateFormatted(cell)) { valorCell = cell.DateCellValue; }
                                                        else { valorCell = cell.NumericCellValue; }
                                                        break;
                                                }
                                                break;
                                            default: valorCell = cell.StringCellValue; break;
                                        }
                                        if (cell.ColumnIndex <= dataTable.Columns.Count - 1) NewReg[cell.ColumnIndex] = valorCell;
                                    }
                                }
                            }
                            if (rowIndex > 0) dataTable.Rows.Add(NewReg);
                        }
                        dataTable.AcceptChanges();
                    }
                }
                else
                {
                    throw new Exception("Podany plik nie istnieje.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dataTable;
        }
    }
}