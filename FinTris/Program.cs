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


            sceneMenu.Width = 50;
            ButtonsContainer btnContainer = new ButtonsContainer();
            btnContainer.Width = 25;
            btnContainer.Position += new Vector2((sceneMenu.Width - btnContainer.Width) / 2, 0);
            Button btnPlay = new Button("123456789");
            Button btnPlay2 = new Button("123456789123456");
            Button btnPlay3 = new Button("Button 3");


            btnContainer.AddComponent(btnPlay);
            btnContainer.AddComponent(btnPlay2);
            btnContainer.AddComponent(btnPlay3);

            btnPlay.HorizontalAlignment = HorizontalAlignment.Center;
            btnPlay2.HorizontalAlignment = HorizontalAlignment.Center;
            btnPlay3.HorizontalAlignment = HorizontalAlignment.Center;

            sceneMenu.AddComponent(btnContainer);
            Console.CursorVisible = false;
            ScenesManager.Add(sceneMenu);
            ScenesManager.SetActiveScene("Menu");

            // Game development


            // Cacher le curseur.
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
