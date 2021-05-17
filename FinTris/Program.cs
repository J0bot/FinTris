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
            Scene sceneMenu = new Scene("Menu");
            Menu menu = new Menu();

            sceneMenu.AddComponent(menu);

            menu.AddComponent(new Button("Play") { Height = 3 });
            menu.AddComponent(new Button("Options") { Height = 3 });
            menu.AddComponent(new Button("Player name:") { Height = 3 });
            menu.AddComponent(new Button("Quit") { Height = 3 });
            menu.HorizontalAlignment = HorizontalAlignment.Center;

            ScenesManager.Add(sceneMenu);
            ScenesManager.SetActiveScene("Menu");

            // Game development


            //Console.CursorVisible = false;

            //// Agrandir la font
            //ConsoleHelper.SetCurrentFont("Consolas", 20);

            //// Changer le titre de la fenêtre.
            //Console.Title = "FinTris";

            //// Commencer le jeu.
            //GameManager.MainMenu();


        }

      
    }
}
