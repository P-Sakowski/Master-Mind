using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ClassLibrary
{
    public class Game
    {
        /// <summary>
        /// Licznik ruchów, inkrementowany przy sprawdzaniu odpowiedzi
        /// </summary>
        public int MoveCounter { get; set; }
        /// <summary>
        /// Status gry, wartości pobierane z enuma (W trakcie, zakończona - przegrana, zakończona - wygrana)
        /// </summary>
        public GameStatus GameStatus { get; set; }
        /// <summary>
        /// Tablica kolorów (enum) przechowująca docelowy wzór odgadywany przez gracza
        /// </summary>
        public Color[] TargetPattern { get; set; }
        /// <summary>
        /// Lista odpowiedzi (obiektów typu answer)
        /// </summary>
        public List<Answer> AnswerList;
        /// <summary>
        /// Długość zgadywanego kodu, parametr wejściowy podawany przez gracza
        /// </summary>
        public int CodeLength;
        /// <summary>
        /// Typ Gry: przyjmuje wartość 1 (domyślnie) gdy gra oparta na kolorach, 2 gdy gra oparta na liczbach
        /// </summary>
        public int GameType { get; set; }
        /// <summary>
        /// Określa limit ruchów dostępnych w danej grze: 9 dla gry opartej na kolorach, 15 dla gry opartej na cyfrach, ustawiane na podstawie wartości GameType
        /// </summary>
        public int MoveLimit { get; set; }
        /// <summary>
        /// Ilość możliwych kolorów, parametr wejściowy podawany przez gracza
        /// </summary>
        public int ColorsNumber;
        /// <summary>
        /// Obiekt przedstawiający odpowiedź udzielaną przez gracza, przechowywany w liście odpowiedzi
        /// </summary>
        public class Answer {
            /// <summary>
            /// Numer ruchu, w którym odzielono tej odpowiedzi
            /// </summary>
            public int MoveNumber { get; set; }
            /// <summary>
            /// Ilość prawidłowych kolorów odgadniętych w ramach tej odpowiedzi
            /// </summary>
            public int GoodAnswers { get; set; }
            /// <summary>
            /// Ciąg kolorów podany przez gracza w ramach tej odpowiedzi
            /// </summary>
            public Color[] AnswerPattern { get; set; }
            /// <summary>
            /// Metoda zwracająca tablicę kolorów w formie tekstowej, konieczna do wyświetlenia odpowiedzi w interfejsie GUI (ListView)
            /// </summary>
            public string AnswerArrayString
            {
                get
                {
                    var foo = "";
                    for (int i = 0; i < AnswerPattern.Length; i++)
                    {
                        if (i != AnswerPattern.Length - 1)
                        {
                            foo = foo + AnswerPattern[i] + ", ";
                        }
                        else
                        {
                            foo += AnswerPattern[i];
                        }
                    }
                    return foo;
                }
            }
            /// <summary>
            /// Metoda zwracająca tablicę liczb odpowiadających danym kolorom w formie tekstowej, konieczna do wyświetlenia odpowiedzi w interfejsie GUI (ListView)
            /// </summary>
            public string IntArrayString
            {
                get
                {
                    var foo = "";
                    for (int i = 0; i < AnswerPattern.Length; i++)
                    {
                        if (i != AnswerPattern.Length - 1)
                        {
                            foo = foo + (int)AnswerPattern[i] + ", ";
                        }
                        else
                        {
                            foo += (int)AnswerPattern[i];
                        }
                    }
                    return foo;
                }
            }
            /// <summary>
            /// Konstruktor najbardziej typowy
            /// </summary>
            /// <param name="goodAnswers"> ilość prawidłowych odpowiedzi </param>
            /// <param name="moveNumber"> numer ruchu </param>
            /// <param name="answer"> tablica z kolorami - odpowiedź gracza </param>
            public Answer(int goodAnswers, int moveNumber, Color[] answer)
            {
                this.GoodAnswers = goodAnswers;
                this.MoveNumber = moveNumber;
                this.AnswerPattern = answer;
            }
            /// <summary>
            /// konstruktor bezargumentowy, domyślny
            /// Zalożenie: podstawowy wariant rozgrywki: odgadujemy ciąg długości 4 znaków, 6 kolorów do wyboru
            /// </summary>
            public Answer()
            {
                this.GoodAnswers = 0;
                this.MoveNumber = MoveNumber;
                this.AnswerPattern = new Color[4];
            }
            /// <summary>
            /// Tekstowe przedstawienie całego obiektu Answer
            /// </summary>
            /// <returns> string z wszystkimi elementami obiektu </returns>
            public override string ToString()
            {
                string foo = $"{this.MoveNumber}. ";
                for (int i = 0; i < this.AnswerPattern.Length; i++)
                {
                    if (i != this.AnswerPattern.Length - 1)
                    {
                       foo = foo + this.AnswerPattern[i] + ", ";
                    }
                    else
                    {
                        foo += this.AnswerPattern[i];
                    }
                }
                foo += $", Prawidłowych: {this.GoodAnswers}";
                return foo;
            }
        }
        /// <summary>
        /// konstruktor bezargumentowy, domyślny
        /// Zakłada podstawowy wariant rozgrywki - odgadujemy ciąg długości 4 znaków, 6 kolorów do wyboru, wariant gry używający nazw kolorów
        /// </summary>
        public Game()
        {
            this.GameStatus = GameStatus.Pending;
            this.TargetPattern = new Color[4];
            this.ColorsNumber = 6;
            this.MoveCounter = 0;
            this.TargetPattern = Generate(TargetPattern, ColorsNumber);
            this.AnswerList = new List<Answer>();
            this.CodeLength = 4;
            this.GameType = 1;
            this.MoveLimit = 9;
        }
        /// <summary>
        /// Konstruktor 3-argumentowy, pozwalający graczowi wybrać dłogość odgadywanego ciągu i ilość kolorów
        /// </summary>
        /// <param name="colors"> ilość kolorów dostępna w rozgrywce </param>
        /// <param name="codeLength"> długość odgadywanego kodu </param>
        /// <param name="gameType"> typ gry, 1 gdy oparta na kolorach, 2 gdy oparta na liczbach </param>
        public Game(int colors, int codeLength, int gameType)
        {
            this.GameStatus = GameStatus.Pending;
            this.TargetPattern = new Color[codeLength];
            this.ColorsNumber = colors;
            this.MoveCounter = 0;
            this.TargetPattern = Generate(TargetPattern, ColorsNumber);
            this.AnswerList = new List<Answer>();
            this.CodeLength = codeLength;
            this.GameType = gameType;
            if(this.GameType == 1)
            {
                this.MoveLimit = 9;
            }
            else
            {
                this.MoveLimit = 15;
            }
        }
        /// <summary>
        /// Konstruktor 1-argumentowy: jeśli typ gry jest liczbowy, to jasne jest, że CodeLength=4 oraz ColorsNumber-10, więc nie trzeba tego parametryzować
        /// </summary>
        /// <param name="gameType">  typ gry, 1 gdy oparta na kolorach, 2 gdy oparta na liczbach  </param>
        public Game(int gameType)
        {
            this.GameStatus = GameStatus.Pending;
            this.TargetPattern = new Color[4];
            this.ColorsNumber = 10;
            this.MoveCounter = 0;
            this.TargetPattern = Generate(TargetPattern, ColorsNumber);
            this.AnswerList = new List<Answer>();
            this.CodeLength = 4;
            this.GameType = gameType;
            if (this.GameType == 1)
            {
                this.MoveLimit = 9;
            }
            else
            {
                this.MoveLimit = 15;
            }
        }
        /// <summary>
        /// Konstruktor wykorzystywany przez metodę deserializującą
        /// </summary>
        /// <param name="game"> obiekt game utworzony z pliku XML </param>
        public Game(Game game)
        {
            this.GameStatus = game.GameStatus;
            this.TargetPattern = game.TargetPattern;
            this.MoveCounter = game.MoveCounter;
            this.AnswerList = game.AnswerList;
            this.ColorsNumber = game.ColorsNumber;
            this.CodeLength = game.CodeLength;
            this.GameType = game.GameType;
            this.MoveLimit = game.MoveLimit;
        }
        /// <summary>
        /// Metoda wypełniająca pustą tablicę kolorów, kolorami z zakresu podanego przy inicjalizacji nowej gry
        /// </summary>
        /// <param name="arr"> pysta tablica kolorów </param>
        /// <param name="maxValue"> maksymalny używany numer koloru </param>
        /// <returns> tablica wypełniona wartościami enuma Color </returns>
        public Color[] Generate(Color[] arr, int maxValue)
        {
            var values = Enum.GetValues(typeof(Color));
            Random random = new Random();
            for (int index = 0; index < arr.Length; index++)
            {
                arr[index] = (Color)values.GetValue(random.Next(maxValue));
            }
            return arr;
        }
        /// <summary>
        /// Metoda sprawdająca odpowiedź. Zwiększa licznik ruchów, liczy prawidłowo odgadnięte kolory, umieszcza odpowiedź na liście odpowiedzi (wywołuje metodę PushAnswerToList)
        /// </summary>
        /// <param name="arr"> tablica kolorów- odpowiedź gracza </param>
        /// <returns> wiadomość zwrotna z oceną odpowiedzi </returns>
        public string Check(Color[] arr)
        {
            this.MoveCounter++;
            string message;
            int goodAnswerCounter = 0;
            for (int index = 0; index < arr.Length; index++)
            {
                if (arr[index] == this.TargetPattern[index])
                {
                    goodAnswerCounter++;
                }
            }
            if(goodAnswerCounter == this.CodeLength && this.MoveCounter <=this.MoveLimit)
            {
                this.GameStatus = GameStatus.FinishedWithWin;
                message = $"WYGRAŁEŚ! Wykorzystałeś: {this.MoveCounter} ruchów";
                PushAnswerToList(goodAnswerCounter, MoveCounter, arr);
            }
            else if (this.MoveCounter >= this.MoveLimit)
            {
                this.GameStatus = GameStatus.FinishedWithLose;
                message = $"PRZEGRAŁEŚ, pozostało: {this.MoveLimit-this.MoveCounter} ruchów";
                PushAnswerToList(goodAnswerCounter, MoveCounter, arr);
            }
            else
            {
                message = $"GRAMY DALEJ, zgadłeś {goodAnswerCounter} elementów, pozostało: {this.MoveLimit-this.MoveCounter} ruchów";
                PushAnswerToList(goodAnswerCounter, MoveCounter, arr);
            }
            return message;
        }
        /// <summary>
        /// Metoda tworząca obiekt Answer na podstawie otrzymanych parametrów i umieszczająca go na liście odpowiedzi (obiektów Answer)
        /// </summary>
        /// <param name="goodAnswers"> ilość prawidłowo odgadniętych kolorów </param>
        /// <param name="moveNumber"> numer ruchu </param>
        /// <param name="answer"> tablica z kolorami - odpowiedź gracza </param>
        public void PushAnswerToList(int goodAnswers, int moveNumber, Color[] answer)
        {
            this.AnswerList.Add(new Answer(goodAnswers, moveNumber, answer));
            this.SerializeObject<Game>(this, "savegame.xml");
        }
        /// <summary>
        /// Metoda zapisująca stan gry - serializuje obiekt game do pliku 
        /// </summary>
        /// <typeparam name="T"> typ serializowanego obiektu - w tym przypadku zawsze Game </typeparam>
        /// <param name="serializableObject"> obiekt zapisywany - konkretna instancja gry </param>
        /// <param name="fileName"> nazwa pliku docelowego </param>
        /// <returns> plik XML w którym zapisana jest instancja obiektu Game oraz wiadomość zwrotna </returns>
        public string SerializeObject<T>(T serializableObject, string fileName)
        {
            string message;
            if (serializableObject == null) { return "Nie można zapisać w tej chwili"; }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fileName);
                }
                message = "Stan gry zapisano pomyślnie";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return message;
        }
        /// <summary>
        /// Metoda wczytująca stan gry - tworzy obiekt game z pliku 
        /// </summary>
        /// <typeparam name="T"> typ deserializowanego obiektu - w tym przypadku zawsze Game </typeparam>
        /// <param name="fileName"> nazwa pliku z którego obiekt jest tworzony </param>
        /// <returns> obiekt typu Game </returns>
        public static T DeSerializeObject<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return default; }

            T objectOut = default;

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(T);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (T)serializer.Deserialize(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return objectOut;
        }
        /// <summary>
        /// Metoda odpowiadająca za konsolowy interfejs użytkownika 
        /// </summary>
        public void CLI_Logic()
        {
            while (this.GameStatus != GameStatus.FinishedWithLose && this.GameStatus != GameStatus.FinishedWithWin)
            {
                Color[] answer = new Color[this.CodeLength];
                if(this.GameType == 1)
                {
                    Console.WriteLine("Legenda: ");
                    foreach (Color foo in Enum.GetValues(typeof(Color)))
                    {
                        if ((int)foo < this.ColorsNumber)
                        {
                            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), foo.ToString());
                            Console.WriteLine($"{(int)foo} - {foo}");
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine("Wpisz cyfrę odpowiadającą danemu elementowi");
                for (int i = 0; i < this.TargetPattern.Length; i++)
                {
                    int foo; 
                    bool shouldStop = false;
                    Console.WriteLine($"Zgadnij co znajduje się w polu nr: {i + 1}");
                    while (!shouldStop)
                    {
                        shouldStop = true;
                        try
                        {
                            string foobar = Console.ReadLine();
                            if (!int.TryParse(foobar, out int valuee))
                            {
                                throw new ArgumentException("Podano nieprawidłową wartość");
                            }
                            else
                            {
                                foo = int.Parse(foobar);
                            }
                            if (foo < 0 || foo >= this.ColorsNumber)
                            {
                                throw new ArgumentException($"Błędna wartość, podaj wartość pola nr {i + 1} jeszcze raz");
                            }
                            else
                            {
                                answer[i] = (Color)foo;
                            }
                        }
                        catch (Exception ex)
                        {
                            shouldStop = false;
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                Console.Write("Twoja odpowiedź: ");
                CLI_ShowColoredValues(answer, this.GameType);
                Console.WriteLine();
                Console.WriteLine(Check(answer));
            }
            Console.WriteLine($"Prawidłowa odpowiedź:");
            CLI_ShowColoredValues(this.TargetPattern, this.GameType);
            Console.WriteLine();
        }
        /// <summary>
        /// Metoda wyświetlająca w konsoli odpowiednio pokolorowane elementy tablicy kolorów
        /// </summary>
        /// <param name="arr"> tablica kolorów </param>
        /// <param name="gameType">   typ gry, 1 gdy oparta na kolorach, 2 gdy oparta na liczbach </param>
        public void CLI_ShowColoredValues(Color[] arr, int gameType)
        {
            if (gameType == 1)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), arr[i].ToString());
                    if (i != arr.Length - 1)
                    {
                        Console.Write(arr[i] + ", ");
                    }
                    else
                    {
                        Console.Write(arr[i]);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
            else
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), arr[i].ToString());
                    if (i != arr.Length - 1)
                    {
                        Console.Write((int)arr[i] + ", ");
                    }
                    else
                    {
                        Console.Write((int)arr[i]);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
        }
        /// <summary>
        /// Metoda używana po wczytaniu gry, wyświetlająca historię udzielonych odpowiedzi wraz z kolorowaniem
        /// </summary>
        public void CLI_ShowAnswerHistory()
        {
            foreach (var item in this.AnswerList)
            {
                Console.Write($"Odpowiedź nr {item.MoveNumber}, ");
                CLI_ShowColoredValues(item.AnswerPattern, this.GameType);
                Console.WriteLine($", Prawidłowych odpowiedzi: {item.GoodAnswers}");
            }
        }
        /// <summary>
        /// Metoda wypełniająca pola Comboboxa wartościami enuma Color lub liczbami, w zależności od typu gry
        /// </summary>
        /// <param name="colors"> Ilość kolorów która ma zostać wprowadzona do enuma (wybierana przy inicjalizacji nowej gry) </param>
        /// <returns> tablica zawierająca stosowną ilość wartości enuma Color </returns>
        public Array GUI_FillComboBox(int colors)
        {
            if (this.GameType == 1)
            {
                Color[] arr = (Color[])Enum.GetValues(typeof(Color));
                Array.Resize(ref arr, arr.Length - 2);
                if (colors < 8)
                {
                    Array.Resize(ref arr, arr.Length - 1);
                }
                if (colors < 7)
                {
                    Array.Resize(ref arr, arr.Length - 1);
                }
                return arr;
            }
            else
            {
                int[] arr = new int[10];
                for (int i = 0; i < 10; i++)
                {
                    arr[i] = i;
                }
                return arr;
            }
        }
        /// <summary>
        /// Zwraca prawidłową odpowiedź, metoda bliźniacza do CLI_ShowColoredValues, ale bez kolorowania tekstu na konsoli
        /// </summary>
        /// <param name="arr"> tablica kolorów </param>
        /// <returns> tekstowa forma tablicy kolorów </returns>
        public string GUI_ShowCorrectAnswer(Color[] arr)
        {
            string foo = "";
            if (this.GameType == 1)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    if (i != arr.Length - 1)
                    {
                        foo += (arr[i] + ", ");
                    }
                    else
                    {
                        foo += arr[i];
                    }
                }
            }
            else
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    if (i != arr.Length - 1)
                    {
                        foo += ((int)arr[i] + ", ");
                    }
                    else
                    {
                        foo += (int)arr[i];
                    }
                }
            }
            return foo;
        }
    }
}