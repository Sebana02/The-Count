using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace The_Count
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Aldea : Page
    {
        bool constructOpen = false;
        DispatcherTimer constructTimer;
        public Aldea()
        {
            this.InitializeComponent();
            Window.Current.CoreWindow.SizeChanged += (sender, e) =>
             {
                 if (constructTimer == null)
                 {
                     if (constructOpen)
                     {
                         sp1.RenderTransform.SetValue(CompositeTransform.TranslateXProperty, 0);
                         ContructButton.RenderTransform.SetValue(CompositeTransform.TranslateXProperty, -sp1.ActualWidth);
                     }
                     else
                     {
                         sp1.RenderTransform.SetValue(CompositeTransform.TranslateXProperty, sp1.ActualWidth);
                         ContructButton.RenderTransform.SetValue(CompositeTransform.TranslateXProperty, 0);
                     }
                 }
                 

             };

            Loaded += (s,e) =>
            {
                sp1.RenderTransform.SetValue(CompositeTransform.TranslateXProperty, sp1.ActualWidth);
            };
        }

        private void TrainButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Seleccion_Aldea));
        }

        private void ConfigButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(opciones));
        }

        private void ContructButton_Click(object sender, RoutedEventArgs e)
        {
           moveConstruct();
        }

        private void moveConstruct() {
            if (constructTimer != null) return;

            if (constructOpen)
            {
                constructTimer = new DispatcherTimer();
                constructTimer.Tick += moveConstructOut;
                constructTimer.Interval = new TimeSpan(1000); //100000*10^-7s=1cs;
                constructTimer.Start();
            }
            else
            {
                constructTimer = new DispatcherTimer();
                constructTimer.Tick += moveConstructIn;
                constructTimer.Interval = new TimeSpan(1000); //100000*10^-7s=1cs;
                constructTimer.Start();
            }

            constructOpen = !constructOpen;
        }
        private void moveConstructOut(object sender, object e)
        {
            var pos = (double)sp1.RenderTransform.GetValue(CompositeTransform.TranslateXProperty);

            if (pos >= sp1.ActualWidth)
            {
                constructTimer.Stop();
                constructTimer = null;
               
            }

            else
            {
                sp1.RenderTransform.SetValue(CompositeTransform.TranslateXProperty, pos + 5);
                ContructButton.RenderTransform.SetValue(CompositeTransform.TranslateXProperty, (double)ContructButton.RenderTransform.GetValue(CompositeTransform.TranslateXProperty) + 5);
            }

        }
        private void moveConstructIn(object sender, object e)
        {
            var pos = (double)sp1.RenderTransform.GetValue(CompositeTransform.TranslateXProperty);

            if (pos <= 0)
            {
                constructTimer.Stop();
                constructTimer = null;

            }

            else
            {
                sp1.RenderTransform.SetValue(CompositeTransform.TranslateXProperty, pos - 5);
                ContructButton.RenderTransform.SetValue(CompositeTransform.TranslateXProperty, (double)ContructButton.RenderTransform.GetValue(CompositeTransform.TranslateXProperty) - 5);
            }
        }
    }
}
