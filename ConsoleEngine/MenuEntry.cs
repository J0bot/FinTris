///ETML
///Auteur   	: José Carlos Gasser, Ahmad Jano, Maxime Andrieux, Maxence Weyermann, Larissa Debarros
///Date     	: 09.03.2021
///Description  : Fintris

using System;

namespace ConsoleEngine
{
    public class MenuEntry 
    {
        //Couleurs par défaut
        private const ConsoleColor selectedBGColor = ConsoleColor.DarkRed;
        private const ConsoleColor selectedFGColor = ConsoleColor.White;
        private const ConsoleColor unselectedBGColor = ConsoleColor.Black;
        private const ConsoleColor unselectedFGColor = ConsoleColor.Gray;

        /// <summary>
        /// Texte du menu
        /// </summary>
        private readonly string _text;

        /// <summary>
        /// bool pour savoir si un élément est séléctionné
        /// </summary>
        private bool _isSelected = false; //Option déselectionnée par défaut

        /// <summary>
        /// Retourne le texte de l'entrée
        /// </summary>
        public string Text
        {
            get { return _text; }
        }

        /// <summary>
        /// Retourne si l'entrée est sélectionnée ou pas
        /// </summary>
        public bool Selected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }

        /// <summary>
        /// Constructeur avec identifiant automatique
        /// </summary>
        /// <param name="text"></param>
        public MenuEntry(string text)
        {
            this._text = text;
        }

        /// <summary>
        /// Affichage de l'entrée actuelle
        /// </summary>
        public void WriteOption()
        {
            //backup state
            ConsoleColor previousBG = Console.BackgroundColor;
            ConsoleColor previousFG = Console.ForegroundColor;

            if (_isSelected)
            {
                Console.BackgroundColor = selectedBGColor;
                Console.ForegroundColor = selectedFGColor;
            }
            else
            {
                Console.BackgroundColor = unselectedBGColor;
                Console.ForegroundColor = unselectedFGColor;
            }

            //Ajoute des caractères vides
            Console.Write(ToString());

            //put original state
            Console.BackgroundColor = previousBG;
            Console.ForegroundColor = previousFG;
        }

        /// <summary>
        /// Renvoie le numéro et le texte de l'entrée
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _text;
        }
    }
}
