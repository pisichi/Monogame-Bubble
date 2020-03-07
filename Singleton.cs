﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Bobble_Game_Mid
{
    public class Singleton
    {
        public const int BUBBLESIZE = 60;
        public const int BoardWidth = 9 * BUBBLESIZE;
        public const int BoardHeight = 12 * BUBBLESIZE;
        public const int SCREENWIDTH = BoardWidth + 1200;
        public const int SCREENHEIGHT = BoardHeight + 200;

        public static int _down = 0;
        public static Color CurrentColor;
        public static int Score;
        public int Level;

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
