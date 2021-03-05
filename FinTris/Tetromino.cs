using System;
using System.Collections.Generic;

namespace FinTris
{
    public class Tetromino
    {

        readonly static Dictionary<TetrominoType, byte[,]> tetrominoShapes = new Dictionary<TetrominoType, byte[,]>
        {
            { TetrominoType.Squarie, new byte[,]
                { 
                    {1, 1}, 
                    {1, 1},                    
                }
            },
            //Snake
            { TetrominoType.Snake, new byte[,]
                {
                    {1 ,0},
                    {1, 1},
                    {0, 1},
                }
            },


            { TetrominoType.ISnake, new byte[,]
                {
                    {0 ,1},
                    {1, 1},
                    {1, 0},
                }
            },

            //Lawlet
            { TetrominoType.Lawlet, new byte[,]
                {
                    {1 ,0},
                    {1, 0},
                    {1, 1},
                }
            },


            { TetrominoType.ILawlet, new byte[,]
                {
                    {0 ,1},
                    {0, 1},
                    {1, 1},
                }
            },

            //Pyramid
            { TetrominoType.Pyramid, new byte[2, 3]
                {
                    {1, 1, 1},
                    {0 ,1 ,0},
                }
            },

            //Malong
            { TetrominoType.Malong, new byte[,]
                {
                    {1},
                    {1},
                    {1},
                    {1}
                }
            },
        };

        //private List<int> SnakeRot = new List<int>
        //{
        //    1,2
        //};

        public int Width { get; private set; }
        public int Height { get; private set; }
        public RotationState Rotation { get; private set; }
        public TetrominoType Type { get; private set; }

        public Vector2 Position { get; set; }
        private byte[,] data { get; set; }
        public TetrominoState State { get; set; }
        public ConsoleColor TetrominoColor { get; set; }

        private List<Vector2> _blocks;

        public List<Vector2> Blocks
        {
            get { return _blocks; }
            set { _blocks = value; }
        }




        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Type de notre tetromino (Square, L, Malong, etc...)</param>
        /// <param name="x">Position X de notre tetromino</param>
        /// <param name="y">Position Y de notre tetromino</param>
        public Tetromino(TetrominoType type, int x, int y, ConsoleColor tetrominoColor = ConsoleColor.Blue, TetrominoState tetrominoState = TetrominoState.Moving)
        {
            Random random = new Random();
            Type = type;
            Position = new Vector2(x, y);
            data = tetrominoShapes[type];

            Width = data.GetLength(0);
            Height = data.GetLength(1);

            _blocks = new List<Vector2>();

            Rotation = (RotationState)random.Next(4); //On balance notre rotation aléatoirement

            TetrominoColor = (ConsoleColor)random.Next(9,15);

            UpdateBlocks();
        }


        private void UpdateBlocks()
        {
            _blocks.Clear();
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (data[x, y] == 1)
                    {
                        _blocks.Add(new Vector2(x, y));
                    }
                }
            }
        }

        #region CrapWay for rotation
        /// <summary>
        /// Fonction qui permet de faire tourner le tetrominos
        /// </summary>
        //public void Rotate()
        //{
        //    int max = 1;
        //    int intType = (int)Type;

        //    if (intType > 0)
        //    {
        //        max = 2;
        //    }
        //    else if (intType > 6)
        //    {
        //        max = 4;
        //    }

        //    intType = (intType + 1) % max;
        //    Type = (TetrominoType)intType;
        //    Rotation = (RotationState)((((int)Rotation) + 1) % 4);
        //    data = tetrominoShapes[(TetrominoType)(intType)];
        //}
        #endregion


        
        
        public void Rotate()
        {
            if (Type==TetrominoType.Squarie)
            {
                return;
            }

            int newWidth = Height;
            int newHeight = Width;

            byte[,] newData = new byte[newWidth, newHeight];

            for (int x = 0; x < newWidth; x++)
            {
                for (int y = 0; y < newHeight; y++)
                {
                    newData[x, y] = data[Width - 1 - y, x];
                }
            }

            Width = newWidth;
            Height = newHeight ;
            data = newData;

            UpdateBlocks();
        }
    }
}
