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
        public readonly int x;
        public readonly int y;

        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 operator + (Vector2 v1 , Vector2 v2)
        {
            return new Vector2(v1.x + v2.x, v1.y + v2.y);
        }
    }
}
