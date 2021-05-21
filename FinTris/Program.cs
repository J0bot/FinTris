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
        /// <summary>
        /// Le point d'entrée du programme/jeu.
        /// </summary>
        /// <param name="args">Les paramètres passés après l'exécution du programme/jeu.</param>
        static void Main(string[] args)
        {
            Scene sceneMenu = new Scene("Menu")
            {
                Title = "Fintris",
                Width = 60,
                IsCursorVisible = false
            };

            Scene sceneOptions = new Scene("Options")
            {
                Title = "Options",
                Width = 60,
                IsCursorVisible = false
            };

            Scene sceneMain = new Scene("Main")
            {
                IsCursorVisible = false,
                Width = 100,
                Height = 28
            };

            Menu menu = new Menu();
            TextBlock title = new TextBlock(Resources.fintris_title)
            {
                HorizontalAlignment = HorizontalAlignment.Center
            };
            menu.Position += Vector2.Up * (title.Height + 3);

            sceneMenu.AddComponent(menu);
            sceneMenu.AddComponent(title);

            Button btnPlay = new Button("Play") { Height = 3 };
            menu.AddComponent(btnPlay);

            Button btnOptions = new Button("Options") { Height = 3 };
            menu.AddComponent(btnOptions);

            Button btnPlayerName = new Button("Player name:") { Height = 3};
            menu.AddComponent(btnPlayerName);

            Button btnQuit = new Button("Quit") { Height = 3 };
            menu.AddComponent(btnQuit);
            menu.HorizontalAlignment = HorizontalAlignment.Center;

            btnPlay.Clicked += (_, __) =>
            {
                ScenesManager.SetActiveScene("Main");
                GameManager.MainMenu();
            };

            btnQuit.Clicked += (_, __) =>
            {
                Environment.Exit(0);
            };

            btnOptions.Clicked += (_, __) =>
            {
                GameManager.ShowOptions(sceneOptions);
            };

            ScenesManager.Add(sceneMenu);
            ScenesManager.Add(sceneMain);
            ScenesManager.Add(sceneOptions);

            ScenesManager.SetActiveScene("Menu");

            // Game development



            //// Agrandir la font
            //ConsoleHelper.SetCurrentFont("Consolas", 20);

            //// Changer le titre de la fenêtre.
            //Console.Title = "FinTris";

            //// Commencer le jeu.
            


        }
    }
}
