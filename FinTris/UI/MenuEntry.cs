///ETML
///Auteur   	: José Carlos Gasser, Ahmad Jano, Maxime Andrieux, Maxence Weyermann, Larissa Debarros
///Date     	: 09.03.2021
///Description  : Fintris

using System;

namespace FinTris
{
    public class MenuEntry
    {
        /// <summary>
        /// Compteur d'instance
        /// </summary>
        private static int _instanceCounter = 0;

        //Couleurs par défaut
        private const ConsoleColor selectedBGColor = ConsoleColor.DarkRed;
        private const ConsoleColor selectedFGColor = ConsoleColor.White;
        private const ConsoleColor unselectedBGColor = ConsoleColor.Black;
        private const ConsoleColor unselectedFGColor = ConsoleColor.Gray;

        /// <summary>
        /// identifiant unique
        /// </summary>
        private readonly int _id;

        /// <summary>
        /// Texte du menu
        /// </summary>
        private string _text;

        /// <summary>
        /// bool pour savoir si un élément est séléctionné
        /// </summary>
        private bool _isSelected = false; //Option déselectionnée par défaut

        /// <summary>
        /// The suffix appended to the entry
        /// </summary>
        private string _suffix = "";

        /// <summary>
        /// Retourne le texte de l'entrée
        /// </summary>
        public string Text
        {
            get { return _text + _suffix; }
            set { _text = value; }
        }

        public string Suffix
        {
            get { return _suffix; }
            set { _suffix = value; }
        }

        /// <summary>
        /// Retourne l'id de l'entrée
        /// </summary>
        public int Id
        {
            get { return _id; } 
        }

        /// <summary>
        /// Retourne si l'entrée est sélectionnée ou pas
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }

        /// <summary>
        /// Constructeur avec identifiant automatique
        /// </summary>
        /// <param name="text">nom du MenuEntry</param>
        public MenuEntry(string text)
        {
            this._id = _instanceCounter++;
            this._text = text;
        }

        /// <summary>
        /// Constructeur renseigné de la classe MenuEntry avec le suffixe
        /// </summary>
        /// <param name="text">nom du MenuEntry</param>
        /// <param name="suffix">suffixe</param>
        public MenuEntry(string text, string suffix)
        {
            this._id = _instanceCounter++;
            this._text = text;
            this._suffix = suffix;
        }

        /// <summary>
        /// Affichage de l'entrée actuelle
        /// </summary>
        public void RenderOption()
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
        /// <returns>retourne le numero et texte de l'entrée</returns>
        public override string ToString()
        {
            return $"{_text}{_suffix}";
        }
    }
}
