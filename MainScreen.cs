using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Bobble_Game_Mid.gameObject;
using System.Threading;

namespace Bobble_Game_Mid
{

     class MainScreen : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private List<GameObject> _gameObjects;
        private Bubble[,] GameBoard = new Bubble[18, 9];

        float tick = 10f;

        Random rnd = new Random();

        Texture2D _bubble;
        Texture2D _bg;
        Texture2D _border;
        Texture2D _head;
        Texture2D _bubble2;
        Texture2D _moutain1;
        Texture2D _moutain2;
        Texture2D _water;
        Texture2D _headColor;
        Texture2D _body;
        Texture2D _bodyColor;
        Texture2D _tail;
        Texture2D _tailColor;
        SpriteFont _font;




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
            Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;
            this.IsMouseVisible = true;



            base.Initialize();

        }


        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
            _bubble = this.Content.Load<Texture2D>("sprite/ball");
            _bg = this.Content.Load<Texture2D>("sprite/bg");
            _border = this.Content.Load<Texture2D>("sprite/frame");
            _head = this.Content.Load<Texture2D>("sprite/head");
            _headColor = this.Content.Load<Texture2D>("sprite/head_color");
            _moutain1 = this.Content.Load<Texture2D>("sprite/mou1");
            _moutain2 = this.Content.Load<Texture2D>("sprite/mou2");
            _water = this.Content.Load<Texture2D>("sprite/water");

            _font = Content.Load<SpriteFont>("font/font");


            _gameObjects = new List<GameObject>()
            {
                new Dragon(_head,_headColor)
                {
                    Position = new Vector2(Singleton.SCREENWIDTH/2,Singleton.SCREENHEIGHT-100),
                    Bubble = new Bubble(_bubble,_font)
                }
            };


            for (int i = 0; i < 5; i += 1)
            {
                for (int j = 0; j < 9 - (i % 2); j += 1)
                {

                    GameBoard[i, j] = new Bubble(_bubble, _font)
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

        }


        protected override void Update(GameTime gameTime)
        {

           
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;





            tick += gameTime.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond;
            if (tick >= 20)
            {
                Singleton._down += 60;

                for (int i = 0; i < 18; i += 1)
                {
                    for (int j = 0; j < 9 - (i % 2); j += 1)
                    {
                        if (GameBoard[i, j] != null)
                        {
                            GameBoard[i, j].Position.Y += 60;
                            if (GameBoard[i, j].Position.Y > Singleton.BoardHeight)
                                Singleton.Instance.CurrentGameState = Singleton.GameState.GameEnded;

                        }
                    }

                }
                tick = 0;
            }

            foreach (var gameobject in _gameObjects.ToArray())
            {
                gameobject.Update(gameTime, _gameObjects,GameBoard);
            }

            PostUpdate();
            base.Update(gameTime);

        }

        private void PostUpdate()
        {
            for (int i = 0; i < _gameObjects.Count; i++)
            {
                if (_gameObjects[i].IsRemove)
                {
                    _gameObjects.RemoveAt(i);
                    i--;
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);



            spriteBatch.Begin(); 

            spriteBatch.Draw(_bg, destinationRectangle: new Rectangle(0, 0, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT));


            spriteBatch.Draw(_water, destinationRectangle: new Rectangle(0, Singleton.SCREENHEIGHT - 130, Singleton.SCREENWIDTH, 50));
            foreach (var gameobject in _gameObjects)
            {
                gameobject.Draw(spriteBatch);
            }


            spriteBatch.Draw(_moutain1, destinationRectangle: new Rectangle(0, 50, 565, 900));
            spriteBatch.Draw(_moutain2, destinationRectangle: new Rectangle(1250, 50, 500, 860));

            spriteBatch.Draw(_border, destinationRectangle: new Rectangle(350,0 + Singleton._down - 150, Singleton.SCREENWIDTH -700 , Singleton.BoardHeight + 300 ));



            for (int i = 5; i >= 0; i--)
                spriteBatch.Draw(_water, destinationRectangle: new Rectangle(0, Singleton.SCREENHEIGHT + 50 - i * 32, Singleton.SCREENWIDTH, 50));

            if (Singleton.Instance.CurrentGameState == Singleton.GameState.GameEnded)
                spriteBatch.Draw(_bg, destinationRectangle: new Rectangle(200, 200, Singleton.SCREENWIDTH, Singleton.SCREENHEIGHT));


            spriteBatch.DrawString(_font, " " + Singleton.Score, new Vector2(0,200), Color.Black);

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
            _color.A = 50;
            return _color;
        }
    }
}
