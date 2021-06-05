/// ETML
/// Auteur   	 : José Carlos Gasser, Ahmad Jano, Maxime Andrieux, Maxence Weyermann, Larissa Debarros
/// Date     	 : 09.03.2021
/// Description  : Fintris

namespace FinTris
{
    /// <summary>
    /// Differents états du jeu.
    /// </summary>
    public enum GameState
    {
        /// <summary>
        /// Le jeu est en attente
        /// </summary>
        Waiting,

        /// <summary>
        /// Le jeu est en cours
        /// </summary>
        Playing,

        /// <summary>
        /// Le jeu est en pause
        /// </summary>
        Paused,

        /// <summary>
        /// La partie actuelle est finie
        /// </summary>
        Finished
    }
}