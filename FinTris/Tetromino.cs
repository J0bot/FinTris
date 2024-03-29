﻿/// ETML
/// Auteur   	 : José Carlos Gasser, Ahmad Jano, Maxime Andrieux, Maxence Weyermann, Larissa Debarros
/// Date     	 : 09.03.2021
/// Description  : Fintris

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
        private readonly static Dictionary<TetrominoType, byte[,]> _tetrominoShapes = new Dictionary<TetrominoType, byte[,]>
        {
            //Carré
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

            //Snake inversé
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

            //Lawlet inversé
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

        private readonly static Dictionary<TetrominoType, ConsoleColor> _tetrominoColors = new Dictionary<TetrominoType, ConsoleColor>()
        {
            { TetrominoType.Squarie, ConsoleColor.Yellow},
            { TetrominoType.Snake, ConsoleColor.Green},
            { TetrominoType.ISnake, ConsoleColor.DarkRed},
            { TetrominoType.Malong, ConsoleColor.Cyan},
            { TetrominoType.Lawlet, ConsoleColor.DarkCyan},
            { TetrominoType.ILawlet, ConsoleColor.DarkYellow},
            { TetrominoType.Pyramid, ConsoleColor.DarkMagenta},
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
        private readonly TetrominoType _shape;

        /// <summary>
        /// Position du Tetromino dans deux dimensions
        /// </summary>
        private Vector2 _position;

        /// <summary>
        /// L'ancienne position du tetromino.
        /// </summary>
        private Vector2 _prevPosition;

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
        /// Largeur du Tetromino
        /// </summary>
        public int Width
        {
            get { return _width; }
        }

        /// <summary>
        /// Hauteur du Tetromino
        /// </summary>
        public int Height
        {
            get { return _height; }
        }

        /// <summary>
        /// Position du tetromino.
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
        }

        /// <summary>
        /// L'ancienne position du tetromino.
        /// </summary>
        public Vector2 PreviousPosition
        {
            get { return _prevPosition; }
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
        /// Type de forme du Tetromino
        /// </summary>
        public TetrominoType Shape
        {
            get { return _shape; }
        }

        /// <summary>
        /// Couleur du Tetromino.
        /// </summary>
        public ConsoleColor Color
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
        /// <param name="initalPosition">position initiale du tetromino en Vector2</param>
        public Tetromino(TetrominoType type, Vector2 initalPosition)
        {
            _shape = type;
            _data = _tetrominoShapes[type];
            _position = initalPosition;
            _state = TetrominoState.Moving;

            _width = _data.GetLength(0);
            _height = _data.GetLength(1);

            _blocks = new List<Vector2>();

            // Attribuer la couleur au Tetromino.
            _tetrominoColor = _tetrominoColors[type];

            UpdateBlocks();
        }

        /// <summary>
        /// Met à jour les nouvelles positions relatives des carrés du tetromino.
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
            if (_shape == TetrominoType.Squarie)
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

        /// <summary>
        /// Déplacer le tetromino.
        /// </summary>
        /// <param name="direction">La direction de déplacement.</param>
        public void Move(Vector2 direction)
        {
            _prevPosition = _position;
            _position += direction;
        }
    }
}
