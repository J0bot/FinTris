///ETML
///Auteur   	: José Carlos Gasser, Ahmad Jano, Maxime Andrieux, Maxence Weyermann, Larissa Debarros
///Date     	: 09.03.2021
///Description  : Fintris

namespace FinTris
{
    /// <summary>
    /// Enum des états de mouvement d'un Tetromino
    /// </summary>
    public enum TetrominoState
    {
        /// <summary>
        /// Quand un Tetromino est en mouvement, il a l'état Moving
        /// </summary>
        Moving,

        /// <summary>
        /// Quand un Tetromino est à l'arret, il a l'état Stopped
        /// </summary>
        Stopped,

        /// <summary>
        /// Quand un Tetromino est en attente de se faire lancer dans le jeu, il est dans l'état NextTetromino
        /// </summary>
        NextTetromino
    }
}
