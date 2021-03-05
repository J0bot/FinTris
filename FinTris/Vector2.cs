
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTris
{
    /// <summary>
    /// Classe qui réprésente des coordonées x et y
    /// </summary>
    public class Vector2
    {
        public static Vector2 Down = new Vector2(0, -1);
        public static Vector2 Right = new Vector2(1, 0);
        public static Vector2 Left = new Vector2(-1, 0);
        public static Vector2 Up = new Vector2(0, 1);

        public readonly int x;
        public readonly int y;

        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x + v2.x, v1.y + v2.y);
        }
        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x - v2.x, v1.y - v2.y);


        }
    }
}
