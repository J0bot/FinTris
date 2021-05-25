/// ETML
/// Auteur   	 : José Carlos Gasser, Ahmad Jano, Maxime Andrieux, Maxence Weyermann, Larissa Debarros
/// Date     	 : 09.03.2021
/// Description  : Fintris

using System;
using ConsoleEngine;

namespace FinTris
{
    /// <summary> 
    /// Classe principle du programme/jeu.
    /// </summary>
    class Program
    {
        static Game game;
        /// <summary>
        /// Le point d'entrée du programme/jeu.
        /// </summary>
        /// <param name="args">Les paramètres passés après l'exécution du programme/jeu.</param>
        static void Main(string[] args)
        {
            // SCENE 1 : (menu)
            Scene sceneMenu = new Scene("Menu", "Fintris", 60)
            {
                IsCursorVisible = false
            };

            StackPanel<Button> menu = new StackPanel<Button>()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = sceneMenu.Width
            };
            TextBlock title = new TextBlock(Resources.fintris_title)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = sceneMenu.Width
            };
            menu.Position += Vector2.Up * (title.Height + 3);

            sceneMenu.AddObject(menu);
            sceneMenu.AddObject(title);

            Button btnPlay = new Button("Play") { Height = 3 };
            btnPlay.Clicked += BtnPlay_Clicked;
            menu.AddComponent(btnPlay);

            Button btnOptions = new Button("Options") { Height = 3 };
            btnOptions.Clicked += BtnOptions_Clicked;
            menu.AddComponent(btnOptions);

            Button btnPlayerName = new Button("Player name:") { Height = 3 };
            btnPlayerName.Clicked += BtnPlayerName_Clicked;
            menu.AddComponent(btnPlayerName);

            Button btnQuit = new Button("Quit") { Height = 3 };
            btnQuit.Clicked += BtnQuit_Clicked;
            menu.AddComponent(btnQuit);
            menu.HorizontalAlignment = HorizontalAlignment.Center;

            // SCENE 2 (main) :
            Scene sceneMain = new Scene("Main", "Fintris", 100, 40)
            {
                BackgroundColor = ConsoleColor.DarkGray,
                BackgroundImage = new ConsoleBackground(Resources.fintris_background),
                IsCursorVisible = false
            };

            // Scene 3 (menu.options):
            Scene sceneMenuOptions = new Scene("Options", "Options", 60)
            {
                IsCursorVisible = false
            };
            StackPanel<Button> options = new StackPanel<Button>()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Middle, //Ahmad on compte sur toi pour implémenter ce truc
                Width = sceneMenuOptions.Width
            };
            sceneMenuOptions.AddObject(options);
           

            Button btnBestScores = new Button("Show best scores");
            btnBestScores.Clicked += BtnBestScores_Clicked;
            options.AddComponent(btnBestScores);

            Button btnDifficulty = new Button($"Difficulty: {Config.DifficultyLevel}");
            options.AddComponent(btnDifficulty);
            btnDifficulty.Clicked += BtnDifficulty_Clicked;

            Button btnCancel = new Button("Return");
            options.AddComponent(btnCancel);
            btnCancel.Clicked += BtnCancel_Clicked;



            game = new Game();

            GameRenderer renderer = new GameRenderer(game)
            {
                Position = new Vector2(38, 13)
            };
            renderer.KeyPressed += Renderer_KeyPressed;
            sceneMain.AddObject(renderer);

            ScenesManager.Add(sceneMenu);
            ScenesManager.Add(sceneMain);
            ScenesManager.Add(sceneMenuOptions);
            ScenesManager.LoadScene("Menu");

        }

       




        //Buttons main menu
        private static void BtnPlay_Clicked(object sender, EventArgs e)
        {
            ScenesManager.LoadScene("Main");
            game.Start();
        }
        private static void BtnOptions_Clicked(object sender, EventArgs e)
        {
            ScenesManager.LoadScene("Options");
        }
        private static void BtnPlayerName_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        private static void BtnQuit_Clicked(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        //Buttons main menu.options
        private static void BtnBestScores_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        private static void BtnDifficulty_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        private static void BtnCancel_Clicked(object sender, EventArgs e)
        {
            ScenesManager.LoadScene("Menu");
        }

        private static void Renderer_KeyPressed(object sender, ConsoleKey input)
        {
            if (input == ConsoleKey.RightArrow)
            {
                game.MoveRight();
            }
            else if (input == ConsoleKey.LeftArrow)
            {
                game.MoveLeft();
            }
            else if (input == ConsoleKey.DownArrow)
            {
                game.MoveDown();
            }
            else if (input == ConsoleKey.Spacebar)
            {
                game.Rotate();
            }
            else if (input == ConsoleKey.DownArrow)
            {
                game.MoveDown();
            }
            else if (input == ConsoleKey.Enter)
            {
                game.DropDown();
            }
            else if (input == ConsoleKey.Escape)
            {
                game.Stop();
            }
            else if (input == ConsoleKey.P)
            {
                game.Pause();
            }
        }
    }
}
