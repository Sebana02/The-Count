using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace The_Count
{
    public class Skin: ObservableObject
    {
        public int id { get; private set; }
        public string nombre { get; private set; }
        public string image { get; private set; }

        private SolidColorBrush color_;
        public SolidColorBrush color
        {
            get { return color_; }
            set { Set(ref color_, value); }
        }

        public Skin(int id_,string image_,string nombre_,SolidColorBrush color_) {
            id = id_;
            nombre = nombre_;
            image = image_;
            color = color_;
        }
    }
    public class skins
    {
        public ObservableCollection<Skin> skinList { get; private set; }

        public skins()
        {
            skinList = new ObservableCollection<Skin>()
            {
                new Skin(0,"Assets\\skin2.png","Aspecto 1",new SolidColorBrush(Colors.Green)),
                new Skin(1,"Assets\\skin2.png","Aspecto 2",new SolidColorBrush(Colors.White)),
                new Skin(2,"Assets\\skin2.png","Aspecto 3",new SolidColorBrush(Colors.White)),
                new Skin(3,"Assets\\skin2.png","Aspecto 4",new SolidColorBrush(Colors.White)),
                new Skin(4,"Assets\\skin2.png","Aspecto 5",new SolidColorBrush(Colors.White)),
                new Skin(5,"Assets\\skin2.png","Aspecto 6",new SolidColorBrush(Colors.White)),
                new Skin(6,"Assets\\skin2.png","Aspecto 7",new SolidColorBrush(Colors.White)),
                new Skin(7,"Assets\\skin2.png","Aspecto 8",new SolidColorBrush(Colors.White)),
                new Skin(8,"Assets\\skin2.png","Aspecto 9",new SolidColorBrush(Colors.White)),
                new Skin(9,"Assets\\skin2.png","Aspecto 10",new SolidColorBrush(Colors.White)),
            };
        }
    }
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class Seleccion_aspecto : Page
    {
        private int sel = 0;

        public skins skins = new skins();

        public Seleccion_aspecto()
        {
            this.InitializeComponent();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private  void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listV = sender as ListView;
            skins.skinList[sel].color = new SolidColorBrush(Colors.White);
            sel = (listV.SelectedItem as Skin).id;
            skins.skinList[sel].color = new SolidColorBrush(Colors.Green);
        }
    }
}
