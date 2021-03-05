namespace FinTris
{
    public class Matrix2
    {
        private readonly int[,] mat;

        public int this[int x, int y]
        {
            get => mat[x, y];
        }

        public Matrix2(int m00, int m10, int m01, int m11)
        {
            mat = new int[2, 2];
            mat[0, 0] = m00;
            mat[1, 0] = m10;
            mat[0, 1] = m01;
            mat[1, 1] = m11;
        }

        public static Vector2 operator *(Vector2 vec, Matrix2 mat)
        {
            return new Vector2(mat[0, 0] * vec.X + mat[1, 0] * vec.Y, mat[0, 1] * vec.X + mat[1, 1] * vec.Y);
        }
    }
}
