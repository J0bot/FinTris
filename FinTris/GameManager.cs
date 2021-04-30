///ETML
///Auteur   	: José Carlos Gasser, Ahmad Jano, Maxime Andrieux, Maxence Weyermann, Larissa Debarros
///Date     	: 19.03.2021
///Description  : Fintris

using Figgle;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Media;

namespace FinTris
{
    public static class GameManager
    {
        /// <summary>
        /// Attribut Game de la classe Program
        /// </summary>
        private static Game _game; //######################PS: j'ai changé en public pour pouvoir avoir les scores, y'a t-il un meilleur moyen? maxime

        /// <summary>
        /// Attribut GameRenderer de la classe Program
        /// </summary>
        private static GameRenderer _gameRenderer;

        /// <summary>
        /// Fonction qui s'occupe du Menu
        /// </summary>
        public static void MainMenu()
        {
            SoundPlayer themeSound = new SoundPlayer("tetrisSoundTheme.wav");
            themeSound.PlayLooping();
           

            Menu _menu = new Menu(FiggleFonts.Starwars.Render("FinTris"));

            MenuEntry play = new MenuEntry("Play");
            MenuEntry options = new MenuEntry("Options");
            MenuEntry playerName = new MenuEntry("Player name: ", Config.PlayerName);
            MenuEntry quit = new MenuEntry("Quit");

            _menu.Add(play);
            _menu.Add(options);
            _menu.Add(playerName);
            _menu.Add(quit);

            MenuEntry choice = _menu.ShowMenu();

            if (choice == play)
            {
                Play();
                
            }
            else if (choice == options)
            {
                ShowOptions();
            }
            if (choice == playerName)
            {
                Config.PlayerName = AskForInput();
                //this is probably disgusting but I'll do it anyway
                MainMenu();
            }
            else
            {
                Environment.Exit(0);
            }

        }

        /// <summary>
        /// Méthode pour choisir un nouveau nom. Assez banal pour l'instant.
        /// </summary>
        /// <returns>Le nouveau nom du joueur</returns>
        public static string AskForInput()
        {
            string askNewName = "Enter a new name: ";
            Console.Clear();
            Console.CursorLeft = (Console.BufferWidth / 2) - askNewName.Length / 2;
            Console.CursorTop = (Console.BufferHeight / 2);
            Console.Write(askNewName);
            string entry = Console.ReadLine();
            
            return entry;
        }

        /// <summary>
        /// Shows the options panel.
        /// </summary>
        public static void ShowOptions()
        {
            Menu optionMenu = new Menu("Options");

            MenuEntry bestScores = new MenuEntry("Show best scores");
            MenuEntry difficulty = new MenuEntry("Difficulty: ", Config.DifficultyLevel);
            MenuEntry cancel = new MenuEntry("Return");
            optionMenu.Add(bestScores);
            optionMenu.Add(difficulty);
            optionMenu.Add(cancel);
            MenuEntry choice;
            do
            {
       
                choice = optionMenu.ShowMenu();

                if (choice == bestScores)
                {
                    ShowBestScores();
                }
                if (choice == difficulty)
                {
                    SelectDifficulty();
                }
                else if (choice == cancel)
                {
                    SoundCancel();
                    MainMenu(); //huuuuh
                }
            } while (choice != cancel);

        }

        /// <summary>
        /// Shows the best scores.
        /// </summary>
        public static void ShowBestScores()
        {
            Console.Clear();
            List<string[]> scores = Config.GetBestScores();
            Console.CursorTop = (Console.BufferHeight / 2) - (scores.Count);

            foreach (string[] entry in scores)
            {
                //why is there a space between the scores???
                Console.CursorTop += 1;
                Console.CursorLeft = (Console.BufferWidth / 2) - entry[0].Length - 2;
                Console.Write(entry[0]);
                Console.CursorLeft = (Console.BufferWidth / 2) + 2;
                Console.WriteLine(entry[1]);
            }
            Console.ReadLine();
        }


        public static void SelectDifficulty()
        {
            Menu optionMenu = new Menu("Difficulty levels");
            MenuEntry diffEasy = new MenuEntry("Easy");
            MenuEntry diffNormal = new MenuEntry("Normal");
            MenuEntry diffHard = new MenuEntry("Hard");
            optionMenu.Add(diffEasy);
            optionMenu.Add(diffNormal);
            optionMenu.Add(diffHard);
            MenuEntry choice = optionMenu.ShowMenu();

            if (choice == diffEasy)
            {
                Config.DifficultyLevel = "Easy";
            }
            else if (choice == diffNormal)
            {
                Config.DifficultyLevel = "Normal";
            }
            else if (choice == diffHard)
            {
                Config.DifficultyLevel = "Hard";
            }
            ShowOptions();
        }

        /// <summary>
        /// Méthode play permet de lancer tous les éléments du jeu et de reset le jeu
        /// </summary>
        public static void Play()
        {
            SoundPlayer okSound = new SoundPlayer("tetrisSoundOK.wav");
            Stopwatch stopWatch = new Stopwatch();

            okSound.Play();
            stopWatch.Start();
            do
            {
                Console.Clear();

            } while (stopWatch.Elapsed.TotalSeconds < 1);

            stopWatch.Stop();

            Console.Clear();

            SoundReady();

            stopWatch.Restart();
            do
            {
                do
                {
                    Console.SetCursorPosition(0, 5);
                    Console.WriteLine(FiggleFonts.Starwars.Render("Ready"));

                } while (stopWatch.Elapsed.TotalSeconds < 0.8);

                Console.Clear();

                SoundPlayer goSound = new SoundPlayer("tetrisSoundGo.wav");
                goSound.Play();

                do
                {
                    Console.SetCursorPosition(0, 5);
                    Console.WriteLine(FiggleFonts.Starwars.Render("GO"));
                    

                } while (stopWatch.Elapsed.TotalSeconds < 2);


            } while (stopWatch.Elapsed.TotalSeconds < 1.3);
            stopWatch.Stop();

            Console.Clear();

            _game = new Game();
            _gameRenderer = new GameRenderer(_game);
            _game.Start();

            ConsoleKey input;

            do
            {
                input = Console.ReadKey(true).Key;
                if (input == ConsoleKey.RightArrow)
                {
                    _game.MoveRight();
                }
                else if (input == ConsoleKey.LeftArrow)
                {
                    _game.MoveLeft();
                }
                else if (input == ConsoleKey.DownArrow)
                {
                    _game.MoveDown();
                }
                else if (input == ConsoleKey.Spacebar)
                {
                    _game.Rotate();
                }
                else if (input == ConsoleKey.DownArrow)
                {
                    _game.MoveDown();
                }
                else if (input == ConsoleKey.Enter)
                {
                    _game.DropDown();
                }
                else if (input == ConsoleKey.Escape)
                {
                    _game.Stop();

                    SoundCancel();
                    MainMenu();
                }
                else if (input == ConsoleKey.R)
                {
                    _game.Stop();
                    _gameRenderer.DeathAnim();
                }
                else if (input == ConsoleKey.P)
                {
                    _game.Pause();
                }
                else if (input == ConsoleKey.A)
                {
                    Console.Clear();
                    _game.Stop();
                    _game.State = GameState.Finished;
                    _gameRenderer.CheatCode();
                    _game.Start();
                }

            } while (input != ConsoleKey.Escape);
        }

        public static void SoundCancel()
        {
            SoundPlayer cancelSound = new SoundPlayer("tetrisSoundCancel.wav");
            cancelSound.Play();
        }          

        public static void SoundReady()
        {
            SoundPlayer readySound = new SoundPlayer("tetrisSoundReady.wav");
            readySound.Play();
        }
    }
}
