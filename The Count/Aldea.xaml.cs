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
        bool constructOpen = false, trainOpen = false;
        bool notifOut = false, chatOut = false;
        DispatcherTimer constructTimer, trainTiemr;
        public Aldea()
        {
            this.InitializeComponent();
            Window.Current.CoreWindow.SizeChanged += OnWindowSizeChanged;
            AddItemToEndChat("Hola!");
            Loaded += (s, e) =>
            {
                sp1.RenderTransform.SetValue(CompositeTransform.TranslateXProperty, sp1.ActualWidth);
                sp2.RenderTransform.SetValue(CompositeTransform.TranslateXProperty, -sp2.ActualWidth);
            };

           
        }

        private void TrainButton_Click(object sender, RoutedEventArgs e)
        {
            moveTrain();
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
        private void NewsButton_Click(object sender, RoutedEventArgs e)
        {
            notifOut = !notifOut;
            Notificaciones.Visibility = notifOut ? Visibility.Visible : Visibility.Collapsed;
            MenuBackground.Visibility = notifOut ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ChatButton_Click(object sender, RoutedEventArgs e)
        {
            chatOut = !chatOut;
            ChatGrid.Visibility = chatOut ? Visibility.Visible : Visibility.Collapsed;
        }
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void ChatBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                SendMessage();
            }
        }
        private void SendMessage()
        {
            string text = ChatBox.Text;
            if (text == "" || text == null) return;
            AddItemToEndChat(text);
            ChatBox.Text = "";
        }

        private void moveConstruct()
        {
            if (constructTimer != null) return;

            if (constructOpen)
            {
                constructTimer = new DispatcherTimer();
                constructTimer.Tick += moveConstructOut;
                constructTimer.Interval = new TimeSpan(100); //100000*10^-7s=1cs;
                constructTimer.Start();
            }
            else
            {
                constructTimer = new DispatcherTimer();
                constructTimer.Tick += moveConstructIn;
                constructTimer.Interval = new TimeSpan(100); //100000*10^-7s=1cs;
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
        private void moveTrain()
        {
            if (trainTiemr != null) return;

            if (trainOpen)
            {
                trainTiemr = new DispatcherTimer();
                trainTiemr.Tick += moveTrainOut;
                trainTiemr.Interval = new TimeSpan(100); //100000*10^-7s=1cs;
                trainTiemr.Start();
            }
            else
            {
                trainTiemr = new DispatcherTimer();
                trainTiemr.Tick += moveTrainIn;
                trainTiemr.Interval = new TimeSpan(100); //100000*10^-7s=1cs;
                trainTiemr.Start();
            }

            trainOpen = !trainOpen;
        }
        private void moveTrainOut(object sender, object e)
        {
            var pos = (double)sp2.RenderTransform.GetValue(CompositeTransform.TranslateXProperty);

            if (pos <= -sp2.ActualWidth)
            {
                trainTiemr.Stop();
                trainTiemr = null;

            }

            else
            {
                sp2.RenderTransform.SetValue(CompositeTransform.TranslateXProperty, pos - 5);
                TrainButton.RenderTransform.SetValue(CompositeTransform.TranslateXProperty, (double)TrainButton.RenderTransform.GetValue(CompositeTransform.TranslateXProperty) - 5);
                AttackButton.RenderTransform.SetValue(CompositeTransform.TranslateXProperty, (double)AttackButton.RenderTransform.GetValue(CompositeTransform.TranslateXProperty) - 5);
            }

        }


        private void moveTrainIn(object sender, object e)
        {
            var pos = (double)sp2.RenderTransform.GetValue(CompositeTransform.TranslateXProperty);

            if (pos >= 0)
            {
                trainTiemr.Stop();
                trainTiemr = null;

            }

            else
            {
                sp2.RenderTransform.SetValue(CompositeTransform.TranslateXProperty, pos + 5);
                TrainButton.RenderTransform.SetValue(CompositeTransform.TranslateXProperty, (double)TrainButton.RenderTransform.GetValue(CompositeTransform.TranslateXProperty) + 5);
                AttackButton.RenderTransform.SetValue(CompositeTransform.TranslateXProperty, (double)AttackButton.RenderTransform.GetValue(CompositeTransform.TranslateXProperty) + 5);
            }
        }
        private void OnWindowSizeChanged(object sender,Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            if (constructTimer != null)
            {
                constructTimer.Stop();
                constructTimer = null;

            }

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



            if (trainTiemr != null)
            {
                trainTiemr.Stop();
                trainTiemr = null;

            }
            if (trainOpen)
            {
                sp2.RenderTransform.SetValue(CompositeTransform.TranslateXProperty, 0);
                TrainButton.RenderTransform.SetValue(CompositeTransform.TranslateXProperty, sp2.ActualWidth);
                AttackButton.RenderTransform.SetValue(CompositeTransform.TranslateXProperty, sp2.ActualWidth);
            }
            else
            {
                sp2.RenderTransform.SetValue(CompositeTransform.TranslateXProperty, -sp2.ActualWidth);
                TrainButton.RenderTransform.SetValue(CompositeTransform.TranslateXProperty, 0);
                AttackButton.RenderTransform.SetValue(CompositeTransform.TranslateXProperty, 0);
            }
        }
        private void AddItemToEndChat(string msg)
        {
            ChatPanel.Items.Add(new Mensaje(msg, DateTime.Now, HorizontalAlignment.Right));
        }
    }
}
