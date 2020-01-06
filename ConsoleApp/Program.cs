using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;

namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            bool shouldStop = false;
            Game game;
            while (!shouldStop)
            {
                shouldStop = true;
                try
                {
                    Console.WriteLine("------------- GRA MASTER MIND ------------");
                    Console.WriteLine("Naciśnij 1 - Nowa Gra");
                    Console.WriteLine("Naciśnij 2 - Wczytaj Grę");
                    string x = Console.ReadLine();
                    int codeLength = 0;
                    int colors = 0;
                    int gameType = 0;
                    if (x == "1")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Nowa Gra: Naciśnij 1, by zagrać w standardową wersję gry z kolorami lub 2, by zagraćw wersję liczbową");
                        string input = Console.ReadLine();
                        if (!int.TryParse(input, out int valueee))
                        {
                            throw new ArgumentException("Podano nieprawidłową wartość");
                        }
                        else
                        {
                            gameType = int.Parse(input);
                            if (gameType < 1 || gameType > 2)
                            {
                                throw new ArgumentException("Podano nieprawidłową wartość");
                            }
                            else if(gameType == 1)
                            {
                                Console.WriteLine("Podaj długość kodu, który chcesz odgadywać: (dozwolona długość od 4 do 6 znaków)");
                                string foo = Console.ReadLine();
                                if (!int.TryParse(foo, out int value))
                                {
                                    throw new ArgumentException("Podano nieprawidłową wartość");
                                }
                                else
                                {
                                    codeLength = int.Parse(foo);
                                }
                                if (codeLength < 4 || codeLength > 6)
                                {
                                    throw new ArgumentException("Podano nieprawidłową wartość");
                                }
                                Console.WriteLine("Podaj ilość używanych kolorów: (dozwolone od 6 do 8 kolorów)");
                                string foobar = Console.ReadLine();
                                if (!int.TryParse(foobar, out int valuee))
                                {
                                    throw new ArgumentException("Podano nieprawidłową wartość");
                                }
                                else
                                {
                                    colors = int.Parse(foobar);
                                }
                                if (colors < 6 || colors > 8)
                                {
                                    throw new ArgumentException("Podano nieprawidłową wartość");
                                }
                                else if (colors == codeLength)
                                {
                                    throw new ArgumentException("W przypadku wybrania 6 kolorów, należy wybrać krótszą długość zgadywanego kodu");
                                }
                                game = new Game(colors, codeLength, 1);
                            }
                            else
                            {
                                game = new Game(2);
                            }
                        }
                        Console.WriteLine("Gra rozpoczęta");
                        game.CLI_Logic();
                    }
                    else if (x == "2")
                    {
                        game = Game.DeSerializeObject<Game>("savegame.xml");
                        if(game.GameStatus != GameStatus.Pending)
                        {
                            Console.WriteLine("Wczytana gra została już zakończona. Rozpoczynam nową grę...");
                            Console.WriteLine();
                            Console.WriteLine("Podaj długość kodu, który chcesz odgadywać: (dozwolona długość od 4 do 6 znaków)");
                            codeLength = int.Parse(Console.ReadLine());
                            if (codeLength < 4 || codeLength > 6)
                            {
                                throw new ArgumentException("Podano nieprawidłową wartość");
                            }
                            Console.WriteLine("Podaj ilość używanych kolorów: (dozwolone od 6 do 8 kolorów)");
                            colors = int.Parse(Console.ReadLine());
                            if (colors < 6 || colors > 8)
                            {
                                throw new ArgumentException("Podano nieprawidłową wartość");
                            }
                            else if (colors == codeLength)
                            {
                                throw new ArgumentException("W przypadku wybrania 6 kolorów, należy wybrać krótszą długość zgadywanego kodu");
                            }
                            game = new Game(colors, codeLength, 1);
                        }
                        else
                        {
                            Console.WriteLine("Gra wczytana");
                        }
                        game.CLI_ShowAnswerHistory();
                        game.CLI_Logic();
                    }
                    else
                    {
                        throw new ArgumentException("Podano nieprawidłową wartość");
                    }
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    shouldStop = false;
                }
            }
        }
    }
}


