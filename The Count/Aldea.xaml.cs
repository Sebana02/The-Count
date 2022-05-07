using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Gaming.Input;
using Windows.Graphics.Display;
using Windows.System;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
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
        bool attacking = false;
        ObservableCollection<Tropa> TroopsList = new ObservableCollection<Tropa>();
        ObservableCollection<Edificio> BuildsList = new ObservableCollection<Edificio>();
        DispatcherTimer constructTimer, trainTiemr;

        PointerPoint ptrPt;
        bool leftPressed;

        //mandos
        private readonly object myLock = new object();
        private List<Gamepad> myGamepads = new List<Gamepad>();
        public Gamepad mainGamepad = null;

        //lectura y escritura de los mandos
        private GamepadReading reading, preReading;
        private GamepadVibration vibration;

        const double DEADZONE = 0.1;

        //maneja el timer
        public DispatcherTimer GamePadTimer { get; private set; }
        private ContentControl FocusedTroop = null;

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

            addTroops();
            addBuildings();


            Gamepad.GamepadAdded += (object sender, Gamepad e) =>
            {
                lock (myLock)
                {
                    bool gamepadInList = myGamepads.Contains(e);
                    if (!gamepadInList)
                    {
                        myGamepads.Add(e);
                        mainGamepad = myGamepads[0];
                    }
                }
            };

            Gamepad.GamepadRemoved += (object sender, Gamepad e) =>
            {
                lock (myLock)
                {
                    int indexRemoved = myGamepads.IndexOf(e);
                    if (indexRemoved > -1)
                    {
                        myGamepads.RemoveAt(indexRemoved);
                        if (mainGamepad == myGamepads[indexRemoved])
                            mainGamepad = null;
                    }
                }
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            GamePadTimerSetup();
        }

        private void GamePadTimerSetup()
        {
            GamePadTimer = new DispatcherTimer();
            GamePadTimer.Tick += GamePadTimer_Tick;// dispatcherTimer_Tick;
            GamePadTimer.Interval = new TimeSpan(100000); //100000*10^-7s=1cs;
            GamePadTimer.Start();
        }

        private void GamePadTimer_Tick(object sender, object e)
        {
            var focus = FocusManager.GetFocusedElement();
            if (mainGamepad != null && focus == FocusedTroop && FocusedTroop!=null)
            {
                CompositeTransform t = (focus as ContentControl).RenderTransform as CompositeTransform;
                LeeMando(); //Lee GamePAd
                            //DetectaGestosMando(); //Detecta Gestos del Mando
                ZMMando(); //ZonaMuerta JoyStick y Triggers
                ActualizaIU(t); //Aplica cambios en IU y VM
                
            }
        }
        private void LeeMando()
        {
            if (mainGamepad != null)
            {
                preReading = reading;
                reading = mainGamepad.GetCurrentReading();
            }
        }

        private void ZMMando() {
            if (mainGamepad != null)
            {
                //eje x
                if (reading.RightThumbstickX > DEADZONE) reading.RightThumbstickX -= DEADZONE;
                else if (reading.RightThumbstickX < -DEADZONE) reading.RightThumbstickX += DEADZONE;
                else reading.RightThumbstickX = 0;

                //eje y
                if (reading.RightThumbstickY > DEADZONE) reading.RightThumbstickY -= DEADZONE;
                else if (reading.RightThumbstickY < -DEADZONE) reading.RightThumbstickY += DEADZONE;
                else reading.RightThumbstickY = 0;

            }
        }
        private void ActualizaIU(CompositeTransform t)
        {
            if (mainGamepad != null && t!=null)
            {
                t.TranslateX += (int)(reading.RightThumbstickX * 10);
                t.TranslateY -= (int)(reading.RightThumbstickY * 10);
            }
        }
        private void addTroops()
        {
            //reset an add tropas
            Tropa.id = 0;
            Tropa.NumeroTropa = 0;
            for (int i = 0; i < 10; i++)
            {
                //ListView de entrenamiento
                TroopsList.Add(new Tropa(100 * (i + 1)));

                //Tropas del canvas
                ContentControl c = new ContentControl();
                c.Content = new Image();
                (c.Content as Image).Source = new BitmapImage(new Uri("ms-appx:///Assets/tropa.png"));
                (c.Content as Image).MaxWidth = 100;
                c.Visibility = Visibility.Collapsed;
                c.IsTabStop = true;
                c.RenderTransform = new CompositeTransform();
                c.UseSystemFocusVisuals = true;
                c.ManipulationMode = ManipulationModes.All;
                c.PointerPressed += ContentControl_PointerPressed;
                c.PointerMoved += ContentControl_PointerMoved;
                c.PointerReleased += ContentControl_PointerReleased;
                c.KeyDown += ContentControl_KeyDown;
                c.GotFocus += ContentControl_GotFocus;
                TroopCanvas.Children.Add(c);
            }
        }

        private void ContentControl_GotFocus(object sender, RoutedEventArgs e)
        {
            FocusedTroop = sender as ContentControl;
        }
        private void ContentControl_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            ContentControl o = sender as ContentControl;
            CompositeTransform t = o.RenderTransform as CompositeTransform;

            switch (e.Key)
            {
                case VirtualKey.W:
                    t.TranslateY -= 3;
                    break;
                case VirtualKey.A:
                    t.TranslateX -= 3;
                    break;
                case VirtualKey.S:
                    t.TranslateY += 3;
                    break;
                case VirtualKey.D:
                    t.TranslateX += 3;
                    break;
                default:
                    break;

            }

        }
        private void ContentControl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ptrPt = e.GetCurrentPoint(MiMapa);
            if (ptrPt.Properties.IsLeftButtonPressed)
            {
                leftPressed = true;
            }
            
        }

        private void ContentControl_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint NewptrPt = e.GetCurrentPoint(MiMapa);
            ContentControl c = sender as ContentControl;
            CompositeTransform t = new CompositeTransform();

            if (leftPressed)
            {
                t.TranslateX = (int)NewptrPt.Position.X -20;
                t.TranslateY = (int)NewptrPt.Position.Y - 15;
                c.RenderTransform = t;
            }
        }

        private void ContentControl_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            ptrPt = e.GetCurrentPoint(MiMapa);
            if (!ptrPt.Properties.IsLeftButtonPressed) leftPressed = false;
        }
        private void addBuildings()
        {
            //reset an add tropas
            Edificio.id = 0;
            Edificio.NumeroEdificio = 0;
            for (int i = 0; i < 10; i++)
            {
                BuildsList.Add(new Edificio(100 * (i + 1)));
            }
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

        private void TrainEnhanceButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var sp = button.Parent as StackPanel;
            var id = int.Parse((sp.Children[0] as TextBlock).Text);
            TroopsList[id].addLevel();
            TroopCanvas.Children[id].Visibility = Visibility.Visible;
        }

        private void Build_Button(object sender, RoutedEventArgs e)
        {
            Construccion.Visibility = Visibility.Visible;
            Mejoras.Visibility = Visibility.Collapsed;
        }

        private void Improve_Button(object sender, RoutedEventArgs e)
        {
            Construccion.Visibility = Visibility.Collapsed;
            Mejoras.Visibility = Visibility.Visible;
        }

        private void AttackButton_Click(object sender, RoutedEventArgs e)
        {
            attacking = !attacking;

            var button = sender as Button;
            Busca.Visibility = attacking ? Visibility.Visible : Visibility.Collapsed;

            if (!attacking)
                (button.Content as Image).Source = new BitmapImage(new Uri("ms-appx:///Assets/Attack-Button.png"));
            else
                (button.Content as Image).Source = new BitmapImage(new Uri("ms-appx:///Assets/x.png"));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var sp = button.Parent as StackPanel;
            var id = int.Parse((sp.Children[0] as TextBlock).Text);
            BuildsList[id].addLevel();
        }

        private async void TroopCanvas_Drop(object sender, DragEventArgs e)
        {
            var id = await e.DataView.GetTextAsync();
            var TropaCanvas = TroopCanvas.Children[int.Parse(id)] as ContentControl;
            if (TropaCanvas.Visibility == Visibility.Collapsed) TropaCanvas.Visibility = Visibility.Visible;
            Point pos = e.GetPosition(MiMapa);
            CompositeTransform t = new CompositeTransform();
            t.TranslateX = pos.X - 60;
            t.TranslateY = pos.Y - 60;
            TropaCanvas.RenderTransform = t;

        }

        private void TroopCanvas_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Move;
        }

        private void Entrenar_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            Tropa item = e.Items[0] as Tropa;
            string id = item.Id.ToString();
            e.Data.SetText(id);
            e.Data.RequestedOperation = DataPackageOperation.Move;
        }
        private void OnListViewItemKeyDown(object sender, KeyRoutedEventArgs e)
        {
            // Code to handle going in/out of nested UI with gamepad and remote only.
            if (e.Handled == true)
            {
                return;
            }

            var focusedElementAsListViewItem = FocusManager.GetFocusedElement() as ListViewItem;
            if (focusedElementAsListViewItem != null)
            {
                // Focus is on the ListViewItem.
                // Go in with Right arrow.
                Control candidate = null;

                switch (e.OriginalKey)
                {
                    case Windows.System.VirtualKey.GamepadDPadRight:
                    case Windows.System.VirtualKey.GamepadLeftThumbstickRight:
                        var rawPixelsPerViewPixel = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
                        GeneralTransform generalTransform = focusedElementAsListViewItem.TransformToVisual(null);
                        Point startPoint = generalTransform.TransformPoint(new Point(0, 0));
                        Rect hintRect = new Rect(startPoint.X * rawPixelsPerViewPixel, startPoint.Y * rawPixelsPerViewPixel, 1, focusedElementAsListViewItem.ActualHeight * rawPixelsPerViewPixel);
                        candidate = FocusManager.FindNextFocusableElement(FocusNavigationDirection.Right, hintRect) as Control;
                        break;
                }

                if (candidate != null)
                {
                    candidate.Focus(FocusState.Keyboard);
                    e.Handled = true;
                }
            }
            else
            {
                // Focus is inside the ListViewItem.
                FocusNavigationDirection direction = FocusNavigationDirection.None;
                switch (e.OriginalKey)
                {
                    case Windows.System.VirtualKey.GamepadDPadUp:
                    case Windows.System.VirtualKey.GamepadLeftThumbstickUp:
                        direction = FocusNavigationDirection.Up;
                        break;
                    case Windows.System.VirtualKey.GamepadDPadDown:
                    case Windows.System.VirtualKey.GamepadLeftThumbstickDown:
                        direction = FocusNavigationDirection.Down;
                        break;
                    case Windows.System.VirtualKey.GamepadDPadLeft:
                    case Windows.System.VirtualKey.GamepadLeftThumbstickLeft:
                        direction = FocusNavigationDirection.Left;
                        break;
                    case Windows.System.VirtualKey.GamepadDPadRight:
                    case Windows.System.VirtualKey.GamepadLeftThumbstickRight:
                        direction = FocusNavigationDirection.Right;
                        break;
                    default:
                        break;
                }

                if (direction != FocusNavigationDirection.None)
                {
                    Control candidate = FocusManager.FindNextFocusableElement(direction) as Control;
                    if (candidate != null)
                    {
                        ListViewItem listViewItem = sender as ListViewItem;

                        // If the next focusable candidate to the left is outside of ListViewItem,
                        // put the focus on ListViewItem.
                        if (direction == FocusNavigationDirection.Left &&
                            !listViewItem.IsAncestorOf(candidate))
                        {
                            listViewItem.Focus(FocusState.Keyboard);
                        }
                        else
                        {
                            candidate.Focus(FocusState.Keyboard);
                        }
                    }

                    e.Handled = true;
                }
            }
        }

        private void listview1_ChoosingItemContainer(ListViewBase sender, ChoosingItemContainerEventArgs args)
        {
            if (args.ItemContainer == null)
            {
                args.ItemContainer = new ListViewItem();
                args.ItemContainer.KeyDown += OnListViewItemKeyDown;
            }
        }

        private void Boton_Construir_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var sp = button.Parent as StackPanel;
            var id = int.Parse((sp.Children[0] as TextBlock).Text);
            BuildsList[id].addConstruction();

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
        private void OnWindowSizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
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

    public static class DependencyObjectExtensions
    {
        public static bool IsAncestorOf(this DependencyObject parent, DependencyObject child)
        {
            DependencyObject current = child;
            bool isAncestor = false;

            while (current != null && !isAncestor)
            {
                if (current == parent)
                {
                    isAncestor = true;
                }

                current = VisualTreeHelper.GetParent(current);
            }

            return isAncestor;
        }
    }
}
