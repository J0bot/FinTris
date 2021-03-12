///ETML
///Auteur   	: José Carlos Gasser, Ahmad Jano, Maxime Andrieux, Maxence Weyermann, Larissa Debarros
///Date     	: 09.03.2021
///Description  : Fintris

using Figgle;
using System;

namespace FinTris
{
    /// <summary>
    /// Classe Main
    /// </summary>
    class Program
    {
        /// <summary>
        /// Attribut Game de la classe Program
        /// </summary>
        private static Game _game;

        /// <summary>
        /// Attribut GameRenderer de la classe Program
        /// </summary>
        private static GameRenderer _gameRenderer;

        

        /// <summary>
        /// Fonction principale qui lance tout et qui gère le menu
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            Console.Title = "FinTris";

            MainMenu();


        }

        /// <summary>
        /// Méthode play permet de lancer tous les éléments du jeu
        /// </summary>
        public static void Play()
        {
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
                else if(input == ConsoleKey.DownArrow)
                {
                    _game.MoveDown();
                }
                else if (input == ConsoleKey.Enter)
                {
                    _game.DropDown();
                }
                else if (input == ConsoleKey.Escape)
                {
                    _game.GameTimer.Stop();
                    MainMenu();
                }
                else if (input == ConsoleKey.R)
                {
                    _game.GameTimer.Stop();
                    _gameRenderer.DeathAnim();
                }


            } while (input != ConsoleKey.Escape);
        }

        private static void MainMenu()
        {
            Menu _menu = new Menu(FiggleFonts.Starwars.Render("FinTris"));

            MenuEntry play = new MenuEntry("Play");
            MenuEntry quit = new MenuEntry("Quit");

            _menu.Add(play);
            _menu.Add(quit);

            MenuEntry choice = null;

            do
            {
                choice = _menu.ShowMenu();

                if (choice == play)
                {
                    Play();
                }
                else
                {
                    Environment.Exit(0);
                }

            } while (choice != quit);

            Environment.Exit(0);
        }
    }
}
