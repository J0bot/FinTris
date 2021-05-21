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
            Scene sceneMain = new Scene("Main")
            {
                IsCursorVisible = false
            };

            Menu menu = new Menu();
            TextBlock title = new TextBlock(Resources.fintris_title)
            {
                HorizontalAlignment = HorizontalAlignment.Center
            };
            menu.Position += Vector2.Up * (title.Height + 3);

            sceneMenu.AddComponent(menu);
            sceneMenu.AddComponent(title);

            Button play = new Button("Play") { Height = 3 };
            menu.AddComponent(play);
            menu.AddComponent(new Button("Options") { Height = 3 });
            menu.AddComponent(new Button("Player name:") { Height = 3 });
            menu.AddComponent(new Button("Quit") { Height = 3 });
            menu.HorizontalAlignment = HorizontalAlignment.Center;

            play.Clicked += (_, __) =>
            {
                ScenesManager.SetActiveScene("Main");
                GameManager.MainMenu();
            };

            ScenesManager.Add(sceneMenu);
            ScenesManager.Add(sceneMain);
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
