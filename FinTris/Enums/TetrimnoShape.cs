/// ETML
/// Auteur   	 : José Carlos Gasser, Ahmad Jano, Maxime Andrieux, Maxence Weyermann, Larissa Debarros
/// Date     	 : 09.03.2021
/// Description  : Fintris

namespace FinTris
{
    /// <summary>
    /// Enum de toutes les formes qu'un Tetromino pourrait avoir.
    /// </summary>
    public enum TetrominoType
    {
        /// <summary>
        /// Le Tetromino Squarie (0) a la forme d'un carré :
        /// 
        /// {1,1}
        /// {1,1}
        /// </summary>
        Squarie,

        /// <summary>
        /// Le Tetromino Snake (1) est un Snake :
        /// 
        /// {1 ,0},
        /// {1, 1},
        /// {0, 1},
        /// </summary>
        Snake,

        /// <summary>
        /// Le Tetromino ISnake (2) est un Snake inversé :
        /// 
        /// {0 ,1},
        /// {1, 1},
        /// {1, 0},
        /// </summary>
        ISnake,

        /// <summary>
        /// Le Tetromino Malong (3) est une longue barre :
        /// 
        /// {1},
        /// {1},
        /// {1},
        /// {1}
        /// </summary>
        Malong,

        /// <summary>
        /// Le tetromino Lawlet (4) a la forme d'un L :
        /// 
        /// {1 ,0},
        /// {1, 0},
        /// {1, 1},
        /// </summary>
        Lawlet,

        /// <summary>
        /// Le tetromino ILawlet (5) a la forme d'un L inversé :
        /// 
        /// {0 ,1},
        /// {0, 1},
        /// {1, 1},
        /// </summary>
        ILawlet,

        /// <summary>
        /// Le tetromino Pyramid (6) a la forme d'une pyramide :
        /// 
        /// {0 ,1, 0},
        /// {1, 1, 1},
        /// </summary>
        Pyramid,

    }

    
}
