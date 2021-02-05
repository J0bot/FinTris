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
            { TetrominoType.Snake1, new byte[,]
                {
                    {0, 1, 1},
                    {1, 1, 0},
                }
            },

            { TetrominoType.ISnake, new byte[,]
                {
                    {0 ,1},
                    {1, 1},
                    {1, 0},
                }
            },
            { TetrominoType.ISnake1, new byte[,]
                {
                    {1, 1, 0},
                    {0, 1, 1},

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
            { TetrominoType.Lawlet1, new byte[,]
                {
                    {1 ,1, 1},
                    {1, 0, 0},
                    
                }
            },
            { TetrominoType.Lawlet2, new byte[,]
                {
                    {1 ,1},
                    {0, 1},
                    {0, 1},
                }
            },
            { TetrominoType.Lawlet3, new byte[,]
                {

                    {1 ,0, 0},
                    {1, 1, 1},
                }
            },

            { TetrominoType.ILawlet, new byte[,]
                {
                    {0 ,1},
                    {0, 1},
                    {1, 1},
                }
            },
            { TetrominoType.ILawlet1, new byte[,]
                {
                    {1 ,0, 0},
                    {1, 1, 1},

                }
            },
            { TetrominoType.ILawlet2, new byte[,]
                {
                    {1 ,1},
                    {1, 0},
                    {1, 0},
                }
            },
            { TetrominoType.ILawlet3, new byte[,]
                {

                    {1 ,1, 1},
                    {1, 0, 0},
                }
            },

            //Pyramid
            { TetrominoType.Pyramid, new byte[,]
                {
                    {0 ,1 ,0},
                    {1, 1, 1},
                }
            },
            { TetrominoType.Pyramid1, new byte[,]
                {
                    {1, 0},
                    {1, 1},
                    {1, 0}
                }
            },
            { TetrominoType.Pyramid2, new byte[,]
                {
                    {1 ,1 ,1},
                    {0, 1, 0},
                }
            },
            { TetrominoType.Pyramid3, new byte[,]
                {
                    {0, 1},
                    {1, 1},
                    {0, 1}
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
            { TetrominoType.Malong1, new byte[,]
                {
                    {1,1,1,1},
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

            Rotation = (RotationState)random.Next(4); //On balance notre rotation aléatoirement

        }

        /// <summary>
        /// Fonction qui permet de faire tourner le tetrominos
        /// </summary>
        public void Rotate()
        {
            int max = 1;
            int intType = (int)Type;

            if (intType > 0)
            {
                max = 2;
            }
            else if (intType > 6)
            {
                max = 4;
            }
            
            intType = (intType + 1) % max;
            Type = (TetrominoType)intType;
            Rotation = (RotationState)((((int)Rotation) + 1) % 4);
            Blocks = tetrominoShapes[(TetrominoType)(intType)];
        }
                                  
    }
}
