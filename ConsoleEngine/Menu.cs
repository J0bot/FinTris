/// ETML
/// Auteur   	 : José Carlos Gasser, Ahmad Jano, Maxime Andrieux, Maxence Weyermann, Larissa Debarros
/// Date     	 : 23.04.2021
/// Description  : Fintris

using System;
using System.Collections.Generic;

namespace ConsoleEngine
{
    /// <summary>
    /// Un menu interactif.
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// titre du menu en string.
        /// </summary>
        private string _title;

        private MenuEntry _selectedOption;

        public MenuEntry SelectedOption
        {
            get { return _selectedOption; }
            set { _selectedOption = value; }
        }


        /// <summary>
        /// Liste de toutes les entrées du menu.
        /// </summary>
        private readonly List<MenuEntry> _entries;

        /// <summary>
        /// Crée un menu qui contient des options.
        /// </summary>
        /// <param name="title">nom du titre du menu en string.</param>
        public Menu(string title)
        {
            this._title = title;
            this._entries = new List<MenuEntry>();
        }

        /// <summary>
        /// Ajouter une entrée dans le menu.
        /// </summary>
        /// <param name="menuEntry">paramètre d'entrée du menu en MenuEntry.</param>
        public void Add(MenuEntry menuEntry)
        {
            _entries.Add(menuEntry);

            // Force la sélection de la première option.
            if (_entries.Count == 0)
            {
                Select(menuEntry);
            }
        }

        /// <summary>
        /// Afficher le menu et retourner l'option sélectionnée.
        /// </summary>
        /// <returns>Retourne l'option que l'on a choisit.</returns>
        public MenuEntry ShowMenu()
        {
            Console.Clear();
            Console.Write(_title);
            Console.WriteLine();

            int initialY = Console.CursorTop;

            // Affichage des options de bases.
            WriteOptions(initialY);

            // TODO : gérer les flèches pour sélectionner une entrée.
            MenuEntry selectedEntry = null;
            int currentlySelected = 0;
            while (selectedEntry == null)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        if (currentlySelected > 0)
                        {
                            _entries[currentlySelected].Selected = false;
                            _entries[currentlySelected - 1].Selected = true;
                            currentlySelected--;
                            Console.SetCursorPosition(0, initialY);

                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (currentlySelected < _entries.Count - 1)
                        {
                            _entries[currentlySelected].Selected = false;
                            _entries[currentlySelected + 1].Selected = true;
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
            return null;// TODO : retourner l'entrée sélectionnée.
        }

        /// <summary>
        /// Affiche toutes les options depuis la position courante du curseur.
        /// </summary>
        private void WriteOptions(int y)
        {
            y = 10;
            for (int i = 0; i < _entries.Count; i++)
            {
                int x = (Console.WindowWidth / 2) - (_entries[i].Text.Length / 2);

                Console.SetCursorPosition(x, y); // x was 35.
                y += 3;
                _entries[i].WriteOption();
            }
        }

        private void Select(MenuEntry newSelectedOption)
        {
            _selectedOption = newSelectedOption;

            foreach (MenuEntry option in _entries)
            {
                if (option != newSelectedOption)
                {
                    option.Selected = false;
                }
            }

            _selectedOption.Selected = true;
        }
    }
}
