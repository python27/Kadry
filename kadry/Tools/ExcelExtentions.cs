using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kadry
{
    public static class ExcelExtentions
    {
        //kolory kolumn
        private static System.Drawing.Color color1 = System.Drawing.Color.FromArgb(0, 176, 80); //zielony
        private static System.Drawing.Color color2 = System.Drawing.Color.FromArgb(248, 203, 173); //cielisty

        public static void CreateHeader(this ExcelWorksheet ws)
        {
            int headerIndex = 1;
            ws.Cells[headerIndex, 1].Value = "Typ konta";
            ws.Cells[headerIndex, 2].Value = "Konto";
            ws.Cells[headerIndex, 3].Value = "Tekst transakcji";
            ws.Cells[headerIndex, 4].Value = "Debet";
            ws.Cells[headerIndex, 5].Value = "Waluta";
            ws.Cells[headerIndex, 6].Value = "Kurs wymiany";
            ws.Cells[headerIndex, 7].Value = "Dział";
            ws.Cells[headerIndex, 8].Value = "MPK";
            ws.Cells[headerIndex, 9].Value = "Profil księgowania";
            ws.Cells[headerIndex, 10].Value = "Kredyt";
            ws.Cells[headerIndex, 11].Value = "Konto przeciwstawne";
            ws.Cells[headerIndex, 12].Value = "Typ konta przeciwstawnego";
            ws.Cells[headerIndex, 13].Value = "Data";

            ws.Cells[$"A{headerIndex}:M{headerIndex}"].Style.Font.Bold = true;
            ws.Cells[$"A{headerIndex}:M{headerIndex}"].Style.Fill.PatternType = ExcelFillStyle.Solid;

            ws.Cells[$"A{headerIndex}:M{headerIndex}"].Style.Fill.BackgroundColor.SetColor(color2);

            ws.Cells[$"A{headerIndex}:F{headerIndex}"].Style.Fill.BackgroundColor.SetColor(color1);
            ws.Cells[$"H{headerIndex}:J{headerIndex}"].Style.Fill.BackgroundColor.SetColor(color1);
            ws.Cells[$"M{headerIndex}"].Style.Fill.BackgroundColor.SetColor(color1);

            //for (int j = 1; j <= 13; j++)
            //{
            //    ws.Column(j).Width = 22;
            //}
        }

        public static void CreateRow(this ExcelWorksheet ws, int i, Pages.Main.Export.ExportRecord r)
        {
            ws.Cells[i, 1].Value = r.TypKonta;
            ws.Cells[i, 2].Value = r.Konto;
            ws.Cells[i, 3].Value = $"{r.NumerPliku}-{r.TekstTransakcji4}";
            ws.Cells[i, 4].Value = r.Debet;
            ws.Cells[i, 4].Style.Numberformat.Format = "0.00";
            ws.Cells[i, 5].Value = r.Waluta;
            ws.Cells[i, 6].Value = r.KursWymiary;
            ws.Cells[i, 7].Value = r.Dział;
            ws.Cells[i, 8].Value = r.MPK;
            ws.Cells[i, 9].Value = r.ProfilKsięgowania;
            ws.Cells[i, 10].Value = null;// r.Kredyt;
            ws.Cells[i, 10].Style.Numberformat.Format = "0.00";
            ws.Cells[i, 11].Value = r.KontoPrzeciwstawne;
            ws.Cells[i, 12].Value = r.TypKontaPrzeciwstawnego;
            ws.Cells[i, 13].Value = r.NumerPliku == 1 ? Convert.ToDateTime(r.DataDokumentu) : Convert.ToDateTime(r.DataWypłaty);
            ws.Cells[i, 13].Style.Numberformat.Format = "yyyy-mm-dd";

            //ws.Cells[$"A{i}:M{i}"].Style.Font.Bold = true;
            ws.Cells[$"A{i}:M{i}"].Style.Fill.PatternType = ExcelFillStyle.Solid;

            ws.Cells[$"A{i}:M{i}"].Style.Fill.BackgroundColor.SetColor(color2);

            ws.Cells[$"A{i}:F{i}"].Style.Fill.BackgroundColor.SetColor(color1);
            ws.Cells[$"H{i}:J{i}"].Style.Fill.BackgroundColor.SetColor(color1);
            ws.Cells[$"M{i}"].Style.Fill.BackgroundColor.SetColor(color1);
        }
    }
}
