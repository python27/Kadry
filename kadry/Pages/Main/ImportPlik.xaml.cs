using Microsoft.Win32;
using Npoi.Mapper;
using Npoi.Mapper.Attributes;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace kadry.Pages.Main
{
    /// <summary>
    /// Logika interakcji dla klasy ImportPlik.xaml
    /// </summary>
    public partial class ImportPlik : Page
    {
        public ImportPlik()
        {
            InitializeComponent();
        }
        /*
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
        */

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

        public static string LoadedFilePath = null;

        /// <summary>
        /// Wybieranie pliku do wczytania
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Arkusze kalkulacyjne Excel (*.xls)|*.xls*|Wszystkie pliki|*.*";
            ofd.Title = "Wybierz plik z premiami";
            ofd.Multiselect = false;

            ofd.ShowDialog();
            if (ofd.FileName != null && ofd.FileName.Trim() != "")
            {
                LoadedFilePath = ofd.FileName;
                LoadedFile.Text = Path.GetFileName(ofd.FileName);
                if (WybranaLista != null && WybranyTyp != null && LoadedFilePath != null && LoadedFilePath.Trim() != "")
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
                        Typy.Add(new ImportPlik.SkType(Convert.ToInt32(r[0]), r[1].ToString(), Convert.ToInt32(r["TWP_AlgAlgorytm"])));
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

            if (WybranaLista != null && WybranyTyp != null && LoadedFilePath != null && LoadedFilePath.Trim() != "")
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
            List<WyplataDialogElement> Wyp = new List<WyplataDialogElement>();
            List<WyplataDialogElement> Wypłaty = new List<WyplataDialogElement>();

            //Pobiera listę płac z Optimy
            using (SqlCommand c = new SqlCommand("SELECT WPL_WplId, WPL_NumerString, [WPL_DataOd], [WPL_LplId] FROM [" + Data.Config.Firm.DB + "].[CDN].Wyplaty", Data.FirmCn))
            {
                using (SqlDataReader r = c.ExecuteReader())
                {
                    while (r.Read())
                    {
                        Wyp.Add(new WyplataDialogElement(r.GetString(1).Replace("@numerS", "*"), r.GetInt32(0).ToString(), Convert.ToDateTime(r[2]), Convert.ToInt32(r[3])));
                    }
                }
            }

            //usuwa powtórzenia
            foreach (WyplataDialogElement w in Wyp)
            {
                bool ca = true;

                foreach (WyplataDialogElement wy in Wypłaty)
                {
                    if (w.Text == wy.Text)
                        ca = false;
                }
                if (ca)
                {
                    Wypłaty.Add(w);
                }
            }

            //Pokazanie okna wyboru wypłat
            Windows.WyplataDialog cbd = new Windows.WyplataDialog("Wybierz wypłatę", Wypłaty);

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

            if (WybranaLista != null && WybranyTyp != null && LoadedFilePath != null && LoadedFilePath.Trim() != "")
            {
                Start.IsEnabled = true;
            }
        }

        /// <summary>
        /// Wraca do menu głównego
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Tools.Goto(new ImportPlik());
        }

        //public static List<PremFormat> Prems = new List<PremFormat>();


        /// <summary>
        /// Uruchamia proces importowania składników płac
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            

            if (File.Exists(LoadedFilePath))
            {
                try
                {
                    WczytajInterpretujPlik();
                    Tools.Goto(new Excel());
                }
                catch (Exception ex)
                {
                    Tools.Log($"Wyjątek dodawania składników\n{ex}\n");
                    if (ex.HResult == -2147024864 || ex.Message.StartsWith("Odmowa dostępu do ścieżki"))
                    {
                        MessageBox.Show(ex.Message, "Błąd wczytywania pliku");
                    }
                    else if (ex.HResult == -2147121671)
                    {
                        MessageBox.Show($"Podana wypłata została już zakmnięta\n{ex.Message}", "Nie można edytować zamkniętej wypłaty.");
                    }
                    else
                    {
                        MessageBox.Show($"Nie można otworzyć pliku\nUpewnij się, że to plik w formacie .xls lub .xlsx\n{ex.Message}", "Błąd wczytywania pliku");
                    }
                }
            }
            else
            {
                Tools.Log($"Plik nie istnieje");
                MessageBox.Show("Wybrany plik nie istnieje", "Błąd wczytywania pliku");
            }
        }
        public static IRow headerRow;
        public static Stream fs;
        /// <summary>
        /// Wczytuje plik i zwraca premie
        /// </summary>
        /// <returns></returns>
        private void WczytajInterpretujPlik()
        {
            Tools.Log("Otwieranie pliku");
            //Otwiera plik
            fs = new FileStream(LoadedFilePath, FileMode.Open);

            IWorkbook wb = WorkbookFactory.Create(fs);
            ISheet sheet = wb.GetSheetAt(0);
            headerRow = sheet.GetRow(0);

            /*
            Mapper importer = new Mapper(wb);

            Tools.Log("Wczytywanie danych");
            //Pobiera zawartość pliku
            var Items = importer.Take<PremFormat>(2).ToList();

            List<PremFormat> Prems = new List<PremFormat>();

            Tools.Log("Dodawanie listy składników");
            //Dodaje składniki do listy
            foreach (var x in Items)
            {
                PremFormat row = x.Value;
                if (row.Kwota != null && !row.Kwota.StartsWith("\"") && !row.Kwota.EndsWith("\""))
                {
                    Tools.Log(row.ToString());
                    Prems.Add(row);
                }
            }

            return Prems;
            */

        }
    }
}
