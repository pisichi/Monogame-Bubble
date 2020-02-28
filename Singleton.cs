using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Bobble_Game_Mid
{
    public class Singleton
    {
        public const int BOBBLESIZE = 60;


        public const int BoardWidth = 9 * BOBBLESIZE;
        public const int BoardHeight = 12 * BOBBLESIZE;

        public const int SCREENWIDTH = BoardWidth + 800;
        public const int SCREENHEIGHT = BoardHeight + 200;

        public static int _down = 0;


        public int Score;
        public int Level;



        public int[,] GameBoard;

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
