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
    public class Menu
    {
        /// <summary>
        /// titre du menu en string.
        /// </summary>
        private readonly string _title;

        /// <summary>
        /// Liste de toutes les entrées du menu
        /// </summary>
        private readonly List<MenuEntry> _entries = new List<MenuEntry>();

        /// <summary>
        /// index de l'entrée
        /// </summary>
        private int _index;

        /// <summary>
        /// Propriété de l'option sélectionnée
        /// </summary>
        public MenuEntry SelectedOption
        {
            get { return _index > -1 ? _entries[_index] : null; }
        }

        /// <summary>
        /// Constructeur renseigné de la classe Menu
        /// </summary>
        /// <param name="title">nom du titre du menu en string</param>
        public Menu(string title)
        {
            this._title = title;
            this._index = 0;
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
        public void ShowMenu()
        {
            Console.Clear();

            int y = 0;
            foreach (string line in _title.Split('\n'))
            {
                Console.SetCursorPosition((Console.WindowWidth - 31) / 2, y);
                Console.WriteLine(line);
                y++;
            }

            ConsoleKey input;

            do
            {
                RenderOptions();
                input = Console.ReadKey().Key;
                if (input == ConsoleKey.UpArrow)
                {
                    if (_index > 0)
                    {
                        _entries[_index].IsSelected = false;
                        _entries[_index - 1].IsSelected = true;
                        _index--;
                    }
                }
                else if (input == ConsoleKey.DownArrow)
                {
                    if (_index < _entries.Count - 1)
                    {
                        _entries[_index].IsSelected = false;
                        _entries[_index + 1].IsSelected = true;
                        _index++;
                    }
                }
                else if (input == ConsoleKey.Escape)
                {
                    _index = -1;
                    return;
                }
            } while (input != ConsoleKey.Enter);
        }

        /// <summary>
        /// Affiche toutes les options depuis la position courante du curseur
        /// </summary>
        private void RenderOptions()
        {
            int y = 10;
            for (int i = 0; i < _entries.Count; i++)
            {
                int x = (Console.WindowWidth - _entries[i].Text.Length) / 2;

                Console.SetCursorPosition(x, y); //x was 35
                _entries[i].RenderOption();
                y += 3;
            }
        }
    }
}
