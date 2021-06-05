/// ETML
/// Auteur   	: José Carlos Gasser, Ahmad Jano, Maxime Andrieux, Maxence Weyermann, Larissa Debarros
/// Date     	: 09.03.2021
/// Description  : Fintris

namespace FinTris
{
    /// <summary>
    /// Représente un vecteur bidimensionnel avec des coordonnées X et Y.
    /// </summary>
    public struct Vector2
    {
        /// <summary>
        /// Vecteur zero.
        /// </summary>
        public static Vector2 Zero = new Vector2(0, 0);

        /// <summary>
        /// Vecteur normalisé.
        /// </summary>
        public static Vector2 One = new Vector2(1, 1);

        /// <summary>
        /// Vecteur normalisé orienté vers le bas.
        /// </summary>
        public static Vector2 Down = new Vector2(0, -1);

        /// <summary>
        /// Vecteur normalisé orienté vers la droite.
        /// </summary>
        public static Vector2 Right = new Vector2(1, 0);

        /// <summary>
        /// Vecteur normalisé orienté vers le bas.
        /// </summary>
        public static Vector2 Left = new Vector2(-1, 0);

        /// <summary>
        /// Vecteur normalisé orienté vers le haut.
        /// </summary>
        public static Vector2 Up = new Vector2(0, 1);

        /// <summary>
        /// La coordonnée X du vecteur.
        /// </summary>
        public readonly int x;

        /// <summary>
        /// La coordonnée Y du vecteur.
        /// </summary>
        public readonly int y;

        /// <summary>
        /// Constructor renseigné pour instancier un nouveau vecteur.
        /// </summary>
        /// <param name="x">Paramètre de coordonnée X.</param>
        /// <param name="y">Paramètre de coordonnée Y.</param>
        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Permet d'additionner deux vecteurs.
        /// </summary>
        /// <param name="v1">Premier Vector2.</param>
        /// <param name="v2">Deuxième Vector2.</param>
        /// <returns>Retourne le résultat en Vector2 de l'addition des deux vecteurs.</returns>
        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x + v2.x, v1.y + v2.y);
        }

        /// <summary>
        /// Permet de soustraire deux vecteurs.
        /// </summary>
        /// <param name="v1">Premier Vector2.</param>
        /// <param name="v2">Deuxième Vector2.</param>
        /// <returns>Retourne le résultat en Vector2 de la soustraction des deux vecteurs.</returns>
        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x - v2.x, v1.y - v2.y);
        }

        /// <summary>
        /// Permet de multiplier les coordonnées d'un vecteurs.
        /// </summary>
        /// <param name="v">Le vecteur.</param>
        /// <param name="scale">Le multiplicateur.</param>
        /// <returns>Retourne le résultat en Vector2 de la multiplication d'un vecteur.</returns>
        public static Vector2 operator *(Vector2 v, int scale)
        {
            return new Vector2(v.x * scale, v.y * scale);
        }
    }
}
