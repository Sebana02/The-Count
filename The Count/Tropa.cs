using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace The_Count
{
    public class Tropa : INotifyPropertyChanged
    {
        private static int NumeroTropa = 0;
        public string Nombre { get; private set; }
        public int Precio { get; private set; }
        private int nivel = 0;
        public string Nivel { get; private set; }
        public string Estado { get; private set; }
        private static int id = 0;
        public int Id;

        public event PropertyChangedEventHandler PropertyChanged;
        private DispatcherTimer _timer;

        public Tropa(int precio)
        {
            Nombre = $"Tropa {++NumeroTropa}";
            Precio = precio;
            Nivel = $"Nivel {nivel}";
            Estado = "Comprar";
            Id = id++;

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.001) };

            _timer.Tick += (sender, o) =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Precio)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Nivel)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Estado)));
            };

            _timer.Start();
        }

        public void addLevel()
        {
            nivel++;
            Nivel = $"Nivel {nivel}";
            Precio += 50;
            Estado = "Mejorar";
        }
    }
}
