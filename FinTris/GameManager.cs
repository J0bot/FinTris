﻿///ETML
///Auteur   	: José Carlos Gasser, Ahmad Jano, Maxime Andrieux, Maxence Weyermann, Larissa Debarros
///Date     	: 19.03.2021
///Description  : Fintris

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Media;
using System.Threading;

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
        public static GameRenderer _gameRenderer;
        public static readonly SoundPlayer themeSound = new SoundPlayer(Resources.tetrisSoundTheme);
        public static readonly SoundPlayer okSound = new SoundPlayer(Resources.tetrisSoundOK);
        public static readonly SoundPlayer goSound = new SoundPlayer(Resources.tetrisSoundGo);
        public static readonly SoundPlayer pauseSound = new SoundPlayer(Resources.TetrisSoundPause);
        public static readonly SoundPlayer readySound = new SoundPlayer(Resources.tetrisSoundReady);
        public static readonly SoundPlayer cancelSound = new SoundPlayer(Resources.tetrisSoundCancel);
        public static readonly SoundPlayer fallSound = new SoundPlayer(Resources.TetrisSoundFall);


        private static bool _muted = false;
        public static bool Muted
        {
            get { return _muted; }
            set { _muted = value; }
        }

        /// <summary>
        /// Fonction qui s'occupe du Menu
        /// </summary>
        public static void MainMenu()
        {

            Menu _menu = new Menu(Resources.fintris_title);

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

            _menu.ShowMenu();

            if (_menu.SelectedOption == play)
            {
                Play();                
            }
            else if (_menu.SelectedOption == options)
            {
                ShowOptions();
                MainMenu();
            }
            else if (_menu.SelectedOption == playerName)
            {
                Config.PlayerName = AskForInput();
                //this is probably disgusting but I'll do it anyway
                MainMenu();
            }
            else if (_menu.SelectedOption == credits)
            {
                PrintCredit();
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

            MenuEntry goBack = new MenuEntry("Resume");
            MenuEntry option = new MenuEntry("Options");
            MenuEntry menuBack = new MenuEntry("Return to the menu");

            pauseMenu.Add(goBack);
            pauseMenu.Add(option);
            pauseMenu.Add(menuBack);

            pauseMenu.ShowMenu();

                if (pauseMenu.SelectedOption == goBack)
                {
                    _game.Resume();
                    _gameRenderer.ResetRender();
                }
                else if (pauseMenu.SelectedOption == option)
                {
                    ShowOptionsInGame();
                }
                else if (pauseMenu.SelectedOption == menuBack)
                {
                    MainMenu();
                }
        }

        private static void PrintCredit()
        {
            Console.Clear();
            Console.SetCursorPosition(55, 9);
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

            Console.ReadKey();

            MainMenu();
        }


        /// <summary>
        /// Méthode pour choisir un nouveau nom. Assez banal pour l'instant.
        /// </summary>
        /// <returns>Le nouveau nom du joueur</returns>
        private static string AskForInput()
        {
            string askNewName = "Enter a new name: ";
            Console.Clear();
            Console.CursorLeft = (Console.WindowWidth / 2) - askNewName.Length / 2;
            Console.CursorTop = (Console.WindowHeight / 2);
            Console.Write(askNewName);
            string entry = Console.ReadLine();
            if (entry.Length > 0)
            {
                return entry;
            }
            return Config.PlayerName;
        }

        /// <summary>
        /// Shows the options panel.
        /// </summary>
        private static void ShowOptions()
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

            do
            {
                optionMenu.ShowMenu();

                if (optionMenu.SelectedOption == bestScores)
                {
                    ShowBestScores();
                }
                else if (optionMenu.SelectedOption == difficulty)
                {
                    SelectDifficulty();
                    difficulty.Suffix = Config.DifficultyLevel;
                }
                else if (optionMenu.SelectedOption == sounds)
                {
                    SoundSettings();
                }

            } while (optionMenu.SelectedOption != cancel && optionMenu.SelectedOption != null);

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


            optionMenu.ShowMenu();
            do
            {


                if (optionMenu.SelectedOption == bestScores)
                {
                    ShowBestScores();
                }
                else if (optionMenu.SelectedOption == difficulty)
                {
                    SelectDifficultyInGame();
                }

                else if (optionMenu.SelectedOption == sounds)
                {
                    SoundSettings();
                }

                else if (optionMenu.SelectedOption == cancel)
                {
                    pauseMenu();
                }
            } while (optionMenu.SelectedOption != cancel);

        }

        /// <summary>
        /// Affiche les meilleurs scores.
        /// </summary>
        private static void ShowBestScores()
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

        /// <summary>
        /// Méthode qui change la difficulté du jeu
        /// </summary>
        private static void SelectDifficulty()
        {
            Menu optionMenu = new Menu("Difficulty levels");
            MenuEntry diffEasy = new MenuEntry("Easy");
            MenuEntry diffNormal = new MenuEntry("Normal");
            MenuEntry diffHard = new MenuEntry("Hard");
            optionMenu.Add(diffEasy);
            optionMenu.Add(diffNormal);
            optionMenu.Add(diffHard);

            optionMenu.ShowMenu();
            if (optionMenu.SelectedOption == diffEasy)
            {
                Config.DifficultyLevel = "Easy";
            }
            else if (optionMenu.SelectedOption == diffNormal)
            {
                Config.DifficultyLevel = "Normal";
            }
            else if (optionMenu.SelectedOption == diffHard)
            {
                Config.DifficultyLevel = "Hard";
            }

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
            optionMenu.ShowMenu();

            if (optionMenu.SelectedOption == diffEasy)
            {
                Config.DifficultyLevel = "Easy";
            }
            else if (optionMenu.SelectedOption == diffNormal)
            {
                Config.DifficultyLevel = "Normal";
            }
            else if (optionMenu.SelectedOption == diffHard)
            {
                Config.DifficultyLevel = "Hard";
            }
            ShowOptionsInGame();
        }

        private static void SoundSettings()
        {
            Menu soundMenu = new Menu("Sounds settings");
            MenuEntry soundOn = new MenuEntry("Sound On");
            MenuEntry soundOff = new MenuEntry("Sound off");

            soundMenu.Add(soundOn);
            soundMenu.Add(soundOff);

            soundMenu.ShowMenu();

            if (soundMenu.SelectedOption == soundOff)
            {
                _muted = true;
                themeSound.Stop();
            }
            else
            {
                _muted = false;
                PlaySound(themeSound, true);
            }
        }

        /// <summary>
        /// Méthode play permet de lancer tous les éléments du jeu et de reset le jeu
        /// </summary>
        public static void Play()
        {
            Console.Clear();

            int y = 0;
            foreach (string line in Resources.ready_title.Split('\n'))
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 - 14, Console.WindowHeight / 2 + y - 5);
                Console.Write(line);
                y++;
            }

            if (!_muted)
            {
                readySound.PlaySync();
            }


            Console.Clear();

            y = 0;
            foreach (string line in Resources.go_title.Split('\n'))
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 - 10, Console.WindowHeight / 2 + y - 5);
                Console.Write(line);
                y++;
            }

            if (!_muted)
            {
                goSound.PlaySync();
            }

            Console.Clear();

            _game = new Game();
            _gameRenderer = new GameRenderer(_game);
            _game.Start();

            PlaySound(themeSound, true);

            long lastRotation = 0;
            long lastDrop = 0;
            int rotCooldown = 100;
            int dropCoolDown = 1000;
            Stopwatch sw = Stopwatch.StartNew();
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
                    if (sw.ElapsedMilliseconds > lastRotation + rotCooldown)
                    {
                        _game.Rotate();
                        lastRotation = sw.ElapsedMilliseconds;
                    }
                }
                else if (input == ConsoleKey.DownArrow)
                {
                    _game.MoveDown();
                }
                else if (input == ConsoleKey.Enter)
                {
                    if (sw.ElapsedMilliseconds > lastDrop + dropCoolDown)
                    {
                        _game.DropDown();
                        lastDrop = sw.ElapsedMilliseconds;
                    }
                }
                else if (input == ConsoleKey.Escape)
                {
                    if (_game.State == GameState.Playing)
                    {
                        _game.Pause();
                        pauseMenu();
                    }
                    else if (_game.State == GameState.Paused)
                    {
                        _game.Start();
                        _gameRenderer.ResetRender();
                    }
                    else
                    {
                        _game.Pause();
                        MainMenu();
                    
                    }
                }
                else if (input == ConsoleKey.R)
                {
                    _game.Stop();
                    _gameRenderer.DeathAnim();
                }
                else if (input == ConsoleKey.P)
                {
                    _game.PauseOrResume();
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

            sw.Stop();
        }

        public static void PlaySound(SoundPlayer sound, bool looping = false)
        {
            if (!_muted)
            {
                if (looping)
                {
                    sound.PlayLooping();
                }
                else
                {
                    sound.Play();
                }
            }
        }
    }
}
