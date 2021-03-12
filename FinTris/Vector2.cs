///ETML
///Auteur   	: José Carlos Gasser, Ahmad Jano, Maxime Andrieux, Maxence Weyermann, Larissa Debarros
///Date     	: 09.03.2021
///Description  : Fintris

namespace FinTris
{
    /// <summary>
    /// Classe qui réprésente des coordonées x et y
    /// </summary>
    public struct Vector2
    {
        /// <summary>
        /// Vector2 avec une direction vers de bas
        /// </summary>
        public static Vector2 Down = new Vector2(0, -1);

        /// <summary>
        /// Vector2 avec une direction vers la droite
        /// </summary>
        public static Vector2 Right = new Vector2(1, 0);

        /// <summary>
        /// Vector2 avec une direction vers la gauche
        /// </summary>
        public static Vector2 Left = new Vector2(-1, 0);

        /// <summary>
        /// Vector2 avec une direction vers le haut
        /// </summary>
        public static Vector2 Up = new Vector2(0, 1);

        /// <summary>
        /// Coordonée x du Vector2
        /// </summary>
        public readonly int x;

        /// <summary>
        /// Coordonée x du Vector2
        /// </summary>
        public readonly int y;

        /// <summary>
        /// Constructor renseigné pour instancier un nouveau Vector2
        /// </summary>
        /// <param name="x">Paramètre de coordonée x</param>
        /// <param name="y">Paramètre de coordonée y</param>
        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Méthode de surcharge pour l'opérateur '+' entre deux Vector2
        /// </summary>
        /// <param name="v1">Premier Vector2</param>
        /// <param name="v2">Deuxième Vector2</param>
        /// <returns>Retourne l'addition des deux vecteurs</returns>
        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x + v2.x, v1.y + v2.y); //Addition de deux Vector2
        }

        /// <summary>
        /// Méthode de surcharge pour l'opérateur '-' entre deux Vector2
        /// </summary>
        /// <param name="v1">Premier Vector2</param>
        /// <param name="v2">Deuxième Vector2</param>
        /// <returns>Retourne la soustraction des deux vecteurs</returns>
        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x - v2.x, v1.y - v2.y); //Soustraction de deux Vector2
        }
    }
}
