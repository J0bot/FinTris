/// ETML
/// Auteur   	 : José Carlos Gasser, Ahmad Jano, Maxime Andrieux, Maxence Weyermann, Larissa Debarros
/// Date     	 : 09.03.2021
/// Description  : Fintris

using System;

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
            ButtonsContainer btnContainer = new ButtonsContainer();

            Button btnPlay = new Button("Play");
            Button btnPlay2 = new Button("Play2");

            btnPlay.Width = 50;
            btnPlay.HorizontalAlignment = HorizontalAlignment.Center;
            btnPlay.IsSelected = true;


            btnContainer.Position += Vector2.Right;
            btnContainer.Add(btnPlay);
            btnContainer.Add(btnPlay2);

            sceneMenu.Components.Add(btnContainer);

            ScenesManager.Add(sceneMenu);
            ScenesManager.SetActiveScene("Menu");

            Console.Read();
            //// Cacher le curseur.
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
