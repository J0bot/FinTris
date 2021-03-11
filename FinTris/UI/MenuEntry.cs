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
        private static int instanceCounter = 0;

        /// <summary>
        /// valeur du texte le plus long
        /// </summary>
        private static int longestText = 0;

        //Couleurs par défaut
        ConsoleColor selectedBGColor = ConsoleColor.DarkRed;
        ConsoleColor selectedFGColor = ConsoleColor.White;
        ConsoleColor unselectedBGColor = ConsoleColor.Black;
        ConsoleColor unselectedFGColor = ConsoleColor.Gray;

        /// <summary>
        /// identifiant unique
        /// </summary>
        private int _id;

        /// <summary>
        /// Texte du menu
        /// </summary>
        private string _text;

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
        /// <param name="text"></param>
        public MenuEntry(string text)
        {
            this._id = instanceCounter++;
            this._text = text;

            //Stocke la plus longue option en terme de caractères
            if (text.Length > longestText)
            {
                longestText = text.Length;
            }

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
