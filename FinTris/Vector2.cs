namespace FinTris
{
    public class Vector2
    {
        public static Vector2 Zero  =   new Vector2(0, 0);
        public static Vector2 Up    =   new Vector2(0, 1);
        public static Vector2 Down  =   new Vector2(0, -1);
        public static Vector2 Right =   new Vector2(1, 0);
        public static Vector2 Left  =   new Vector2(-1, 0);

        public readonly int X;
        public readonly int Y;

        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
