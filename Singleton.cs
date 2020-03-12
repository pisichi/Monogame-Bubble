using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bubble_Game_Mid.gameObject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Bubble_Game_Mid
{
    public class Singleton
    {
        public const int BUBBLESIZE = 60;
        public const int BoardWidth = 9 * BUBBLESIZE;
        public const int BoardHeight = 12 * BUBBLESIZE;
        public const int SCREENWIDTH = BoardWidth + 1200;
        public const int SCREENHEIGHT = BoardHeight + 200;

        public static int ScreenDown;
        public static Color CurrentColor;
        public static int Score;
        public static int Charge;
        public bool ult;

        public Bubble[,] GameBoard = new Bubble[18, 9];


        public enum GameState
        {
            GameMenu,
            GameControl,
            GameAbout,
            GamePlaying,
            GamePaused,
            GameLose,
            GameWin
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
