using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace The_Count
{
    public class Tropa : ObservableObject
    {
        private static int NumeroTropa = 0;
        public string Nombre { get; private set; }

        private int precio_;
        public int Precio
        {
            get { return precio_; }
            set {Set(ref precio_, value);}
        }

        private int nivel = 0;

        private string nivel_;
        public string Nivel
        {
            get { return nivel_; }
            set { Set(ref nivel_, value); }
        }

        private string estado_;
        public string Estado
        {
            get { return estado_; }
            set { Set(ref estado_, value); }
        }

        private static int id = 0;
        public int Id;

        public Tropa(int precio)
        {
            Nombre = $"Tropa {++NumeroTropa}";
            Precio = precio;
            Nivel = $"Nivel {nivel}";
            Estado = "Comprar";
            Id = id++;

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
