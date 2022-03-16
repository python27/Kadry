using System;
using System.Linq;
using System.Xml;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Npoi.Mapper;
using Npoi.Mapper.Attributes;
using NPOI.SS.UserModel;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Windows.Controls.Primitives;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using Microsoft.WindowsAPICodePack.Dialogs;
using CDNKH;
using CDNHeal;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Threading;

namespace kadry.Pages.Main
{
    /// <summary>
    /// Logika interakcji dla klasy Menu.xaml
    /// </summary>
    public partial class Export : Page
    {
        public Export()
        {
            InitializeComponent();
        }

        private class PremFormat
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

        private void LoadPrem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Arkusze kalkulacyjne Excel (*.xls)|*.xls|Wszystkie pliki|*.*";
            ofd.Title = "Wybierz plik z premiami";
            ofd.Multiselect = false;

            ofd.ShowDialog();
            if (ofd.FileName != null && ofd.FileName.Trim() != "")
            {
                Stream fs = ofd.OpenFile();

                IWorkbook wb = WorkbookFactory.Create(fs);

                Mapper importer = new Mapper(wb);

                var Items = importer.Take<PremFormat>(2).ToList();

                List<PremFormat> Prems = new List<PremFormat>();

                foreach (var x in Items)
                {

                    PremFormat row = x.Value;
                    if (row.Kwota != null && !row.Kwota.StartsWith("\"") && !row.Kwota.EndsWith("\""))
                    {
                        Tools.Log(row.ToString());
                        Prems.Add(row);
                    }
                }

                List<ComboBoxDialogElement> Wyp = new List<ComboBoxDialogElement>();
                List<ComboBoxDialogElement> Wypłaty = new List<ComboBoxDialogElement>();
                using (SqlCommand c = new SqlCommand("SELECT WPL_WplId, WPL_NumerString FROM [" + Data.Config.Firm.DB + "].[CDN].Wyplaty", Data.FirmCn))
                {
                    using (SqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            Wyp.Add(new ComboBoxDialogElement(r.GetString(1).Replace("@numerS", "*"), r.GetInt32(0).ToString()));
                        }
                    }
                }

                foreach (ComboBoxDialogElement w in Wyp)
                {
                    bool ca = true;

                    foreach (ComboBoxDialogElement wy in Wypłaty)
                    {
                        if (w.Text == wy.Text)
                            ca = false;
                    }
                    if (ca)
                    {
                        Wypłaty.Add(w);
                    }
                }


                Windows.ListBoxDialog cbd = new Windows.ListBoxDialog("Wybierz wypłatę", "Wybierz wypłatę do której chcesz dodać premie", Wypłaty);

                cbd.ShowDialog();

                if (cbd.Result == null)
                {
                    MessageBox.Show("Nie wybrano żadnej wypłaty\nAnulowanie", "Brak wypłaty");
                    return;
                }

                ComboBoxDialogElement wyp = cbd.Result;

                CDNBase.AdoSession Sesja = Data.Optima.Login.CreateSession();

                foreach (PremFormat pr in Prems)
                {
                    Tools.Log($"Dodawanie wypłaty dla: {pr.Pracownik}");
                    var Pracownik = Sesja.CreateObject("CDN.Pracownicy", "PRA_Kod = '" + pr.Pracownik + "'");
                    var rTypSkladnika = Sesja.CreateObject("CDNPlCfg.TypySkladnikow", "TWP_TwpId = 53"); //Premia procentowa

                    try
                    {
                        var rWyplata = Sesja.CreateObject("CDNWyplaty.Wyplaty", String.Format("WPL_NumerString = '{0}' AND WPL_PraId = '{1}'", wyp.Text.Replace("*", "@numerS"), Pracownik.ID));

                        var rElement = rWyplata.Elementy.AddNew();
                        rElement.Wyplata = rWyplata;
                        var rSkladnik = rElement.Podstawowy;
                        rSkladnik.Okres.Period = rWyplata.Okres.Period;
                        rSkladnik.Typ = rTypSkladnika;
                        rSkladnik.Procent = Convert.ToDecimal(pr.Kwota);
                        rSkladnik.Wartosc1 = Convert.ToDecimal(pr.Kwota);
                        //rElement.Podstawowy.Procent = Convert.ToDecimal(pr.Kwota);
                        //rElement.Value = rWyplata.Brutto * Convert.ToDecimal(pr.Kwota) / 100;
                        rWyplata.PrzeliczWartosc();

                        var Lista = Sesja.CreateObject("CDNWyplaty.ListyPlac")("1");
                        var PodliczPodatki = Sesja.CreateObject("CDNWyplaty.PoliczPodatki");
                        PodliczPodatki.Pracownik = Pracownik;
                        PodliczPodatki.MiesiacDeklaracji = Lista.MiesiacDeklaracji.MonthYear;
                        PodliczPodatki.ListaPlac = Lista;
                        PodliczPodatki.Przelicz();
                    }
                    catch { }
                }
                Sesja.Save();



            }
        }

        private void LoadPremUz_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Arkusze kalkulacyjne Excel (*.xls)|*.xls|Wszystkie pliki|*.*";
            ofd.Title = "Wybierz plik z premiami";
            ofd.Multiselect = false;

            ofd.ShowDialog();
            if (ofd.FileName != null && ofd.FileName.Trim() != "")
            {
                Stream fs = ofd.OpenFile();

                IWorkbook wb = WorkbookFactory.Create(fs);

                Mapper importer = new Mapper(wb);

                var Items = importer.Take<PremFormat>(2).ToList();

                List<PremFormat> Prems = new List<PremFormat>();

                foreach (var x in Items)
                {

                    PremFormat row = x.Value;
                    if (row.Kwota != null && !row.Kwota.StartsWith("\"") && !row.Kwota.EndsWith("\""))
                    {
                        Prems.Add(row);
                    }
                }

                List<ComboBoxDialogElement> Wyp = new List<ComboBoxDialogElement>();
                List<ComboBoxDialogElement> Wypłaty = new List<ComboBoxDialogElement>();
                using (SqlCommand c = new SqlCommand("SELECT WPL_WplId, WPL_NumerString FROM [" + Data.Config.Firm.DB + "].[CDN].Wyplaty", Data.FirmCn))
                {
                    using (SqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            Wyp.Add(new ComboBoxDialogElement(r.GetString(1).Replace("@numerS", "*"), r.GetInt32(0).ToString()));
                        }
                    }
                }

                foreach (ComboBoxDialogElement w in Wyp)
                {
                    bool ca = true;

                    foreach (ComboBoxDialogElement wy in Wypłaty)
                    {
                        if (w.Text == wy.Text)
                            ca = false;
                    }
                    if (ca)
                    {
                        Wypłaty.Add(w);
                    }
                }


                Windows.ListBoxDialog cbd = new Windows.ListBoxDialog("Wybierz wypłatę", "Wybierz wypłatę do której chcesz dodać premie", Wypłaty);

                cbd.ShowDialog();

                if (cbd.Result == null)
                {
                    MessageBox.Show("Nie wybrano żadnej wypłaty\nAnulowanie", "Brak wypłaty");
                    return;
                }

                ComboBoxDialogElement wyp = cbd.Result;

                CDNBase.AdoSession Sesja = Data.Optima.Login.CreateSession();

                foreach (PremFormat pr in Prems)
                {
                    var Pracownik = Sesja.CreateObject("CDN.Pracownicy", "PRA_Kod = '" + pr.Pracownik + "'");
                    var rTypSkladnika = Sesja.CreateObject("CDNPlCfg.TypySkladnikow", "TWP_TwpId = 52"); //Premia procentowa

                    try
                    {
                        var rWyplata = Sesja.CreateObject("CDNWyplaty.Wyplaty", String.Format("WPL_NumerString = '{0}' AND WPL_PraId = '{1}'", wyp.Text.Replace("*", "@numerS"), Pracownik.ID));

                        var rElement = rWyplata.Elementy.AddNew();
                        rElement.Wyplata = rWyplata;
                        var rSkladnik = rElement.Podstawowy;
                        rSkladnik.Okres.Period = rWyplata.Okres.Period;
                        rSkladnik.Typ = rTypSkladnika;
                        rSkladnik.Wartosc1 = Convert.ToDecimal(pr.Kwota);
                        //rElement.Value =;
                        rWyplata.PrzeliczWartosc();

                        var Lista = Sesja.CreateObject("CDNWyplaty.ListyPlac")("1");
                        var PodliczPodatki = Sesja.CreateObject("CDNWyplaty.PoliczPodatki");
                        PodliczPodatki.Pracownik = Pracownik;
                        PodliczPodatki.MiesiacDeklaracji = Lista.MiesiacDeklaracji.MonthYear;
                        PodliczPodatki.ListaPlac = Lista;
                        PodliczPodatki.Przelicz();
                    }
                    catch { }
                }
                Sesja.Save();

            }
        }

        private class SkType
        {
            public SkType(int id, string nazwa, int algorytm)
            {
                ID = id;
                Nazwa = nazwa;
                Algorytm = algorytm;
            }
            public int ID { get; set; }
            public string Nazwa { get; set; }
            public int Algorytm { get; set; }
        }

        private class ListaPlac
        {
            public ListaPlac(int LpId, string NrOpt, string nazwa)
            {
                NumerOpt = NrOpt;
                Nazwa = nazwa;
                IdListyPłac = LpId;
            }
            public string NumerOpt { get; set; }
            public string Nazwa { get; set; }
            public int IdListyPłac { get; }
        }

        private string LoadedFilePath = null;

        /// <summary>
        /// Wybieranie pliku do wczytania
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Arkusze kalkulacyjne Excel (*.xls)|*.xls*|Wszystkie pliki|*.*",
                Title = "Wybierz plik z premiami",
                Multiselect = false
            };

            ofd.ShowDialog();
            if (ofd.FileName != null && ofd.FileName.Trim() != "")
            {
                LoadedFilePath = ofd.FileName;
                //LoadedFile.Text = Path.GetFileName(ofd.FileName);
                if (WybranaLista != null && LoadedFilePath != null && LoadedFilePath.Trim() != "")
                {
                    Start.IsEnabled = true;
                }

            }
        }

        private SkType WybranyTyp = null;

        /// <summary>
        /// Pozwala na wybór typu importowanego składnika
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChSkType_Click(object sender, RoutedEventArgs e)
        {
            List<SkType> Typy = new List<SkType>();

            //Pobiera listę składników... Ja nie wiem czemu ta kwerenda jest tak długa, ja tylko sprawdziłem Profilerem SQL Servera jakie zapytanie wysyła Optima do bazy

            using (SqlCommand c = new SqlCommand(String.Format("SELECT TWP_TwpId, TWP_Nazwa, TWP_Skrot, TWP_Nieaktywny, TWP_Standardowy, TWP_AlgAlgorytm FROM [{0}].CDN.TypWyplata WHERE ((((twp_umowa <> 1 and twp_typwaluty <> 1 and (TWP_Standardowy = 0  AND TWP_AlgAlgorytm <> 13 AND TWP_AlgAlgorytm <> 7 AND TWP_AlgAlgorytm <> 8 AND TWP_ZaOkrZwolLek = 0 or twp_standardowy = 454 OR TWP_Standardowy = 801 OR TWP_Standardowy = 803 OR TWP_Standardowy = 811 OR TWP_Standardowy = 812 OR TWP_Standardowy = 813 OR (TWP_Standardowy >= 600 AND TWP_Standardowy <= 699) OR (TWP_Standardowy >= 300 AND TWP_Standardowy <= 399) OR (TWP_Standardowy >= 250 AND TWP_Standardowy <= 299) OR (TWP_Standardowy >= 410 AND TWP_Standardowy <= 413) or (TWP_Standardowy >= 436 and TWP_Standardowy <= 438) OR (TWP_Standardowy >= 503 AND TWP_Standardowy <= 504) OR (TWP_Standardowy >= 510 AND TWP_Standardowy <= 511) or twp_standardowy = 850 or twp_standardowy = 860 or twp_standardowy = 870 or twp_standardowy = 821 or twp_standardowy = 15 or twp_standardowy = 16 or twp_standardowy = 17 or twp_standardowy = 18 or twp_standardowy = 19 or twp_standardowy = 20 or twp_standardowy = 40 or twp_standardowy = 41 or twp_standardowy = 42 or twp_standardowy = 43))) AND TWP_Nieaktywny <> 1 AND (TWP_AlgAlgorytm = 1 OR TWP_AlgAlgorytm = 2 OR TWP_AlgAlgorytm = 11))) ORDER BY TWP_Nazwa", Data.Config.Firm.DB), Data.FirmCn))
            {
                using (SqlDataReader r = c.ExecuteReader())
                {
                    while (r.Read())
                    {
                        Typy.Add(new Export.SkType(Convert.ToInt32(r[0]), r[1].ToString(), Convert.ToInt32(r["TWP_AlgAlgorytm"])));
                    }
                }
            }

            List<ComboBoxDialogElement> Skl = new List<ComboBoxDialogElement>();

            foreach (SkType t in Typy)
            {
                Skl.Add(new ComboBoxDialogElement(t.Nazwa, t.ID.ToString()));
            }

            //Przedstawianie wyboru użytkownikowi
            Windows.ListBoxDialog cbd = new Windows.ListBoxDialog("Wybierz składnik", "Wybierz wypłatę do której chcesz dodać premie", Skl);

            cbd.ShowDialog();

            if (cbd.Result == null)
            {
                MessageBox.Show("Nie wybrano żadnego typu składnika", "Brak typu składnika");
                return;
            }

            //Wprowadzanie wybranego składnika
            ComboBoxDialogElement wyp = cbd.Result;

            WybranyTyp = Typy.FirstOrDefault(x => x.ID.ToString() == wyp.Id);

            SkTypeText.Text = wyp.Text;

            if (WybranaLista != null && LoadedFilePath != null && LoadedFilePath.Trim() != "")
            {
                Start.IsEnabled = true;
            }
        }

        private ListaPlac WybranaLista = null;

        /// <summary>
        /// Wybiera listę płac
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChList_Click(object sender, RoutedEventArgs e)
        {
            List<DekretDialogElement> Wyp = new List<DekretDialogElement>();
            List<DekretDialogElement> Wypłaty = new List<DekretDialogElement>();

            // SELECT CDN.DekretyNag.DeN_Dokument as 'numer LP do wyboru na liście dokumentów', DeN_DataDok as 'data faza 1 (dokumentu)',  DeN_DataWys as 'data faza 2 (wypłaty)' FROM CDN.Konta AS kontaWN INNER JOIN CDN.DekretyElem ON kontaWN.Acc_AccId = CDN.DekretyElem.DeE_AccWnId INNER JOIN CDN.Konta AS KontaMA ON CDN.DekretyElem.DeE_AccMaId = KontaMA.Acc_AccId INNER JOIN CDN.DekretyNag ON CDN.DekretyElem.DeE_DeNId = CDN.DekretyNag.DeN_DeNId where DeN_Zrodlo= 10

            //"SELECT WPL_WplId, WPL_NumerString, [WPL_DataOd], [WPL_LplId] FROM [" + Data.Config.Firm.DB + "].[CDN].Wyplaty"

            string db = Data.Config.Firm.DB;
            //Pobiera listę płac z Optimy
            using (SqlCommand c = new SqlCommand($@"
SELECT 
    [{db}].CDN.DekretyNag.DeN_Dokument as 'numer LP do wyboru na liście dokumentów',
    DeN_DataDok as 'data faza 1 (dokumentu)',
    DeN_DataWys as 'data faza 2 (wypłaty)'
FROM [{db}].CDN.Konta AS kontaWN INNER JOIN [{db}].CDN.DekretyElem ON kontaWN.Acc_AccId = [{db}].CDN.DekretyElem.DeE_AccWnId  INNER JOIN [{db}].CDN.DekretyNag ON [{db}].CDN.DekretyElem.DeE_DeNId = [{db}].CDN.DekretyNag.DeN_DeNId where DeN_Zrodlo= 10 ", Data.FirmCn))
            {
                using (SqlDataReader r = c.ExecuteReader())
                {
                    while (r.Read())
                    {
                        Wyp.Add(new DekretDialogElement(r[0].ToString(), "-1", Convert.ToDateTime(r[2]), -1));
                    }
                }
            }

            Wypłaty = Wyp.Distinct(new WypłatyComparer()).ToList();

            //Pokazanie okna wyboru wypłat
            Windows.DekretDialog cbd = new Windows.DekretDialog("Wybierz wypłatę", Wypłaty);

            cbd.ShowDialog();

            if (cbd.Result == null)
            {
                MessageBox.Show("Nie wybrano żadnej wypłaty", "Brak wypłaty");
                return;
            }

            //ustawianie wybranej listy
            WyplataDialogElement wyp = cbd.Result;

            WybranaLista = new ListaPlac(wyp.IdListyPłac, wyp.Text.Replace("*", "@numerS"), wyp.Text);

            LstPlac.Text = wyp.Text;

            if (WybranaLista != null && !string.IsNullOrWhiteSpace(SavePath))
            {
                Start.IsEnabled = true;
            }

        }

        private class WypłatyComparer : IEqualityComparer<DekretDialogElement>
        {
            public bool Equals(DekretDialogElement x, DekretDialogElement y)
            {
                //Check whether the compared objects reference the same data.
                if (Object.ReferenceEquals(x, y)) return true;

                //Check whether any of the compared objects is null.
                if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                    return false;

                if(x.Data.HasValue && y.Data.HasValue)
                {
                    return x.Text == y.Text && x.Data.Value.Equals(y.Data.Value);
                }
                else
                {
                    return false;
                }
            }
            public int GetHashCode(DekretDialogElement product)
            {
                //Check whether the object is null
                if (Object.ReferenceEquals(product, null)) return 0;

                //Get hash code for the Name field if it is not null.
                int hashProductName = product.Text?.GetHashCode() ?? 0;

                //Get hash code for the Code field.
                int hashProductCode = product.Data?.GetHashCode() ?? 0;

                //Calculate the hash code for the product.
                return hashProductName ^ hashProductCode;
            }
        }

        /// <summary>
        /// Wraca do menu głównego
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Tools.Goto(new ImportExport());
        }

        /// <summary>
        /// Uruchamia proces importowania składników płac
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            new Thread(() =>
            {
                try
                {
                    List<ExportRecord> er1 = new List<ExportRecord>();
                    string db = Data.Config.Firm.DB;

                    using (SqlCommand c = new SqlCommand($@"
SELECT 
[{db}].CDN.DekretyNag.DeN_Dokument as 'numer LP do wyboru na liście dokumentów',
DeN_DataDok as 'data faza 1 (dokumentu)',
DeN_DataWys as 'data faza 2 (wypłaty)',
left(DeE_Kategoria, 1) as 'Nr pliku',
concat (DeE_Dokument, ' - Faza ',
left(DeE_Kategoria, 1)  ) as 'Nazwa pliku',
kontaWN.Acc_Nazwa2 as 'Typ konta',
[{db}].CDN.DekretyElem.DeE_KontoWn as Konto,
DeE_Dokument as 'Tekst transakcji1',
DeE_Kategoria as 'Tekst transakcji2',
DeE_Kwota as 'Debet',
'PLN' as Waluta,
'100,00' as 'Kurs wymiany',
'Ogol' as Dział, right (DeE_Kategoria, 6) as 'MPK',
'STD' as 'Profil księgowania',
DeE_Kwota as 'Kredyt', 
[{db}].CDN.DekretyElem.DeE_KontoMa as 'Konto przeciwstawne',
KontaMA.Acc_Nazwa2 AS 'Typ konta przeciwstawnego'

FROM [{db}].CDN.Konta AS kontaWN INNER JOIN
[{db}].CDN.DekretyElem ON kontaWN.Acc_AccId = [{db}].CDN.DekretyElem.DeE_AccWnId INNER JOIN        [{db}].CDN.Konta AS KontaMA ON 
[{db}].CDN.DekretyElem.DeE_AccMaId = KontaMA.Acc_AccId INNER JOIN       [{db}].CDN.DekretyNag ON [{db}].CDN.DekretyElem.DeE_DeNId = 
[{db}].CDN.DekretyNag.DeN_DeNId		 where DeN_Zrodlo= 10 and [{db}].CDN.DekretyNag.DeN_Dokument = '{WybranaLista.Nazwa}'", Data.Sqlc))
                    {
                        using (SqlDataReader r = c.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                var ere = new ExportRecord(
                                    r[0].ToString(),
                                    Convert.ToDateTime(r[1]).ToString("yyyy-MM-dd"),
                                    Convert.ToDateTime(r[2]).ToString("yyyy-MM-dd"),
                                    1,
                                    r[4].ToString(),
                                    r[5].ToString(),
                                    r[6].ToString(),
                                    r[7].ToString(),
                                    r[8].ToString(),
                                    Convert.ToDecimal(r[9]),
                                    r[10].ToString(),
                                    Convert.ToDecimal(r[11]),
                                    r[12].ToString(),
                                    r[13].ToString(),
                                    r[14].ToString(),
                                    Convert.ToDecimal(r[15]),
                                    r[16].ToString(),
                                    r[17].ToString()
                                    );
                                
                                
                                if (int.TryParse(r[3].ToString(), out int res))
                                {
                                    ere.NumerPliku = res;
                                }


                                er1.Add(ere);
                            }
                        }
                    }

                    List<ExportRecord> er2 = er1.Where(x => x.NumerPliku == 2).ToList();
                    er1 = er1.Where(x => x.NumerPliku == 1).ToList();

                    var filteredEr1 = new List<ExportRecord>();
                    var filteredEr2 = new List<ExportRecord>();

                    foreach(var record in er1)
                    {
                        var existingRecord = filteredEr1.Find(x => x.Equals(record));
                        if (existingRecord != null)
                        {
                            existingRecord.Kredyt += record.Kredyt;
                            existingRecord.Debet   += record.Debet;
                            continue;
                        }
                        filteredEr1.Add(record);
                    }
                    er1 = filteredEr1;

                    foreach (var record in er2)
                    {
                        var existingRecord = filteredEr2.Find(x => x.Equals(record));
                        if (existingRecord != null)
                        {
                            existingRecord.Kredyt += record.Kredyt;
                            existingRecord.Debet += record.Debet;
                            continue;
                        }
                        filteredEr2.Add(record);
                    }
                    er2 = filteredEr2;


                    new Thread(() =>
                    {
                        if (er1.Count > 0)
                        {
                            try
                            {
                                string fn = er1[0].NazwaPliku.Replace("/", "_").Replace("*", "");

                                string fname = Path.Combine(SavePath, fn);
                                if (File.Exists($"{fname}.xlsx"))
                                {
                                    throw new Exception($"Plik {fname}.xlsx już istnieje");
                                }
                                FileInfo fi = new FileInfo($"{fname}.xlsx");

                                using (ExcelPackage pack = new ExcelPackage(fi))
                                {
                                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("Arkusz1");
                                    ws.CreateHeader();

                                    int i = 2;
                                    foreach (ExportRecord er in er1)
                                    {
                                        ws.CreateRow(i, er);
                                        i++;
                                    }

                                    for (int j = 1; j <= 13; j++)
                                    {
                                        ws.Column(j).BestFit = true;
                                        ws.Column(j).AutoFit();
                                    }

                                    pack.Save();
                                }

                            }
                            catch (Exception ex1)
                            {
                                MessageBox.Show($"Eksport pliku dla fazy 1 nie powiódł się\n{ex1.Message}", "Błąd eksportu");

                            }

                        }


                        if (er2.Count > 0)
                        {
                            try
                            {
                                string fname = er2[0].NazwaPliku.Replace("/", "_").Replace("*", "");
                                fname = Path.Combine(SavePath, fname);
                                if (File.Exists($"{fname}.xlsx"))
                                {
                                    throw new Exception($"Plik {fname}.xlsx już istnieje");
                                }
                                FileInfo fi = new FileInfo($"{fname}.xlsx");

                                using (ExcelPackage pack = new ExcelPackage(fi))
                                {
                                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("Arkusz1");
                                    ws.CreateHeader();

                                    int i = 2;
                                    foreach (ExportRecord er in er2)
                                    {
                                        ws.CreateRow(i, er);
                                        i++;

                                    }
                                    for (int j = 1; j <= 13; j++)
                                    {
                                        ws.Column(j).BestFit = true;
                                        ws.Column(j).AutoFit();
                                    }

                                    pack.Save();
                                }

                            }
                            catch (Exception ex1)
                            {
                                MessageBox.Show($"Eksport pliku dla fazy 2 nie powiódł się\n{ex1.Message}", "Błąd eksportu");
                            }
                            MessageBox.Show("Eksport plików został zakończony", "Eksport");
                        }
                    }).Start();


                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Eksport plików nie powiódł się\n{ex.Message}", "Błąd eksportu");
                }
            }).Start();
        }

        public class ExportRecord
        {

            public ExportRecord(
                string numer,
                string dataDokumentu,
                string dataWypłaty,
                int numerPliku,
                string nazwaPliku,
                string typKonta,
                string konto,
                string tekstTransakcji,
                string tekstTransakcji1,
                decimal debet,
                string waluta,
                decimal kursWymiany,
                string dział,
                string mpk,
                string profilKsięgowania,
                decimal kredyt,
                string kontoPrzeciwstawne,
                string typKontaPrzeciwstawnego)
            {
                Numer = numer;
                DataDokumentu = dataDokumentu;
                DataWypłaty = dataWypłaty;
                NumerPliku = numerPliku;
                NazwaPliku = nazwaPliku;
                TypKonta = typKonta;
                Konto = konto;

                TekstTransakcji1 = tekstTransakcji;

                string tt1 = tekstTransakcji1;
                int ind = tt1.IndexOf("/");
                tt1 = tt1.Substring(ind + 1);

                TekstTransakcji = $"{tekstTransakcji};{tt1}";

                TekstTransakcji2 = tt1;

                ind = tt1.IndexOf("/");
                tt1 = tt1.Substring(0, ind);

                TekstTransakcji3 = tt1;
                TekstTransakcji4 = $"{tekstTransakcji};{tt1}";

                Debet = debet;
                Waluta = waluta;
                KursWymiary = kursWymiany;
                Dział = dział;
                MPK = mpk;
                ProfilKsięgowania = profilKsięgowania;
                Kredyt = kredyt;
                KontoPrzeciwstawne = kontoPrzeciwstawne.Replace("D", "");
                TypKontaPrzeciwstawnego = typKontaPrzeciwstawnego;
            }
            public string Numer { get; }
            public string DataDokumentu { get; }
            public string DataWypłaty { get; }
            public int NumerPliku { get; set; }
            public string NazwaPliku { get; }
            public string TypKonta { get; }
            public string Konto { get; }
            public string TekstTransakcji { get;  }

            public string TekstTransakcji1 { get; }
            public string TekstTransakcji2 { get; }
            public string TekstTransakcji3 { get; }
            public string TekstTransakcji4 { get; }

            public decimal Debet { get; set; }
            public string Waluta { get; }
            public decimal KursWymiary { get; }
            public string Dział { get; }
            public string MPK { get; }
            public string ProfilKsięgowania { get; }
            public decimal Kredyt { get; set; }
            public string KontoPrzeciwstawne { get; }
            public string TypKontaPrzeciwstawnego { get; }

            public override bool Equals(object obj)
            {
                if (!typeof(ExportRecord).Equals(obj.GetType()))
                    return false;

                var o = (ExportRecord)obj;

                return 
                    Numer                   == o.Numer                   &&
                    DataDokumentu           == o.DataDokumentu           &&
                    DataWypłaty             == o.DataWypłaty             &&
                    NumerPliku              == o.NumerPliku              &&
                    NazwaPliku              == o.NazwaPliku              &&
                    TypKonta                == o.TypKonta                &&
                    Konto                   == o.Konto                   &&
                    TekstTransakcji         == o.TekstTransakcji         &&
                    TekstTransakcji1        == o.TekstTransakcji1        &&
                    TekstTransakcji2        == o.TekstTransakcji2        &&
                    TekstTransakcji3        == o.TekstTransakcji3        &&
                    TekstTransakcji4        == o.TekstTransakcji4        &&
                    Waluta                  == o.Waluta                  &&
                    KursWymiary             == o.KursWymiary             &&
                    Dział                   == o.Dział                   &&
                    MPK                     == o.MPK                     &&
                    ProfilKsięgowania       == o.ProfilKsięgowania       &&
                    KontoPrzeciwstawne      == o.KontoPrzeciwstawne      &&
                    TypKontaPrzeciwstawnego == o.TypKontaPrzeciwstawnego;
            }
        }

        private DateTime? SelectedDate;

        private void DateCh_Click(object sender, RoutedEventArgs e)
        {
            MouseDevice mouseDev = InputManager.Current.PrimaryMouseDevice;
            DatePick.RaiseEvent(new MouseButtonEventArgs(mouseDev, 0, MouseButton.Left)
            {
                RoutedEvent = MouseLeftButtonUpEvent
            });
        }

        private string SavePath;

        private void ChSavePlace_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                IsFolderPicker = true,
                Title = "Wybierz miejsce zapisu"
            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                SavePath = dialog.FileName;
                SkTypeText.Text = SavePath;
            }
            if (WybranaLista != null && !string.IsNullOrWhiteSpace(SavePath))
            {
                Start.IsEnabled = true;
            }
        }

        private string DateText { get {
                if (DatePick.SelectedDate != null)
                {
                    DateTime d = (DateTime)DatePick.SelectedDate;
                    return d.ToString("dd.MM.yyyy");
                }
                else
                {
                    return "Nie wybrano";
                };
            }
        }

        private void DatePick_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //SelDate.Text = DateText;
        }
    }
}
