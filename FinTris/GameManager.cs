﻿///ETML
///Auteur   	: José Carlos Gasser, Ahmad Jano, Maxime Andrieux, Maxence Weyermann, Larissa Debarros
///Date     	: 19.03.2021
///Description  : Fintris

using System;
using Figgle;

namespace FinTris
{
    public static class GameManager
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
        /// Fonction qui s'occupe du Menu
        /// </summary>
        public static void MainMenu()
        {
            Menu _menu = new Menu(FiggleFonts.Starwars.Render("FinTris"));

            MenuEntry play = new MenuEntry("Play");
            MenuEntry options = new MenuEntry("Options");
            MenuEntry quit = new MenuEntry("Quit");
            MenuEntry playerName = new MenuEntry("Player name: ", Config.PlayerName);

            _menu.Add(play);
            _menu.Add(options);
            _menu.Add(quit);
            _menu.Add(playerName);

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

                }
                if (choice == difficulty)
                {
                    SelectDifficulty();
                }
                else if (choice == cancel)
                {
                    MainMenu(); //huuuuh
                }
            } while (choice != cancel);

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
    }
}
