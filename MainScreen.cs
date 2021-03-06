﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Bubble_Game_Mid.gameObject;
using System.Threading;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace Bubble_Game_Mid
{
     class MainScreen : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private List<GameObject> _gameObjects;
        private Vector2 wiggle;

        float tick = 0;
        float _chargeTimer = 0;
        float _comboTimer = 0;
        float _wiggle;
  

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
        Texture2D _cloud1;
        Texture2D _cloud2;
        Texture2D _cloud3;
        Texture2D _cloud4;
        Texture2D _wave;
        Texture2D _headColor;
        Texture2D _body;
        Texture2D _bodyColor;
        Texture2D _tail;
        Texture2D _tailColor;
        Texture2D _overlay;
        Texture2D _overlay_s;
        Texture2D _scroll;
        Texture2D _gauge;
        Texture2D _charge;
        Texture2D _controlBG;
        Texture2D _aboutBG;
        Texture2D _brush;

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

        #region button
        Button btn_play;
        Button btn_exit;
        Button btn_resume;
        Button btn_back;
        Button btn_control;
        Button btn_about;
        #endregion

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
            
            IsMouseVisible = true;
            IsFixedTimeStep = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
        #region Load
            spriteBatch = new SpriteBatch(GraphicsDevice);
            _bubble = Content.Load<Texture2D>("sprite/ball");
            _bg = Content.Load<Texture2D>("image/bg");
            _menuBG = Content.Load<Texture2D>("image/menuBG");
            _controlBG = Content.Load<Texture2D>("image/control");
            _aboutBG = Content.Load<Texture2D>("image/about");
            _overlay = Content.Load<Texture2D>("image/overlay");
            _overlay_s = Content.Load<Texture2D>("image/overlay_s");
            _scroll = Content.Load<Texture2D>("sprite/scroll");
            _border = Content.Load<Texture2D>("sprite/frame");

            _head = Content.Load<Texture2D>("sprite/head");
            _headColor = Content.Load<Texture2D>("sprite/head_color");
            _body = Content.Load<Texture2D>("sprite/body");
            _bodyColor = Content.Load<Texture2D>("sprite/body_color");
            _tail = Content.Load<Texture2D>("sprite/tail");
            _tailColor = Content.Load<Texture2D>("sprite/tail_color");

            _moutain1 = Content.Load<Texture2D>("sprite/mou1");
            _moutain2 = Content.Load<Texture2D>("sprite/mou2");
            _water = Content.Load<Texture2D>("sprite/water");
            _wave = Content.Load<Texture2D>("sprite/wave");
            _cloud1 = Content.Load<Texture2D>("sprite/cloud1");
            _cloud2 =Content.Load<Texture2D>("sprite/cloud2");
            _cloud3 = Content.Load<Texture2D>("sprite/cloud3");
            _cloud4 = Content.Load<Texture2D>("sprite/cloud4");

            _gauge = Content.Load<Texture2D>("sprite/gauge");
            _brush = Content.Load<Texture2D>("sprite/brush");
            _charge = Content.Load<Texture2D>("sprite/charge");
            
            
            _font = Content.Load<SpriteFont>("font/font");

            music = Content.Load<Song>("music/hua sui yue");

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
            MediaPlayer.Play(music);
            MediaPlayer.IsRepeating = true;

           btn_play = new Button(_font, graphics.GraphicsDevice);
           btn_exit = new Button(_font, graphics.GraphicsDevice);
           btn_resume = new Button(_font, graphics.GraphicsDevice);
           btn_back = new Button(_font, graphics.GraphicsDevice);
           btn_control = new Button(_font, graphics.GraphicsDevice);
           btn_about = new Button(_font, graphics.GraphicsDevice);

            _gameObjects = new List<GameObject>()
            {
                new Dragon(_head,_headColor,_shoot,_shoot_S,_skill,_skill_S)
                {
                    Position = new Vector2(Singleton.SCREENWIDTH/2,Singleton.SCREENHEIGHT-100),
                    Bubble = new Bubble(_bubble,_hit)
                }
            };
            for (int i = 0; i < 5; i += 1)
            {
                for (int j = 0; j < 9 - (i % 2); j += 1)
                {

                    Singleton.Instance.GameBoard[i, j] = new Bubble(_bubble,_hit)
                    {

                        Position = new Vector2(600 + 15 + j * (Singleton.BUBBLESIZE + 5) + ((i % 2) == 0 ? 0 : 30), 100 + i * (Singleton.BUBBLESIZE)),
                        active = false,
                        _color = GetRandomColor(),
                        Location = new Vector2(i, j),
                        Isshooting = false
                };
                    _gameObjects.Add(Singleton.Instance.GameBoard[i, j]);
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
                    Singleton.Instance.Score += 100 * 1 + (Singleton.Instance.combo/5);
                    Singleton.Instance.combo += 1;
                    _comboTimer = 0;

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
                    UpdateOverlay(gameTime);
                    break;
                case Singleton.GameState.GameControl:
                case Singleton.GameState.GameAbout:
                    UpdateImage(gameTime);
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
                    DrawImage(_controlBG);
                    break;
                case Singleton.GameState.GameAbout:
                    DrawImage(_aboutBG);
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

            _comboTimer += gameTime.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond;
            if(_comboTimer >= 5)
            {
                
                Singleton.Instance.combo = 0 ;
            }

            _chargeTimer += gameTime.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond;
            if (_chargeTimer >= 3 && Singleton.Instance.Charge < 12)
            {
                _recharge.Play();
                Singleton.Instance.Charge += 1;
                _chargeTimer = 0;
            }

          if( Singleton.Instance.Charge < 12)
            {
                _chargeColor = Color.Yellow;
            }

            else if(Singleton.Instance.Charge == 12)
            {
                _chargeColor = GetRandomColor();
            }


            _wiggle += gameTime.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond;

            if (_wiggle > 0.3)
            {
                wiggle.X += rnd.Next(0, 2);
                wiggle.Y += rnd.Next(0, 2);
                if (wiggle.X >= 2) wiggle.X -= rnd.Next(0, 2);
                if (wiggle.Y >= 1) wiggle.Y -= rnd.Next(0, 2);
                _wiggle = 0;
            }



            tick += gameTime.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond;
            if (tick >= 20)
            {
                Singleton.ScreenDown += 60;

                for (int i = 0; i < 18; i += 1)
                {
                    for (int j = 0; j < 9 - (i % 2); j += 1)
                    {
                        if (Singleton.Instance.GameBoard[i, j] != null)
                        {
                            Singleton.Instance.GameBoard[i, j].Position.Y += 60;
                        }
                    }
                }
                tick = 0;
            }

            foreach (var gameobject in _gameObjects.ToArray())
            {
                gameobject.Update(gameTime, _gameObjects);
            }

        }

        private void DrawPlaying()
        {
            spriteBatch.Draw(_bg, destinationRectangle: new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT));


            spriteBatch.Draw(_cloud1, destinationRectangle: new Rectangle(150, 50, 150, 80));
            spriteBatch.Draw(_cloud2, destinationRectangle: new Rectangle(450, 20, 150, 50));
            spriteBatch.Draw(_cloud3, destinationRectangle: new Rectangle(1500, 10, 200, 80));
            spriteBatch.Draw(_cloud4, destinationRectangle: new Rectangle(1200, 80, 150, 80));

            spriteBatch.Draw(_water, destinationRectangle: new Rectangle(0 + (int)wiggle.Y / 2, Singleton.SCREENHEIGHT - 130 + (int)wiggle.Y/3, Singleton.SCREENWIDTH, 50));
            foreach (var gameobject in _gameObjects)
            {
                gameobject.Draw(spriteBatch);
            }
            spriteBatch.Draw(_moutain1, destinationRectangle: new Rectangle(0, 50, 500, 900));
            spriteBatch.Draw(_moutain2, destinationRectangle: new Rectangle(1250, 50, 500, 860));
            spriteBatch.Draw(_border, destinationRectangle: new Rectangle(385, Singleton.ScreenDown - 120, 980, Singleton.BoardHeight + 300));

            spriteBatch.Draw(_body, new Vector2(1000, 800), null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.Draw(_bodyColor, new Vector2(1000, 800), null, Singleton.CurrentColor, 0f, Vector2.Zero, 1, SpriteEffects.None, 0);

            spriteBatch.Draw(_water, destinationRectangle: new Rectangle(0 , Singleton.SCREENHEIGHT - 90, Singleton.SCREENWIDTH, 50));
            spriteBatch.Draw(_water, destinationRectangle: new Rectangle(0 - (int)wiggle.X, Singleton.SCREENHEIGHT - 100 - (int)wiggle.Y / 3, Singleton.SCREENWIDTH, 50));
            spriteBatch.Draw(_wave, destinationRectangle: new Rectangle(500 - (int)wiggle.X,800 - (int)wiggle.Y / 3, 100, 100));


            spriteBatch.Draw(_tail, new Vector2 (1300,670 ), Color.White);
            spriteBatch.Draw(_tailColor, new Vector2(1300, 670), Singleton.CurrentColor);
           
            spriteBatch.Draw(_water, destinationRectangle: new Rectangle(0 + (int)wiggle.X , Singleton.SCREENHEIGHT - 70 + (int)wiggle.Y /3 , Singleton.SCREENWIDTH, 50));

            spriteBatch.Draw(_body, new Vector2(300, 850),null, Color.White, 0f, Vector2.Zero, 1.25f, SpriteEffects.None, 0);
            spriteBatch.Draw(_bodyColor, new Vector2(300, 850),null, Singleton.CurrentColor, 0f, Vector2.Zero, 1.25f, SpriteEffects.None, 0);

            spriteBatch.Draw(_wave, destinationRectangle: new Rectangle(1250 - (int)wiggle.X, 800 - (int)wiggle.Y / 3, 100, 100),effects: SpriteEffects.FlipHorizontally);
            spriteBatch.Draw(_water, destinationRectangle: new Rectangle(0 + (int)wiggle.X /2, Singleton.SCREENHEIGHT - 40 - (int)wiggle.Y/3, Singleton.SCREENWIDTH, 50));

            spriteBatch.Draw(_body, new Vector2(800, 850), null, Color.White, 0.3f, Vector2.Zero, 1.5f, SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(_bodyColor, new Vector2(800, 850), null, Singleton.CurrentColor, 0.3f, Vector2.Zero, 1.5f, SpriteEffects.FlipHorizontally, 0);


            spriteBatch.Draw(_brush, destinationRectangle: new Rectangle(100, 670, 250, 80));
            spriteBatch.DrawString(_font, " " + Singleton.Instance.Score, new Vector2(200, 680), Color.White, 0, Vector2.Zero, 1.2f, SpriteEffects.None, 0);
            spriteBatch.DrawString(_font, "X " +  (1 + Singleton.Instance.combo / 5) , new Vector2(230, 650) + _font.MeasureString("" + Singleton.Instance.Score), Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);

            if (Singleton.Instance.combo > 0)
            {
                spriteBatch.DrawString(_font, "Combo " + Singleton.Instance.combo, new Vector2(200, 640), Color.Red, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0);
                spriteBatch.DrawString(_font, "Combo " + Singleton.Instance.combo, new Vector2(200-3, 640-3), Color.White, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 0);
            }
            spriteBatch.Draw(_gauge, destinationRectangle: new Rectangle(20,750 , 350, 80));
            for (int i = 0; i < Singleton.Instance.Charge; i++)
            {
                spriteBatch.Draw(_charge, destinationRectangle: new Rectangle(160 + i * 15, 772, 10, 35),color: _chargeColor);
            }

            if (Singleton.Instance.ult)
            {
                spriteBatch.Draw(_overlay_s, destinationRectangle: new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT),color: GetRandomColor());
            }

        }

        private void UpdateMenu(GameTime gameTime)

           
        {
            if (_currentkey.IsKeyDown(Keys.Enter))
            {
                Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
            }

            if (btn_play.isClick == true)
            {
                Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                _hit.Play();
            }
                btn_play.update(_currentmouse, _previousmouse);

            if (btn_control.isClick == true)
            {
                _hit.Play();
                Singleton.Instance.CurrentGameState = Singleton.GameState.GameControl;
                btn_control.isClick = false;
            }
            btn_control.update(_currentmouse, _previousmouse);

            if (btn_about.isClick == true)
            {
                _hit.Play();

                Singleton.Instance.CurrentGameState = Singleton.GameState.GameAbout;
                btn_about.isClick = false;
            }
            btn_about.update(_currentmouse, _previousmouse);

            if (btn_exit.isClick == true) Exit();
            btn_exit.update(_currentmouse, _previousmouse);

        }

        void DrawMenu()
        {
            spriteBatch.Draw(_menuBG, destinationRectangle: new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT));
            btn_play.Draw(spriteBatch);
            btn_play.set(new Vector2(400,450),"play");
            btn_control.Draw(spriteBatch);
            btn_control.set(new Vector2(400, 550), "control");
            btn_about.Draw(spriteBatch);
            btn_about.set(new Vector2(400, 650), "about");
            btn_exit.Draw(spriteBatch);
            btn_exit.set(new Vector2(400, 750), "exit");
        }

        private void UpdateOverlay(GameTime gameTime)
        {

            if (_currentkey.IsKeyDown(Keys.Escape) && _previouskey.IsKeyUp(Keys.Escape))
            {
                Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
                
            }
            if (btn_exit.isClick == true) Exit();
            btn_exit.update(_currentmouse, _previousmouse);

            if (btn_back.isClick == true)
            {
                _hit.Play();

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
            spriteBatch.DrawString(_font, "Score: " + Singleton.Instance.Score, new Vector2(Singleton.SCREENWIDTH / 2, Singleton.SCREENHEIGHT/2) - _font.MeasureString("Score: " + Singleton.Instance.Score) / 2, Color.DarkRed);
            btn_exit.Draw(spriteBatch);
            btn_exit.set(new Vector2(Singleton.SCREENWIDTH / 2 - 60, 600), "exit");

            if (dia == "Game paused")
            {
                btn_back.Draw(spriteBatch);
                btn_back.set(new Vector2(Singleton.SCREENWIDTH / 2 - 90, 530), "resume");
            }
        }

        private void UpdateImage(GameTime gameTime)
        {
            if (btn_back.isClick == true)
            {
                 _hit.Play();
                Singleton.Instance.CurrentGameState = Singleton.GameState.GameMenu;
                btn_back.isClick = false;
            }
            btn_back.update(_currentmouse, _previousmouse);
        }

        private void DrawImage(Texture2D image)
        {
            spriteBatch.Draw(image, destinationRectangle: new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT));
            btn_back.Draw(spriteBatch);
            btn_back.set(new Vector2(Singleton.SCREENWIDTH/2 - 75, 750) ,"back");

        }
    }
}
