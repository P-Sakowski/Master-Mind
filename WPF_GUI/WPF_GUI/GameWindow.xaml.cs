using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using ClassLibrary;

namespace WPF_GUI
{
    /// <summary>
    /// Logika interakcji dla klasy GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        readonly Game game = new Game();
        /// <summary>
        /// Konstruktor używany przy wczytaniu stanu gry, tworzy obiekt Game z pliku XML oraz wyświetla stosowne elementy GUI w zależności od wariantu gry
        /// Comboboxy są wypełnione nazwami lub numerami kolorów, w zależności od wybranego wariantu gry
        /// </summary>
        public GameWindow()
        {
            InitializeComponent();
            this.game = new Game(Game.DeSerializeObject<Game>("savegame.xml"));
            if (this.game.GameStatus != GameStatus.Pending){
                MessageBox.Show("Poprzednia gra została zakończona, rozpoczynam nową grę.");
                this.game = new Game();
            }
            if (game != null)
            {
                DataContext = this;
                this.listView.ItemsSource = this.game.AnswerList;
                if (this.game.GameType == 2)
                {
                    AnswerGrid.Columns[1].Width = 0;
                    AnswerGrid.Columns[2].Width = 100;
                }
                else
                {
                    AnswerGrid.Columns[1].Width = 268;
                    AnswerGrid.Columns[2].Width = 0;
                }
            }
            this.Move.Content = $"Ruch nr {this.game.MoveCounter + 1}";
            cmbField1.ItemsSource = this.game.GUI_FillComboBox(this.game.ColorsNumber);
            cmbField2.ItemsSource = this.game.GUI_FillComboBox(this.game.ColorsNumber);
            cmbField3.ItemsSource = this.game.GUI_FillComboBox(this.game.ColorsNumber);
            cmbField4.ItemsSource = this.game.GUI_FillComboBox(this.game.ColorsNumber);
            if (this.game.CodeLength > 4)
            {
                Label5.Visibility = Visibility.Visible;
                cmbField5.Visibility = Visibility.Visible;
                cmbField5.ItemsSource = this.game.GUI_FillComboBox(this.game.ColorsNumber);
            }
            if (this.game.CodeLength > 5)
            {
                Label6.Visibility = Visibility.Visible;
                cmbField6.Visibility = Visibility.Visible;
                cmbField6.ItemsSource = this.game.GUI_FillComboBox(this.game.ColorsNumber);
            }
        }
        /// <summary>
        /// Konstruktor 3-argumentowy używany przy tworzeniu nowej instancji gry
        /// Comboboxy są wypełnione nazwami lub numerami kolorów, w zależności od wybranego wariantu gry
        /// </summary>
        /// <param name="colors"> określa ile kolorów będzie używane w tej grze </param>
        /// <param name="length"> określa  długość odgadywanego ciągu znaków </param>
        /// <param name="gameType"> określa  typ gry, 1 dla opartej na kolorach, 2 dla opartej na cyfrach </param>
        public GameWindow(string colors, char length, int gameType)
        {
            InitializeComponent();
            this.game = new Game(int.Parse(colors.ToString()), int.Parse(length.ToString()), gameType);
            if (game != null)
            {
                DataContext = this;
                this.listView.ItemsSource = this.game.AnswerList;
                if(this.game.GameType == 2)
                {
                    AnswerGrid.Columns[1].Width = 0;
                    AnswerGrid.Columns[2].Width = 100;
                }
                else
                {
                    AnswerGrid.Columns[1].Width = 268;
                    AnswerGrid.Columns[2].Width = 0;
                }
            }
            this.Move.Content = $"Ruch nr {this.game.MoveCounter + 1}";
            cmbField1.ItemsSource = this.game.GUI_FillComboBox(this.game.ColorsNumber);
            cmbField2.ItemsSource = this.game.GUI_FillComboBox(this.game.ColorsNumber);
            cmbField3.ItemsSource = this.game.GUI_FillComboBox(this.game.ColorsNumber);
            cmbField4.ItemsSource = this.game.GUI_FillComboBox(this.game.ColorsNumber);
            if (this.game.CodeLength > 4)
            {
                Label5.Visibility = Visibility.Visible;
                cmbField5.Visibility = Visibility.Visible;
                cmbField5.ItemsSource = this.game.GUI_FillComboBox(this.game.ColorsNumber);
            }
            if (this.game.CodeLength > 5)
            {
                Label6.Visibility = Visibility.Visible;
                cmbField6.Visibility = Visibility.Visible;
                cmbField6.ItemsSource = this.game.GUI_FillComboBox(this.game.ColorsNumber);
            }
        }
        /// <summary>
        /// Powrót do menu
        /// </summary>
        private void Button_Menu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }
        /// <summary>
        /// Pobiera wartości z comboboxów, tworzy z nich tablicę kolorów, przesyła ją do metody Check celem sprawdzenia odpowiedzi, oraz wyświetla stosowny komunikat
        /// Jeśli gra została zakończona, unieaktywnia elementy interfejsu użytkownika umożliwiające dalsze sprawdzanie odpowiedzi, po czym pokazuje jaka była prawidłowa odpowiedź
        /// </summary>
        private void Button_Check_Click(object sender, RoutedEventArgs e)
        {
            ClassLibrary.Color[] answer = new ClassLibrary.Color[this.game.CodeLength];
            try
            {
                answer[0] = (ClassLibrary.Color)Enum.Parse(typeof(ClassLibrary.Color), this.cmbField1.SelectedItem.ToString());
                answer[1] = (ClassLibrary.Color)Enum.Parse(typeof(ClassLibrary.Color), this.cmbField2.SelectedItem.ToString());
                answer[2] = (ClassLibrary.Color)Enum.Parse(typeof(ClassLibrary.Color), this.cmbField3.SelectedItem.ToString());
                answer[3] = (ClassLibrary.Color)Enum.Parse(typeof(ClassLibrary.Color), this.cmbField4.SelectedItem.ToString());
                if (this.game.CodeLength > 4)
                {
                    answer[4] = (ClassLibrary.Color)Enum.Parse(typeof(ClassLibrary.Color), this.cmbField5.SelectedItem.ToString());
                }
                if (this.game.CodeLength > 5)
                {
                    answer[5] = (ClassLibrary.Color)Enum.Parse(typeof(ClassLibrary.Color), this.cmbField6.SelectedItem.ToString());
                }
                this.MoveCounterLabel.Content = this.game.Check(answer);
            }
            catch (Exception)
            {
                MessageBox.Show("Uzupełnij pola w swojej odpowiedzi");
            }
            ICollectionView view = CollectionViewSource.GetDefaultView(this.game.AnswerList);
            view.Refresh();
            if (game.GameStatus == GameStatus.Pending)
            {
                this.Move.Content = $"Ruch nr {this.game.MoveCounter + 1}";
            }
            else
            {
                this.Move.Content = "";
                this.CheckButton.IsEnabled = false;
                this.ScoreLabel.Content = $"Szukany ciąg: {this.game.GUI_ShowCorrectAnswer(this.game.TargetPattern)}";
            }
        }
    }
}