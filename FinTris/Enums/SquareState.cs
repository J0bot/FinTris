/// ETML
/// Auteur   	 : José Carlos Gasser, Ahmad Jano, Maxime Andrieux, Maxence Weyermann, Larissa Debarros
/// Date      	 : 09.03.2021
/// Description  : Fintris

namespace FinTris
{
    /// <summary>
    /// Etat des blocs dans le jeu.
    /// </summary>
    public enum SquareState
    {
        /// <summary>
        /// Signifie que ce bloc est vide, il est donc à 0.
        /// </summary>
        Empty,

        /// <summary>
        /// Signifie que ce bloc est rempli, il est donc à 1.
        /// </summary>
        SolidBlock,

        /// <summary>
        /// Signifie que ce bloc est en mouvement, il est donc à 2.
        /// </summary>
        MovingBlock
    }
}
