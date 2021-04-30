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

            // Cacher le curseur.
            Console.CursorVisible = false;

            // Agrandir la font
            ConsoleHelper.SetCurrentFont("Consolas", 20);

            // Changer le titre de la fenêtre.
            Console.Title = "FinTris";

            // Commencer le jeu.
            GameManager.MainMenu();
        }

    }
}
