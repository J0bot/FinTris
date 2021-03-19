///ETML
///Auteur   	: José Carlos Gasser, Ahmad Jano, Maxime Andrieux, Maxence Weyermann, Larissa Debarros
///Date     	: 09.03.2021
///Description  : Fintris

using System;

namespace FinTris
{
    /// <summary>
    /// Classe Main
    /// </summary>
    class Program
    {

        /// <summary>
        /// Fonction principale qui lance tout et qui gère le menu
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            Console.Title = "FinTris";

            GameManager.MainMenu();
        }
    }
}
