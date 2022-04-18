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
    public sealed partial class Seleccion_aspecto : Page
    {
        public SkinLogic skinLogic { get; } = new SkinLogic();

        public Seleccion_aspecto()
        {
            this.InitializeComponent();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private void ChangeSel(object sender, ItemClickEventArgs e)
        {
            skinLogic.aspectos[skinLogic.sel].color = new SolidColorBrush(Colors.White);
            skinLogic.sel = (e.ClickedItem as SkinDesc).id;
            skinLogic.aspectos[skinLogic.sel].color = new SolidColorBrush(Colors.Green);            
        }
    }
}
