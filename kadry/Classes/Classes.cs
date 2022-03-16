using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kadry
{
    public class ComboBoxDialogElement
    {
        public ComboBoxDialogElement()
        {

        }
        public ComboBoxDialogElement(string Text, string ID)
        {
            this.Text = Text;
            Id = ID;
        }
        public string Text { get; set; }
        public string Id { get; set; }
    }

    public class WyplataDialogElement : ComboBoxDialogElement
    {
        public WyplataDialogElement() { }
        public WyplataDialogElement(string Text, string ID, DateTime Date, int lpid)
        {
            this.Text = Text;
            Id = ID;
            Rok = Date.Year.ToString();
            Data = Date;
            IdListyPłac = lpid;
        }
        public int IdListyPłac { get; set; }
        public DateTime? Data { get; set; }
        public string Rok { get; set; }
    }

    public class DekretDialogElement : WyplataDialogElement
    {
        public DekretDialogElement(
            string Text,
            string ID,
            DateTime Date,
            int lpid)
        {
            this.Text = Text;
            Id = ID;
            Rok = Date.Year.ToString();
            Data = Date;
            IdListyPłac = lpid;
        }

        public string DateText { get { return Data?.ToString("dd-MM-yyyy"); } }
    }

    public class Config
    {
        public string ServerSql { get; set; }
        public string Database { get; set; }
        public string UserSql { get; set; }
        public string PasswordSql { get; set; }

        public Firm Firm { get; set; }
    }

    public class Firm
    {
        public string Firma { get; set; }
        public string DB { get; set; }
        public string Serwer { get; set; }

    }
}
