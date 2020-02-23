using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Bobble_Game_Mid
{
    class Singleton
    {
        public const int BOBBLESIZE = 60;

        public const int GAMEWIDTH = 8;
        public const int GAMEHEIGHT = 15;

        public const int SCREENWIDTH = BOBBLESIZE * (GAMEWIDTH + 5);
        public const int SCREENHEIGHT = BOBBLESIZE * GAMEHEIGHT;


        public const int BoardWidth = SCREENWIDTH;
        public const int BoardHeight = SCREENHEIGHT;

        public int Score;
        public int Level;
        public int LineDeleted;

        public KeyboardState PreviousKey, CurrentKey;

        public int[,] GameBoard;

        public Random Random = new Random();

        public enum GameState
        {
            GameMenu,
            GameTryAgain,
            GameBegin,
            GamePlaying,
            GamePaused,
            GameEnded
        }

        public GameState CurrentGameState;

        private static Singleton instance;

        private Singleton()
        {
        }

        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }
    }
}
