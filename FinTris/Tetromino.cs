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
            { TetrominoType.Pyramid, new byte[,]
                {
                    {0 ,1 ,0},
                    {1, 1, 1},
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

        public RotationState Rotation { get; private set; }
        public TetrominoType Type { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }
        public byte[,] Blocks { get; private set; }
        public TetrominoState State { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Type de notre tetromino (Square, L, Malong, etc...)</param>
        /// <param name="x">Position X de notre tetromino</param>
        /// <param name="y">Position Y de notre tetromino</param>
        public Tetromino(TetrominoType type, int x, int y)
        {
            Random random = new Random();
            Type = type;
            X = x;
            Y = y;
            Blocks = tetrominoShapes[type];

            Rotation = (RotationState)random.Next(4); // On balance notre rotation aléatoirement

        }

        /// <summary>
        /// Fonction qui permet de faire tourner le tetromino
        /// </summary>
        public void Rotate()
        {
            Rotation = (RotationState)(((int)Rotation + 1) % 4);
            //byte[,] newBytes = new byte[Blocks.GetLength(1), Blocks.GetLength(0)];
            //for (int y = 0; y < newBytes.GetLength(1); y++)
            //{
            //    for (int x = 0; x < newBytes.GetLength(0); x++)
            //    {
            //        newBytes[x, y] = 
            //    }
            //}
        }
                                  
    }
}
