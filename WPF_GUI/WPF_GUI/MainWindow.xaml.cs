using System;
using System.Windows;

namespace WPF_GUI
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Button_NewGame_Click(object sender, RoutedEventArgs e)
        {
            ColorLabel.Visibility = Visibility.Visible;
            LengthLabel.Visibility = Visibility.Visible;
            StartButton.Visibility = Visibility.Visible;
            ComboColor.Visibility = Visibility.Visible;
            ComboLength.Visibility = Visibility.Visible;
            Checkbox.Visibility = Visibility.Visible;
            NewGameButton.Visibility = Visibility.Hidden;
            LoadGameButton.Visibility = Visibility.Hidden;
            ExitGameButton.Visibility = Visibility.Hidden;
        }

        private void Button_LoadGame_Click(object sender, RoutedEventArgs e)
        {
            GameWindow window = new GameWindow();
            window.Show();
            this.Close();
        }
        /// <summary>
        /// Przy naciśnięciu przycisku rozpoczynającego nową grę, sprawdzane jest, czy ilość kolorów jest równa długości zgadywanego ciągu. Jeśli tak, należy podać inne wartości
        /// </summary>
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if(Checkbox.IsChecked == false)
            {
                if (ComboColor.SelectedItem.ToString()[ComboColor.SelectedItem.ToString().Length - 1] == ComboLength.SelectedItem.ToString()[ComboLength.SelectedItem.ToString().Length - 1])
                {
                    MessageBox.Show("W przypadku wybrania 6 kolorów, należy wybrać krótszy odgadywany ciąg znaków");
                }
                else
                {
                    GameWindow window = new GameWindow(ComboColor.SelectedItem.ToString()[ComboColor.SelectedItem.ToString().Length - 1].ToString(), ComboLength.SelectedItem.ToString()[ComboLength.SelectedItem.ToString().Length - 1], 1);
                    window.Show();
                    this.Close();
                }
            }
            else
            {
                GameWindow window = new GameWindow("10",'4',2);
                window.Show();
                this.Close();
            }
        }
        private void Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            ColorLabel.Visibility = Visibility.Hidden;
            LengthLabel.Visibility = Visibility.Hidden;
            ComboColor.Visibility = Visibility.Hidden;
            ComboLength.Visibility = Visibility.Hidden;
        }
        private void Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            ColorLabel.Visibility = Visibility.Visible;
            LengthLabel.Visibility = Visibility.Visible;
            ComboColor.Visibility = Visibility.Visible;
            ComboLength.Visibility = Visibility.Visible;
        }
    }
}
