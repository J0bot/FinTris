/// ETML
/// Auteur   	 : José Carlos Gasser, Ahmad Jano, Maxime Andrieux, Maxence Weyermann, Larissa Debarros
/// Date     	 : 09.03.2021
/// Description  : Fintris

namespace FinTris
{
    /// <summary>
    /// Enum des états de mouvement d'un Tetromino.
    /// </summary>
    public enum TetrominoState
    {
        /// <summary>
        /// Quand un Tetromino est en mouvement.
        /// </summary>
        Moving,

        /// <summary>
        /// Quand un Tetromino est à l'arret.
        /// </summary>
        Stopped,

        /// <summary>
        /// Quand un Tetromino est en attente de se faire lancer dans le jeu.
        /// </summary>
        NextTetromino
    }
}
