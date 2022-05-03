using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace The_Count
{
    public class Edificio : ObservableObject
    {
        public static int NumeroEdificio = 0;
        public string Nombre { get; private set; }

        private int precio_;
        public int Precio
        {
            get { return precio_; }
            set {Set(ref precio_, value);}
        }

        private int nivel = 1;

        private string nivel_;
        public string Nivel
        {
            get { return nivel_; }
            set { Set(ref nivel_, value); }
        }

        public static int id = 0;
        public int Id;

        public Edificio(int precio)
        {
            Nombre = $"Construcción {++NumeroEdificio}";
            Precio = precio;
            Nivel = $"Nivel {nivel}";
            Id = id++;

        }

        public void addLevel()
        {
            nivel++;
            Nivel = $"Nivel {nivel}";
            Precio += 50;
        }
    }
}
