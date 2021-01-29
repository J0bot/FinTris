namespace FinTris
{
    class Tetromino
    {
        public static byte[,] S = new byte[4, 4]
        {
            {1 ,0, 0, 0 },
            {1 ,1, 0, 0 },
            {0 ,1, 0, 0 },
            {0 ,0, 0, 0 }
        };

        //public static byte[] S = new byte[16] { 1, 0, 0, 0, 1, 1, 0,0 };
        //{0,1,1,0}
        //{1,1,0,0}
        //{0,0,0,0}
        //{0,0,0,0}

        public static byte[,] L = new byte[4, 4]
        {
            { 1,0,0,0 },
            { 1,0,0,0 },
            { 1,1,0,0 },
            { 0,0,0,0 }
        };

    public TetrominoType Type { get; }
        public int X { get; set; }
        public int Y { get; set; }

        public byte[,] blocks { get; }

        public Tetromino(TetrominoType type, int x, int y)
        {
            Type = type;
            X = x;
            Y = y;
            blocks = S;
        }
    }

    enum TetrominoType
    {
        RED,
        BLUE,

    }

    enum TetrominoState
    {
        _0,
        _90,
        _180,
        _270
    }
}
