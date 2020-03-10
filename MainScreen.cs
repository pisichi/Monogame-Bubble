using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Bobble_Game_Mid.gameObject;
using System.Threading;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace Bobble_Game_Mid
{
     class MainScreen : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private List<GameObject> _gameObjects;
        private Bubble[,] GameBoard = new Bubble[18, 9];
        private Vector2 textSize;

        float tick = 0;
        float _tick = 0;

        Color _chargeColor;
        Random rnd = new Random();

        #region Texture
        Texture2D _bubble;
        Texture2D _bg;
        Texture2D _menuBG;
        Texture2D _border;
        Texture2D _head;
        Texture2D _moutain1;
        Texture2D _moutain2;
        Texture2D _water;
        Texture2D _headColor;
        Texture2D _body;
        Texture2D _bodyColor;
        Texture2D _tail;
        Texture2D _tailColor;
        Texture2D _overlay;
        Texture2D _scroll;
        Texture2D _gauge;
        Texture2D _charge;
        Texture2D _controlBG;

        SpriteFont _font;
        #endregion

        #region sound
        Song music;
        SoundEffect _pop;
        SoundEffect _hit;
        SoundEffect _recharge;
        SoundEffect _score;
        SoundEffect _shoot;
        SoundEffect _shoot_S;
        SoundEffect _skill;
        SoundEffect _skill_S;
        SoundEffect _victory;
        #endregion

        Button btn_play;
        Button btn_exit;
        Button btn_resume;
        Button btn_back;
        Button btn_control;

        private KeyboardState _currentkey;
        private KeyboardState _previouskey;
        MouseState _currentmouse;
        MouseState _previousmouse;

        public MainScreen()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content"; 
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = Singleton.SCREENWIDTH;
            graphics.PreferredBackBufferHeight = Singleton.SCREENHEIGHT;
            graphics.ApplyChanges();
            Singleton.Instance.CurrentGameState = Singleton.GameState.GameMenu;
            TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d);
            this.IsMouseVisible = true;
            IsFixedTimeStep = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
        #region Load
            spriteBatch = new SpriteBatch(GraphicsDevice);
            _bubble = this.Content.Load<Texture2D>("sprite/ball");
            _bg = this.Content.Load<Texture2D>("sprite/bg");
            _menuBG = this.Content.Load<Texture2D>("sprite/menuBG");
            _controlBG = this.Content.Load<Texture2D>("sprite/control");
            _overlay = this.Content.Load<Texture2D>("sprite/overlay");
            _scroll = this.Content.Load<Texture2D>("sprite/scroll");
            _border = this.Content.Load<Texture2D>("sprite/frame");

            _head = this.Content.Load<Texture2D>("sprite/head");
            _headColor = this.Content.Load<Texture2D>("sprite/head_color");
            _body = this.Content.Load<Texture2D>("sprite/body");
            _bodyColor = this.Content.Load<Texture2D>("sprite/body_color");
            _tail = this.Content.Load<Texture2D>("sprite/tail");
            _tailColor = this.Content.Load<Texture2D>("sprite/tail_color");

            _moutain1 = this.Content.Load<Texture2D>("sprite/mou1");
            _moutain2 = this.Content.Load<Texture2D>("sprite/mou2");
            _water = this.Content.Load<Texture2D>("sprite/water");

            _gauge = this.Content.Load<Texture2D>("sprite/gauge");
            _charge = this.Content.Load<Texture2D>("sprite/charge");
            
            
            _font = Content.Load<SpriteFont>("font/font");

            music = this.Content.Load<Song>("music/hua sui yue");

            _pop = Content.Load<SoundEffect>("sfx/bubble_pop");
            _hit = Content.Load<SoundEffect>("sfx/hit_bubble");
            _recharge = Content.Load<SoundEffect>("sfx/recharge");
            _score = Content.Load<SoundEffect>("sfx/score");
            _shoot = Content.Load<SoundEffect>("sfx/shoot");
            _shoot_S = Content.Load<SoundEffect>("sfx/shoot_special");
            _skill = Content.Load<SoundEffect>("sfx/skill_swap");
            _skill_S = Content.Load<SoundEffect>("sfx/skill_special");
            _victory = Content.Load<SoundEffect>("sfx/victory");

            #endregion
            //MediaPlayer.Play(music);
            //MediaPlayer.IsRepeating = true;

           btn_play = new Button(_charge,_font, graphics.GraphicsDevice);
           btn_exit = new Button(_charge,_font, graphics.GraphicsDevice);
           btn_resume = new Button(_charge,_font, graphics.GraphicsDevice);
           btn_back = new Button(_charge,_font, graphics.GraphicsDevice);
           btn_control = new Button(_charge,_font, graphics.GraphicsDevice);

            _gameObjects = new List<GameObject>()
            {
                new Dragon(_head,_headColor,_shoot,_shoot_S,_skill,_skill_S)
                {
                    Position = new Vector2(Singleton.SCREENWIDTH/2,Singleton.SCREENHEIGHT-100),
                    Bubble = new Bubble(_bubble,_font,_hit)
                }
            };
            for (int i = 0; i < 5; i += 1)
            {
                for (int j = 0; j < 9 - (i % 2); j += 1)
                {

                    GameBoard[i, j] = new Bubble(_bubble, _font,_hit)
                    {

                        Position = new Vector2(600 + 15 + j * (Singleton.BUBBLESIZE + 5) + ((i % 2) == 0 ? 0 : 30), 100 + i * (Singleton.BUBBLESIZE)),
                        IsActive = false,
                        _color = GetRandomColor(),
                        Location = new Vector2(i, j),
                        Isshooting = false
                };
                    _gameObjects.Add(GameBoard[i, j]);
                }

            }
        }

        protected override void UnloadContent()
        {
            for (int i = 0; i < _gameObjects.Count; i++)
            {
                if (_gameObjects[i].IsRemove)
                {
                    _gameObjects.RemoveAt(i);
                    i--;
                    Singleton.Score += 100;
                    _pop.Play();
                }
                if (_gameObjects.Count < 2)
                {
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GameWin;
                }
            }

        }

        protected override void Update(GameTime gameTime)
        {

            _previouskey = _currentkey;
            _currentkey = Keyboard.GetState();
           
            _previousmouse = _currentmouse;
            _currentmouse = Mouse.GetState();

            switch (Singleton.Instance.CurrentGameState)
            {
                case Singleton.GameState.GameMenu:
                    UpdateMenu(gameTime);
                    break;
                case Singleton.GameState.GamePlaying:
                    UpdatePlaying(gameTime);
                    break;
                case Singleton.GameState.GameWin:
                case Singleton.GameState.GameLose:
                case Singleton.GameState.GamePaused:
                    UpdatePause(gameTime);
                    break;
                case Singleton.GameState.GameControl:
                    UpdateControl(gameTime);
                    break;
            }
            UnloadContent();
            base.Update(gameTime);
        }

       

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            switch (Singleton.Instance.CurrentGameState)
            {
                case Singleton.GameState.GameMenu:
                    DrawMenu();
                    break;
                case Singleton.GameState.GameControl:
                    DrawMenu();
                    DrawControl();
                    break;
                case Singleton.GameState.GamePlaying:
                    DrawPlaying();
                    break;
                case Singleton.GameState.GamePaused:
                    DrawPlaying();
                    DrawOverlay("Game paused");
                    break;
                case Singleton.GameState.GameLose:
                    DrawPlaying();
                    DrawOverlay("You Lose");
                    break;
                case Singleton.GameState.GameWin:
                    DrawPlaying();
                    DrawOverlay("You Win");
                    break;
            }
            spriteBatch.End();
            graphics.BeginDraw();
            base.Draw(gameTime);
        }

        public Color GetRandomColor()
        {
            Color _color = Color.White;
            switch (rnd.Next(0, 6))
            {
                case 0:
                    _color = Color.White;
                  
                    break;
                case 1:
                    _color = Color.Blue;

                    break;
                case 2:
                    _color = Color.Yellow;

                    break;
                case 3:
                    _color = Color.Red;

                    break;
                case 4:
                    _color = Color.Green;

                    break;
                case 5:
                    _color = Color.Purple;
                    break;
            }
            return _color;
        }

        private void UpdatePlaying(GameTime gameTime)
        {
            if (_currentkey.IsKeyDown(Keys.Escape) && _previouskey.IsKeyUp(Keys.Escape))
                Singleton.Instance.CurrentGameState = Singleton.GameState.GamePaused;

            _tick += gameTime.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond;
            if (_tick >= 3 && Singleton.Charge < 12)
            {
                _recharge.Play();
                Singleton.Charge += 1;
                _tick = 0;
            }

          if( Singleton.Charge < 12)
            {
                _chargeColor = Color.Yellow;
            }
            else if(Singleton.Charge == 12)
            {
                _chargeColor = GetRandomColor();
            }

            tick += gameTime.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond;
            if (tick >= 20)
            {
                Singleton.ScreenDown += 60;

                for (int i = 0; i < 18; i += 1)
                {
                    for (int j = 0; j < 9 - (i % 2); j += 1)
                    {
                        if (GameBoard[i, j] != null)
                        {
                            GameBoard[i, j].Position.Y += 60;
                            if (GameBoard[i, j].Position.Y > Singleton.BoardHeight)
                                Singleton.Instance.CurrentGameState = Singleton.GameState.GameLose;
                        }
                    }
                }
                tick = 0;
            }

            foreach (var gameobject in _gameObjects.ToArray())
            {
                gameobject.Update(gameTime, _gameObjects, GameBoard);
            }

        }

        private void DrawPlaying()
        {
            spriteBatch.Draw(_bg, destinationRectangle: new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT));
            spriteBatch.Draw(_water, destinationRectangle: new Rectangle(0, Singleton.SCREENHEIGHT - 130, Singleton.SCREENWIDTH, 50));
            foreach (var gameobject in _gameObjects)
            {
                gameobject.Draw(spriteBatch);
            }
            spriteBatch.Draw(_moutain1, destinationRectangle: new Rectangle(0, 50, 500, 900));
            spriteBatch.Draw(_moutain2, destinationRectangle: new Rectangle(1250, 50, 500, 860));
            spriteBatch.Draw(_border, destinationRectangle: new Rectangle(385, Singleton.ScreenDown - 120, 980, Singleton.BoardHeight + 300));

            spriteBatch.Draw(_body, new Vector2(1000, 800), null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.Draw(_bodyColor, new Vector2(1000, 800), null, Singleton.CurrentColor, 0f, Vector2.Zero, 1, SpriteEffects.None, 0);


            spriteBatch.Draw(_water, destinationRectangle: new Rectangle(0, Singleton.SCREENHEIGHT - 100 , Singleton.SCREENWIDTH, 50));

            spriteBatch.Draw(_tail, new Vector2 (1300,670 ), Color.White);
            spriteBatch.Draw(_tailColor, new Vector2(1300, 670), Singleton.CurrentColor);


            spriteBatch.Draw(_water, destinationRectangle: new Rectangle(0, Singleton.SCREENHEIGHT - 70 , Singleton.SCREENWIDTH, 50));


            spriteBatch.Draw(_body, new Vector2(300, 850),null, Color.White, 0f, Vector2.Zero, 1.25f, SpriteEffects.None, 0);
            spriteBatch.Draw(_bodyColor, new Vector2(300, 850),null, Singleton.CurrentColor, 0f, Vector2.Zero, 1.25f, SpriteEffects.None, 0);

            spriteBatch.Draw(_water, destinationRectangle: new Rectangle(0, Singleton.SCREENHEIGHT - 40 , Singleton.SCREENWIDTH, 50));
            spriteBatch.Draw(_body, new Vector2(800, 850), null, Color.White, 0.3f, Vector2.Zero, 1.5f, SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(_bodyColor, new Vector2(800, 850), null, Singleton.CurrentColor, 0.3f, Vector2.Zero, 1.5f, SpriteEffects.FlipHorizontally, 0);

            spriteBatch.DrawString(_font, " " + Singleton.Score, new Vector2(200, 700), Color.White);

            spriteBatch.Draw(_gauge, destinationRectangle: new Rectangle(20,750 , 350, 80));
            for (int i = 0; i < Singleton.Charge; i++)
            {
                spriteBatch.Draw(_charge, destinationRectangle: new Rectangle(160 + i * 15, 772, 10, 35),color: _chargeColor);
            }

        }




        private void UpdateMenu(GameTime gameTime)

           
        {
            if (_currentkey.IsKeyDown(Keys.Enter))
            {
                Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
            }

            if(btn_play.isClick == true) Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
            btn_play.update(_currentmouse, _previousmouse);

            if (btn_control.isClick == true)
            {
                Singleton.Instance.CurrentGameState = Singleton.GameState.GameControl;
                btn_control.isClick = false;
            }
            btn_control.update(_currentmouse, _previousmouse);

            if (btn_exit.isClick == true) Exit();
            btn_exit.update(_currentmouse, _previousmouse);

        }

        void DrawMenu()
        {
            spriteBatch.Draw(_menuBG, destinationRectangle: new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT));
            btn_play.Draw(spriteBatch);
            btn_play.set(new Vector2(425,350),"play");
            btn_control.Draw(spriteBatch);
            btn_control.set(new Vector2(425, 450), "control");
            btn_exit.Draw(spriteBatch);
            btn_exit.set(new Vector2(425, 550), "exit");
        }



        private void UpdatePause(GameTime gameTime)
        {

            if (_currentkey.IsKeyDown(Keys.Escape) && _previouskey.IsKeyUp(Keys.Escape))
            {
                Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                _victory.Play();
            }
            if (btn_exit.isClick == true) Exit();
            btn_exit.update(_currentmouse, _previousmouse);

            if (btn_back.isClick == true)
            {
                Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                btn_back.isClick = false;
            }
            btn_back.update(_currentmouse, _previousmouse);

        }

        private void DrawOverlay(string dia)
        {
            spriteBatch.Draw(_overlay, destinationRectangle: new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT),color: Color.Black);
            spriteBatch.Draw(_scroll, destinationRectangle: new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT));
            spriteBatch.DrawString(_font, ""+ dia,  new Vector2(Singleton.SCREENWIDTH/2, Singleton.SCREENHEIGHT /2 - 100) - _font.MeasureString(dia) /2 , Color.Red);
            spriteBatch.DrawString(_font, "Score: " + Singleton.Score, new Vector2(Singleton.SCREENWIDTH / 2, Singleton.SCREENHEIGHT/2) - _font.MeasureString("Score: " + Singleton.Score) / 2, Color.DarkRed);
            btn_exit.Draw(spriteBatch);
            btn_exit.set(new Vector2(Singleton.SCREENWIDTH / 2 - 60, 600), "exit");

            if (dia == "Game paused")
            {
                btn_back.Draw(spriteBatch);
                btn_back.set(new Vector2(Singleton.SCREENWIDTH / 2 - 90, 530), "resume");
            }
        }



        private void UpdateControl(GameTime gameTime)
        {
            if (btn_back.isClick == true)
            {
                Singleton.Instance.CurrentGameState = Singleton.GameState.GameMenu;
                btn_back.isClick = false;
            }
            btn_back.update(_currentmouse, _previousmouse);
        }

        private void DrawControl()
        {
            spriteBatch.Draw(_controlBG, destinationRectangle: new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT));
            btn_back.Draw(spriteBatch);
            btn_back.set(new Vector2(Singleton.SCREENWIDTH/2 - 75, 750) ,"back");

        }
    }
}
