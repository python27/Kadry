using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace kadry.Windows.Models
{
    class ErrorListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            PropertyHasChanged(propertyName);

            return true;
        }

        protected void PropertyHasChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private double messageHeight = 9999;

        public double MessageHeight {
            get => messageHeight;
            set {
                SetProperty(ref messageHeight, value);
                PropertyHasChanged(nameof(Row0Height));
            } 
        }
        public System.Windows.GridLength Row0Height => new System.Windows.GridLength(messageHeight + 10);

        private string message;

        public string Message { get => message; set => SetProperty(ref message, value); }

        private double errorsHeight = 9999;

        public double ErrorsHeight { get => errorsHeight; set => SetProperty(ref errorsHeight, value); }

        public System.Windows.GridLength Row1Height => new System.Windows.GridLength(errorsHeight);

        private string errors;

        public string Errors { get => errors; set => SetProperty(ref errors, value); }

    }
}
