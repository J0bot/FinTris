
///Auteur   	: José Carlos Gasser, Ahmad Jano, Maxime Andrieux, Maxence Weyermann, Larissa Debarros
///Date     	: 09.03.2021
///Description  : Fintris

using System;
using System.Collections.Generic;

namespace FinTris
{
    /// <summary>
    /// Classe qui représente le Tetromino avec une position par rapport au plateau de jeu.
    /// </summary>
    public class Tetromino
    {
        /// <summary>
        /// Dictionnaire qui contient les données des blocs de chaque forme de Tetromino.
        /// </summary>
        private readonly static Dictionary<TetrominoShape, byte[,]> _tetrominoShapes = new Dictionary<TetrominoShape, byte[,]>
        {
            { TetrominoShape.Squarie, new byte[,]
                {
                    {1, 1},
                    {1, 1},
                }
            },
            //Snake
            { TetrominoShape.Snake, new byte[,]
                {
                    {1 ,0},
                    {1, 1},
                    {0, 1},
                }
            },



            { TetrominoShape.ISnake, new byte[,]
                {
                    {0 ,1},
                    {1, 1},
                    {1, 0},
                }
            },

            //Lawlet
            { TetrominoShape.Lawlet, new byte[,]
                {
                    {1 ,0},
                    {1, 0},
                    {1, 1},
                }
            },


            { TetrominoShape.ILawlet, new byte[,]
                {
                    {0 ,1},
                    {0, 1},
                    {1, 1},
                }
            },

            //Pyramid
            { TetrominoShape.Pyramid, new byte[2, 3]
                {
                    {1, 1, 1},
                    {0 ,1 ,0},
                }
            },

            //Malong
            { TetrominoShape.Malong, new byte[,]
                {
                    {1},
                    {1},
                    {1},
                    {1}
                }
            },
        };

        /// <summary>
        /// Largeur du Tetromino.
        /// </summary>
        private int _width;

        /// <summary>
        /// Longeur du Tetromino.
        /// </summary>
        private int _height;

        /// <summary>
        /// Forme du Tetromino
        /// </summary>
        private TetrominoShape _shape;

        /// <summary>
        /// Position du Tetromino dans deux dimensions
        /// </summary>
        private Vector2 _position;

        /// <summary>
        /// tableau qui aide aux collisions
        /// </summary>
        private byte[,] _data;

        /// <summary>
        /// Etat de mouvement du Tetromino
        /// </summary>
        private TetrominoState _state;

        /// <summary>
        /// Couleur du Tetromino
        /// </summary>
        private ConsoleColor _tetrominoColor;

        /// <summary>
        /// Liste des positions des blocs du Tetromino
        /// </summary>
        private List<Vector2> _blocks;

        /// <summary>
        /// Variable random qui permet de générer des nombres random
        /// </summary>
        private Random _random;

        /// <summary>
        /// Position du tetromino.
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// Etat de mouvement du Tetromino. 
        /// </summary>
        public TetrominoState State
        {
            get { return _state; }
            set { _state = value; }
        }

        /// <summary>
        /// Couleur du Tetromino.
        /// </summary>
        public ConsoleColor TetrominoColor
        {
            get { return _tetrominoColor; }
            set { _tetrominoColor = value; }
        }

        /// <summary>
        /// Liste des positions des bloca relatives au tetromino.
        /// </summary>
        public List<Vector2> Blocks
        {
            get { return _blocks; }
            set { _blocks = value; }
        }



        /// <summary>
        /// Permet de créer une nouvelle instance de Tetromino.
        /// </summary>
        /// <param name="type">Type de notre tetromino (Square, L, Malong, etc...)</param>
        /// <param name="x">Position X de notre tetromino</param>
        /// <param name="y">Position Y de notre tetromino</param>
        /// <param name="tetrominoColor">Couleur du Tetromino</param>

        public Tetromino(TetrominoShape type, int x = 0, int y = 0, ConsoleColor tetrominoColor = ConsoleColor.Blue)
        {
            Random random = new Random();
            _shape = type;
            _position = new Vector2(x, y);
            _data = _tetrominoShapes[type];

            _width = _data.GetLength(0);
            _height = _data.GetLength(1);

            _blocks = new List<Vector2>();

            

            TetrominoColor = (ConsoleColor)random.Next(9, 15);

            UpdateBlocks();
        }

        /// <summary>
        /// Met à jour les nouvelles positions des carrés du tetromino.
        /// </summary>
        private void UpdateBlocks()
        {
            _blocks.Clear();
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    if (_data[x, y] == 1)
                    {
                        _blocks.Add(new Vector2(x, y));
                    }
                }
            }
        }

        /// <summary>
        /// Effectue une rotation de 90 degrés d'un Tetromino.
        /// </summary>
        public void Rotate()
        {

            if (_shape == TetrominoShape.Squarie)
            {
                return;
            }

            int newWidth = _height;
            int newHeight = _width;

            byte[,] newData = new byte[newWidth, newHeight];

            for (int x = 0; x < newWidth; x++)
            {
                for (int y = 0; y < newHeight; y++)
                {
                    newData[x, y] = _data[_width - 1 - y, x];
                }
            }

            _width = newWidth;
            _height = newHeight;
            _data = newData;

            UpdateBlocks();

        }


        public int Height
        {
            get { return _height; }
        }
    }
}
