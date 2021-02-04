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
                    {1, 1, 0, 0}, 
                    {1, 1, 0, 0}, 
                    {0, 0, 0, 0}, 
                    {0, 0, 0, 0} 
                }
            },
            { TetrominoType.Snake, new byte[,]
                {
                    {1 ,0},
                    {1, 1},
                    {0, 1},
                }
            },
            { TetrominoType.Lawlet, new byte[,]
                {
                    {1 ,0 ,0 ,0},
                    {1, 0, 0, 0},
                    {1, 1, 0, 0},
                    {0, 0, 0, 0}
                }
            },
            { TetrominoType.ISnake, new byte[,]
                {
                    {0 ,1 ,0 ,0},
                    {1, 1, 0, 0},
                    {1, 0, 0, 0},
                    {0, 0, 0, 0}
                }
            },
            { TetrominoType.ILawlet, new byte[,]
                {
                    {0 ,1 ,0 ,0},
                    {0, 1, 0, 0},
                    {1, 1, 0, 0},
                    {0, 0, 0, 0}
                }
            },
            { TetrominoType.Pyramid, new byte[,]
                {
                    {0 ,1 ,0 ,0},
                    {1, 1, 1, 0},
                    {0, 0, 0, 0},
                    {0, 0, 0, 0}
                }
            },
            { TetrominoType.Malong, new byte[,]
                {
                    {1 ,0 ,0 ,0},
                    {1, 0, 0, 0},
                    {1, 0, 0, 0},
                    {1, 0, 0, 0}
                }
            },
        };

        public RotationState Rotation { get; set; }
        public TetrominoType Type { get; }
        public int X { get; set; }
        public int Y { get; set; }
        public byte[,] Blocks { get; }




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

            Rotation = (RotationState)random.Next(4); //On balance notre rotation aléatoirement

        }
                                  
    }
}
