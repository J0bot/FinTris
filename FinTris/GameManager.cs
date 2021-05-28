﻿///ETML
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

        public static bool checkSound = true;

        /// <summary>
        /// Fonction qui s'occupe du Menu
        /// </summary>
        public static void MainMenu()
        {
            SoundPlayer themeSound = new SoundPlayer(Resources.tetrisSoundTheme);
            themeSound.PlayLooping();

            if (checkSound == false)
            {
                themeSound.Stop();
            }

           
            Menu _menu = new Menu(FiggleFonts.Starwars.Render("FinTris"));

            MenuEntry play = new MenuEntry("Play");
            MenuEntry options = new MenuEntry("Options");
            MenuEntry playerName = new MenuEntry("Player name: ", Config.PlayerName);
            MenuEntry credits = new MenuEntry("Credits");
            MenuEntry quit = new MenuEntry("Quit");

            _menu.Add(play);
            _menu.Add(options);
            _menu.Add(playerName);
            _menu.Add(credits);
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
            else if (choice == playerName)
            {
                Config.PlayerName = AskForInput();
                //this is probably disgusting but I'll do it anyway
                MainMenu();
            }

            else if (choice == credits)
            {
                Console.Clear();
                Console.SetCursorPosition(55,9);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Développeurs : ");
                Console.ResetColor();

                Console.SetCursorPosition(52, 11);
                Console.WriteLine("José Carlos Gasser");

                Console.SetCursorPosition(56, 13);
                Console.WriteLine("Ahmad Jano");

                Console.SetCursorPosition(53, 15);
                Console.WriteLine("Maxime Andrieux");

                Console.SetCursorPosition(53, 17);
                Console.WriteLine("Maxence Weyermann");

                Console.SetCursorPosition(53, 19);
                Console.WriteLine("Larissa Debarros");
                Console.Read();
                MainMenu();
            }

            else
            {
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Méthode qui affiche le menu lorsqu'on appuie sur esc
        /// </summary>
        public static void pauseMenu()
        {
            _game.Pause();

            Menu pauseMenu = new Menu("Pause");
            MenuEntry choice;

            MenuEntry goBack = new MenuEntry("Resume");
            MenuEntry option = new MenuEntry("Option");
            MenuEntry menuBack = new MenuEntry("Return to the menu");

            pauseMenu.Add(goBack);
            pauseMenu.Add(option);
            pauseMenu.Add(menuBack);

            choice = pauseMenu.ShowMenu();

            if (choice == goBack)
            {
                Console.Clear();
                _gameRenderer = new GameRenderer(_game);
                _game.Pause();
            }

            else if (choice == option)
            {
                ShowOptionsInGame();
            }

            else if (choice == menuBack)
            {
                MainMenu();
            }      
        }

        /// <summary>
        /// Méthode pour choisir un nouveau nom. Assez banal pour l'instant.
        /// </summary>
        /// <returns>Le nouveau nom du joueur</returns>
        public static string AskForInput()
        {
            //Rend le curseur visible
            Console.CursorVisible = true;

            //déclare les variables nécessaires
            string askNewName = "Enter a new name: ";
            string newName = "";
            byte maxNameLength = 30;
            int input;

            //rafraîchit l'écran et écrit le string askNewName et le nom bien centré
            RefreshAskForName(askNewName, newName);

            do
            {
                RefreshAskForName(askNewName, newName);

                input = Console.Read();

                //si input égale 0, la touche BACKSPACE a été pressée
                //checker aussi si la taille du nom actuel est plus grande que zéro
                if (input == 0 && newName.Length > 0)
                {
                    //retirer le dernier caractère du nom actuel
                    newName = newName.Remove(newName.Length - 1);
                }
                //sinon, si la taille du nom est inférieure au maximum et que la touche pressée n'est pas ESCAPE...
                else if (newName.Length < maxNameLength && input != 0)
                {
                    //ajouter le caractère qui vient d'être pressé au nom actuel
                    newName += Convert.ToChar(input);
                }

                //continuer cette boucle tant que input n'égal pas à ces valeurs.
                //27 veut dire ESCAPE, 10 et 13 sont des terminateurs de lignes sur Windows, Mac et UNIX (donc équivaut à ENTER)
            } while (input != 27 && input != 10 && input != 13);
            Console.CursorVisible = false;

            //Si le joueur a annulé le changement de nom, on renvoie juste l'ancien nom
            if (input == 27 || newName.Length < 2) //pourquoi le nom comporte 1 caractère d'office?
            {
                return Config.PlayerName;
            }
            return newName;
        }


        private static void RefreshAskForName(string askNewName, string newName)
        {
            Console.Clear();
            Console.CursorTop = Console.WindowHeight / 2;
            Console.CursorLeft = (Console.WindowWidth - askNewName.Length - newName.Length) / 2;
            //Console.Write(askNewName + newName);

            Console.Write(askNewName);
            if (newName == "")
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(Config.PlayerName);
                Console.ForegroundColor = ConsoleColor.White;
                //Console.CursorLeft = Console.CursorLeft - Config.PlayerName.Length;
            }
            else
            {
                Console.Write(newName);
            }

        }

        /// <summary>
        /// Shows the options panel.
        /// </summary>
        public static void ShowOptions()
        {
            Menu optionMenu = new Menu("Options");

            MenuEntry bestScores = new MenuEntry("Show best scores");
            MenuEntry difficulty = new MenuEntry("Difficulty: ", Config.DifficultyLevel);
            MenuEntry sounds = new MenuEntry("Sounds");
            
            MenuEntry cancel = new MenuEntry("Return");

            optionMenu.Add(bestScores);
            optionMenu.Add(difficulty);
            optionMenu.Add(sounds);
            optionMenu.Add(cancel);       

            MenuEntry choice;
            do
            {      
                choice = optionMenu.ShowMenu();

                if (choice == bestScores)
                {
                    ShowBestScores();
                }
                else if (choice == difficulty)
                {
                    SelectDifficulty();
                }

                else if (choice == sounds)
                {
                    SoundSettings();
                }


                else if (choice == cancel)
                {
                    SoundCancel();
                    MainMenu(); //huuuuh
                }
            } while (choice != cancel);

        }

        /// <summary>
        /// Affiche le menu option depuis le menu pause
        /// </summary>
        public static void ShowOptionsInGame()
        {
            Menu optionMenu = new Menu("Options");

            MenuEntry bestScores = new MenuEntry("Show best scores");
            MenuEntry difficulty = new MenuEntry("Difficulty: ", Config.DifficultyLevel);
            MenuEntry sounds = new MenuEntry("Sounds");
            MenuEntry cancel = new MenuEntry("Return");

            optionMenu.Add(bestScores);
            optionMenu.Add(difficulty);
            optionMenu.Add(sounds);
            optionMenu.Add(cancel);

            MenuEntry choice;
            do
            {
                choice = optionMenu.ShowMenu();

                if (choice == bestScores)
                {
                    ShowBestScores();
                }
                else if (choice == difficulty)
                {
                    SelectDifficultyInGame();
                }

                else if (choice == sounds)
                {
                    SoundSettings();
                }

                else if (choice == cancel)
                {
                    SoundCancel();
                    pauseMenu();
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
            Console.CursorTop = (Console.WindowHeight / 2) - (scores.Count);

            foreach (string[] entry in scores)
            {
                //why is there a space between the scores???
                Console.CursorTop += 1;
                Console.CursorLeft = (Console.WindowWidth / 2) - entry[0].Length - 2;
                Console.Write(entry[0]);
                Console.CursorLeft = (Console.WindowWidth / 2) + 2;
                Console.WriteLine(entry[1]);
            }
            Console.ReadLine();
        }

        /// <summary>
        /// Méthode qui change la difficulté du jeu
        /// </summary>
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

        public static void SelectDifficultyInGame()
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
            ShowOptionsInGame();
        }

        public static void SoundSettings()
        {
            Menu soundMenu = new Menu("Sounds settings");
            MenuEntry soundOn = new MenuEntry("Sound On");
            MenuEntry soundOff = new MenuEntry("Sound off");

            soundMenu.Add(soundOn);
            soundMenu.Add(soundOff);
            MenuEntry choice = soundMenu.ShowMenu();

            if (choice == soundOff)
            {
                SoundPlayer themeSound = new SoundPlayer(Resources.tetrisSoundTheme);
                themeSound.Stop();
                checkSound = false;
            }

            else
            {
                checkSound = true;
            }
        }

        /// <summary>
        /// Méthode play permet de lancer tous les éléments du jeu et de reset le jeu
        /// </summary>
        public static void Play()
        {
            SoundPlayer okSound = new SoundPlayer(Resources.tetrisSoundOK);
            Stopwatch stopWatch = new Stopwatch();

            if (checkSound == true)
            {
                okSound.Play();
            }
            
            stopWatch.Start();
            do
            {
                Console.Clear();

            } while (stopWatch.Elapsed.TotalSeconds < 1);

            stopWatch.Stop();

            Console.Clear();

            if (checkSound == true)
            {
                SoundReady();
            }     

            stopWatch.Restart();
            do
            {
                do
                {
                    Console.SetCursorPosition(0, 5);
                    Console.WriteLine(FiggleFonts.Starwars.Render("Ready"));

                } while (stopWatch.Elapsed.TotalSeconds < 0.8);

                Console.Clear();

                SoundPlayer goSound = new SoundPlayer(Resources.tetrisSoundGo);

                if (checkSound == true)
                {
                    goSound.Play();
                }

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
                    SoundPlayer pauseSound = new SoundPlayer(Resources.TetrisSoundPause);
                    pauseMenu();

                    /*
                    _game.Stop();

                    SoundCancel();
                    MainMenu();
                    */
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

            } while (input != ConsoleKey.Q);
        }

        public static void SoundCancel()
        {
            SoundPlayer cancelSound = new SoundPlayer(Resources.tetrisSoundCancel);

            if (checkSound == true)
            {
                cancelSound.Play();
            }
            
        }          

        public static void SoundReady()
        {
            SoundPlayer readySound = new SoundPlayer(Resources.tetrisSoundReady);
            readySound.Play();
        }
    }
}
