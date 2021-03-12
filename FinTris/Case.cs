///ETML
///Auteur   	: José Carlos Gasser, Ahmad Jano, Maxime Andrieux, Maxence Weyermann, Larissa Debarros
///Date     	: 12.03.2021
///Description  : Fintris

using System;

namespace FinTris
{
    /// <summary>
    /// Struct qui représente une case du terrain
    /// </summary>
    public struct Case
    {
        /// <summary>
        /// Couleur de la case
        /// </summary>
        private ConsoleColor _consoleColor;

        /// <summary>
        /// Etat de la case
        /// </summary>
        private SquareState _state;

        /// <summary>
        /// Couleur de la case en ConsoleColor.
        /// </summary>
        public ConsoleColor Color
        {
            get { return _consoleColor; }
            set { _consoleColor = value; }
        }

        /// <summary>
        /// Etat de la case en SquareState.
        /// </summary>
        public SquareState State
        {
            get { return _state; }
            set { _state = value; }
        }

        /// <summary>
        /// Constructeur renseigné du struct Case.
        /// </summary>
        /// <param name="consoleColor">couleur de la case</param>
        /// <param name="state">êtat de la case</param>
        public Case(SquareState state = SquareState.Empty, ConsoleColor consoleColor= ConsoleColor.Blue)
        {
            _consoleColor = consoleColor;
            _state = state;
        }

    }
}
