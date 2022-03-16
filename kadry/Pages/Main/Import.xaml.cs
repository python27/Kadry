using Microsoft.Win32;
using Npoi.Mapper;
using Npoi.Mapper.Attributes;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace kadry.Pages.Main
{
    /// <summary>
    /// Logika interakcji dla klasy Menu.xaml
    /// </summary>
    public partial class Menu : Page
    {
        public Menu()
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
                        Typy.Add(new Menu.SkType(Convert.ToInt32(r[0]), r[1].ToString(), Convert.ToInt32(r["TWP_AlgAlgorytm"])));
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
            Tools.Goto(new ImportExport());
        }

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
                    List<PremFormat> Prems = WczytajInterpretujPlik();

                    int i = 0;
                    int start = 0;

                    var errors = new List<string>();

                    string numerListyToDisplay = WybranaLista.NumerOpt.Replace("@numerS", "*");

                    while (i <= Prems.Count - 1)
                    {
                        GC.Collect();
                        Tools.Log($"Tworzenie sesji");
                        CDNBase.AdoSession Sesja = Data.Optima.Login.CreateSession();

                        for (i = start; i < Prems.Count; i++)
                        {
                            var pr = Prems[i];
                            Tools.Log($"Wczytywanie premii dla {pr.Pracownik}");

                            try
                            {
                                Tools.Log($"Szukanie pracownika");

                                //Generuje składnik płacy
                                var Pracownik = Sesja.CreateObject(
                                    "CDN.Pracownicy",
                                    "PRA_Kod = '" + pr.Pracownik + "'"
                                    );

                                Tools.Log($"szukanie składnika wypłaty {WybranyTyp.Nazwa} [{WybranyTyp.ID}]");

                                var rTypSkladnika = Sesja.CreateObject(
                                    "CDNPlCfg.TypySkladnikow",
                                    "TWP_TwpId = " + WybranyTyp.ID
                                    );

                                Tools.Log($"Sprawdzanie czy pracownik ma wystawioną wypłatę na liście {numerListyToDisplay}");

                                using (var command = new SqlCommand($"SELECT TOP (1) [WPL_WplId] FROM [{Data.DbName}].[CDN].[Wyplaty] where WPL_NumerString = '{WybranaLista.NumerOpt}' AND WPL_PraId = '{Pracownik.ID}'", Data.FirmCn))
                                using (var reader = command.ExecuteReader())
                                {
                                    if (!reader.Read())
                                    {
                                        Tools.Log($"Nie ma\n");

                                        errors.Add($"Pracownik o akronimie '{pr.Pracownik}' nie ma wystawionej wypłaty na liście '{numerListyToDisplay}'");
                                        continue;
                                    }
                                }

                                Tools.Log($"Otwieranie wypłaty dla pracownika o id {Pracownik.ID}");

                                var rWyplata = Sesja.CreateObject(
                                    "CDNWyplaty.Wyplaty",
                                    string.Format(
                                        "WPL_NumerString = '{0}' AND WPL_PraId = '{1}'",
                                        WybranaLista.NumerOpt,
                                        Pracownik.ID
                                        )
                                    );

                                Tools.Log($"Dodawanie nowego elementu wypłaty");

                                var rElement = rWyplata.Elementy.AddNew();

                                Tools.Log($"Ustawianie wypłaty na elemencie");

                                rElement.Wyplata = rWyplata;

                                Tools.Log($"Wyciąganie składnika wypłaty z elementu wypłaty");

                                var rSkladnik = rElement.Podstawowy;

                                Tools.Log($"Ustawianie okresu płatności na składniku");
                                rSkladnik.Okres.Period = rWyplata.Okres.Period;

                                Tools.Log($"Ustawianie typu składnika");
                                rSkladnik.Typ = rTypSkladnika;

                                Tools.Log($"Sprawdzanie czy typ składnika to potrącenie");
                                var potracenie = (int)rTypSkladnika.AlgWartosci.Potracenie;

                                var wartosc = Convert.ToDecimal(pr.Kwota);
                                if (potracenie == 1)
                                    wartosc *= -1;

                                //Uzupełnia wartość składnika

                                Tools.Log($"Uzupełnienie wartości składnika: {wartosc}");
                                if (WybranyTyp.Algorytm == 2 || WybranyTyp.Algorytm == 11)
                                {
                                    rSkladnik.Wartosc1 = wartosc;
                                }
                                else
                                {
                                    rElement.Wartosc = wartosc;
                                }

                                //rWyplata.PrzeliczWartosc();

                                Tools.Log($"Otwieranie listy wypłat");

                                var Lista = Sesja.CreateObject("CDNWyplaty.ListyPlac", $"[LPL_LplId] = '{WybranaLista.IdListyPłac}'");

                                Tools.Log($"Otwieranie obiektu do podliczania podatków");
                                var PodliczPodatki = Sesja.CreateObject("CDNWyplaty.PoliczPodatki");

                                Tools.Log($"Ustawianie pracownika w obiekcie do podliczania");
                                PodliczPodatki.Pracownik = Pracownik;

                                Tools.Log($"Ustawianie miesiąca deklaracji");
                                PodliczPodatki.MiesiacDeklaracji = Lista.MiesiacDeklaracji.MonthYear;

                                Tools.Log($"Ustawianie listy płac w obiekcie do podliczania");
                                PodliczPodatki.ListaPlac = Lista;

                                Tools.Log($"Ustawianie wypłaty w obiekcie do podliczania");
                                PodliczPodatki.Wyplata = rWyplata;

                                Tools.Log($"Przeliczanie podatków");
                                PodliczPodatki.Przelicz();

                                Tools.Log($"Przeliczanie wartości wypłaty");
                                rWyplata.PrzeliczWartosc();

                                //Lista.Wyplata = rWyplata;
                                if (i >= start + 20)
                                {
                                    Tools.Log($"Czyszczenie pamięci");
                                    start = i + 1;
                                    GC.Collect();
                                    GC.WaitForFullGCComplete();
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                var errorString = $"Wystąpił wyjątek podczas dodawania składników wypłaty dla '{pr.Pracownik}'\nWiadomość: {ex.Message}\nStack Trace{ex.StackTrace}";
                                Tools.Log(errorString);
                                errors.Add(errorString);
                            }
                        }

                        Tools.Log($"Zapisywanie sesji\n");
                        Sesja.Save();
                    }

                    if (errors.Count == 0)
                    {
                        MessageBox.Show("Lista składników została wczytana", "Pomyślnie wczytano");
                    }
                    else
                    {
                        new Windows.ErrorsList("Wczytano wypłaty", "Lista składników została wczytana.\nPodczas wczytywania wystąpiły pewne problemy.", errors).ShowDialog();
                    }
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

        /// <summary>
        /// Wczytuje plik i zwraca premie
        /// </summary>
        /// <returns></returns>
        private List<PremFormat> WczytajInterpretujPlik()
        {
            Tools.Log("Otwieranie pliku");
            //Otwiera plik
            Stream fs = new FileStream(LoadedFilePath, FileMode.Open);

            IWorkbook wb = WorkbookFactory.Create(fs);

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
        }

    }
}