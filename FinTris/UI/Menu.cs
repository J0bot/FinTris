///ETML
///Auteur   	: José Carlos Gasser, Ahmad Jano, Maxime Andrieux, Maxence Weyermann, Larissa Debarros
///Date     	: 09.03.2021
///Description  : Fintris

using System;
using System.Collections.Generic;

namespace FinTris
{
    /// <summary>
    /// Un menu interactif
    /// </summary>
    class Menu
    {
        /// <summary>
        /// titre du menu en string.
        /// </summary>
        private string _title;

        /// <summary>
        /// Liste de toutes les entrées du menu
        /// </summary>
        private List<MenuEntry> _entries = new List<MenuEntry>();

        /// <summary>
        /// Constructeur renseigné de la classe Menu
        /// </summary>
        /// <param name="title">nom du titre du menu en string</param>
        public Menu(string title)
        {
            this._title = title;
        }

        /// <summary>
        /// Ajouter une entrée dans le menu
        /// </summary>
        /// <param name="menuEntry">paramètre d'entrée du menu en MenuEntry</param>
        public void Add(MenuEntry menuEntry)
        {
            //Force la sélection de la première option
            if (_entries.Count == 0)
            {
                menuEntry.IsSelected = true;
            }
            _entries.Add(menuEntry);
        }

        /// <summary>
        /// Afficher le menu et retourner l'option sélectionnée
        /// </summary>
        /// <returns>Retourne l'option que l'on a choisit</returns>
        public MenuEntry ShowMenu()
        {
            Console.Clear();

            Console.WriteLine(_title);
            Console.WriteLine();
            int initialY = Console.CursorTop;

            //Affichage des options de bases
            WriteOptions(initialY);

            //TODO gérer les flèches pour sélectionner une entrée
            MenuEntry selectedEntry = null;
            int currentlySelected = 0;
            while (selectedEntry == null)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        if (currentlySelected > 0)
                        {
                            _entries[currentlySelected].IsSelected = false;
                            _entries[currentlySelected - 1].IsSelected = true;
                            currentlySelected--;
                            Console.SetCursorPosition(0, initialY);

                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (currentlySelected < _entries.Count - 1)
                        {
                            _entries[currentlySelected].IsSelected = false;
                            _entries[currentlySelected + 1].IsSelected = true;
                            currentlySelected++;
                            Console.SetCursorPosition(0, initialY);
                        }
                        break;
                    case ConsoleKey.Enter:
                        return _entries[currentlySelected];

                }
                WriteOptions(initialY);
            }

            Console.ReadLine();
            return null;//TODO retourner l'entrée sélectionnée
        }

        /// <summary>
        /// Affiche toutes les options depuis la position courante du curseur
        /// </summary>
        private void WriteOptions(int y)
        {
            y = 10;
            for (int i = 0; i < _entries.Count; i++)
            {
                Console.SetCursorPosition(35, y);
                y += 7;
                _entries[i].WriteOption();
            }
        }
    }
}
