using CDNBase;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static kadry.Data.Optima;

namespace kadry
{
    public static partial class Tools
    {
        public static class Optima
        {

            public static bool Connect(string Operator, string Haslo)
            {

                //object[] hPar = new object[] { 1,  1,   0,  0,  1,   1,  0,    0,   0,   0,   0,   0,   1,   1,  1,   0,  0, 0 }; // do jakich modułów się logujemy
                /* Kolejno: KP, KH, KHP, ST, FA, MAG, PK, PKXL, CRM, ANL, DET, BIU, SRW, ODB, KB, KBP, HAP, CRMP
                 */
                SetOptimaDirectory();
                // katalog, gdzie jest zainstalowana Optima (bez ustawienia tej zmiennej nie zadziała, chyba że program odpalimy z katalogu O!)
                //System.Environment.CurrentDirectory = @"C:\Program Files (x86)\Comarch ERP Optima";//@"C:\Program Files\OPTIMA.NET";	

                // tworzymy nowy obiekt apliakcji
                Data.Optima.Application = new CDNBase.Application();

                // Jeśli proces nie ma dostępu do klucza w rejstrze 
                // HKCU\Software\CDN\CDN OPT!MA\CDN OPT!MA\Login\KonfigConnectStr
                // np. gdy pracuje jako aplikacji w IIS 
                // ciąg połączeniowy (ConnectString) podajemy bezpośrednio :
                // Application.KonfigConnectStr = "NET:CDN_KNF_Konfiguracja_DW,SERWERSQL,NT=0";

                string Firma = Data.Config.Firm.Firma;

                
                

                // blokujemy optimę
                Login = Data.Optima.Application.LockApp(255, 5000, null, null, null, null);
                //Login =  Application.LockApp(1, 5000, null, null, null, null);


                // logujemy się do podanej Firmy, na danego operatora, do podanych modułów
                // Login = Application.Login(Operator, Haslo, Firma);


                //  Logowanie z pobraniem ustawienia modułów z karty Operatora
                try
                {
                    Login = Data.Optima.Application.Login(Operator, Haslo, Firma, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Wprowadzone hasło nie jest poprawne"))
                    {
                        MessageBox.Show("Wprowadzone hasło nie jest poprawne", "Błąd logowania");
                    }
                    else
                    {
                        MessageBox.Show(ex.Message, "Błąd logowania");
                    }
                    return false;
                }


                return true;
            }

            private static void SetOptimaDirectory()
            {
                //Odczytanie katalogu O! następuje przez odwołanie do klucza w rejestrach dla zarejestrowanego obiektu sesji ADOSession
                var ru = new RegUtil();
                string comguid = ru.ReadFromRegistry(RegutilKeys.HKCR, "CDNBase.AdoSession\\CLSID", "", "").ToString();
                string optimaDir = ru.ReadFromRegistry(RegutilKeys.HKCR, "CLSID\\" + comguid + "\\InprocServer32", "", "").ToString();
                optimaDir = optimaDir.Substring(0, optimaDir.Length - 12);
                System.Environment.CurrentDirectory = optimaDir;
            }

            public static void Close()
            {
                if (Data.Optima.Application == null || Data.Optima.Login == null) return;

                Data.Optima.Login = null;
                // odblokowanie (wylogowanie) O!
                Data.Optima.Application.UnlockApp();
                // niszczymy obiekt Aplikacji
                Data.Optima.Application = null;
            }

            public enum RegutilKeys
            {
                HKCR = 0,
                HKCU = 1,
                HKLM = 2,
                HKU = 3,
                HKCC = 4
            }

            public class RegUtil
            {
                public const int REG_ERR_NULL_REFERENCE = 12;
                public const int REG_ERR_SECURITY = 13;
                public const int REG_ERR_UNKNOWN = 14;

                readonly char[] delimit = { '\x005c' };  //Hex for '\'

                public int WriteToReg(RegutilKeys Key, string RegPath, string KeyName, object KeyValue)
                {
                    string[] RegString = RegPath.Split(delimit);

                    //First item of array will be the base key,return REG_ERR_SECURITY; so be carefull iterating below
                    var RegKey = new RegistryKey[RegPath.Length + 1];

                    //Returns proper key --  Will Default to hkey_current_user
                    RegKey[0] = SelectKey(Key);

                    for (int i = 0; i < RegString.Length; i++)
                    {

                        RegKey[i + 1] = RegKey[i].OpenSubKey(RegString[i], true);
                        //If key does not exist, create it.  This logic usually suits my needs, but 
                        //you may change it if you wish.
                        if (RegKey[i + 1] == null)
                        {
                            RegKey[i + 1] = RegKey[i].CreateSubKey(RegString[i]);
                        }
                    }
                    try
                    {
                        //Write the value to the registry.  If fail, return the constant values defined at the beginning of the class
                        RegKey[RegString.Length].SetValue(KeyName, KeyValue);
                    }
                    catch (System.NullReferenceException)
                    {
                        return REG_ERR_NULL_REFERENCE;
                    }
                    catch (System.UnauthorizedAccessException)
                    {

                        return REG_ERR_SECURITY;
                    }

                    return 0;
                }//-----------  End WriteToReg  -----------------------------------------------------------------------------------------------
                /// <summary>
                /// 
                /// </summary>
                /// <param name="Key"> </param>
                /// <param name="RegPath"> </param>
                /// <param name="KeyName"> </param>
                /// <param name="DefaultValue"> </param>
                public object ReadFromRegistry(RegutilKeys Key, string RegPath, string KeyName, object DefaultValue)
                {
                    object result = null;
                    string[] regString = RegPath.Split(delimit);
                    //First item of array will be the base key, so be carefull iterating below
                    RegistryKey[] RegKey = new RegistryKey[RegPath.Length + 1];
                    //Returns proper key --  Will Default to HKEY_CURRENT_USER
                    RegKey[0] = SelectKey(Key);
                    for (int i = 0; i < regString.Length; i++)
                    {
                        RegKey[i + 1] = RegKey[i].OpenSubKey(regString[i]);
                        if (RegKey[i + 1] == null) //&& i < RegString.Length - 1 ) {
                            break;

                        if (i == regString.Length - 1)
                        {
                            result = (object)RegKey[i + 1].GetValue(KeyName, DefaultValue);
                        }
                    }
                    if (result == null)
                        result = DefaultValue;
                    return result;
                }  //  --------------  End ReadFromRegistry  --------------------------------------------------------------------------- 


                /// <summary>
                /// 
                /// </summary>
                /// <param name="Key"> </param>
                /// <param name="RegPath"> </param>
                public ArrayList GetEnumValFromRegistry(RegutilKeys Key, string RegPath)
                {
                    string[] regString = RegPath.Split(delimit);
                    var retValue = new ArrayList();

                    var regKey = new RegistryKey[RegPath.Length + 1];
                    regKey[0] = SelectKey(Key);

                    for (int i = 0; i < regString.Length; i++)
                    {
                        regKey[i + 1] = regKey[i].OpenSubKey(regString[i]); // zwraca null jeśli takiego klucza nie ma
                        if (regKey[i + 1] == null)
                            break;
                        if (i != regString.Length - 1) continue;
                        string[] s1 = regKey[i + 1].GetValueNames();
                        for (int j = 0; j < s1.Length; j++)
                            retValue.Add(regKey[i + 1].GetValue(s1[j]));
                    }

                    return retValue;
                }


                //Separated for cleanliness
                private RegistryKey SelectKey(RegutilKeys Key)
                {
                    switch (Key)
                    {
                        case RegutilKeys.HKCR:
                            return Registry.ClassesRoot;
                        case RegutilKeys.HKCU:
                            return Registry.CurrentUser;
                        case RegutilKeys.HKLM:
                            return Registry.LocalMachine;
                        case RegutilKeys.HKU:
                            return Registry.Users;
                        case RegutilKeys.HKCC:
                            return Registry.CurrentConfig;
                        default:
                            return Registry.CurrentUser;
                    }
                }
            }//  end Class 


        }
    }
}
