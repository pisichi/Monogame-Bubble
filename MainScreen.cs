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
        private Bubble[,] bubble = new Bubble[18, 9];

        float tick = 10f;

        Random rnd = new Random();

        Texture2D _bubble;
        Texture2D _bg;
        Texture2D _border;
        Texture2D _gun;
        Texture2D _bubble2;
        Texture2D _frame;
        Rectangle _borderRect;




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


       
            base.Initialize();

        }


        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
            _bubble = this.Content.Load<Texture2D>("ball");
            _bg = this.Content.Load<Texture2D>("bg");
            _border = this.Content.Load<Texture2D>("border");
            _gun = this.Content.Load<Texture2D>("head3");
            _bubble2 = this.Content.Load<Texture2D>("ball2");
            _frame = this.Content.Load<Texture2D>("frame");


            _gameObjects = new List<GameObject>()
            {
                new Gun(_gun)
                {
                    Position = new Vector2(Singleton.SCREENWIDTH/2,Singleton.SCREENHEIGHT-100),
                    Bubble = new Bubble(_bubble)
                }
            };


            for (int i = 0; i < 5; i += 1)
            {
                for (int j = 0; j < 9 - (i % 2); j += 1)
                {

                    bubble[i, j] = new Bubble(_bubble)
                    {
                        Position = new Vector2(400 + 15 + j * (Singleton.BOBBLESIZE + 5) + ((i % 2) == 0 ? 0 : 30), 100 + i * (Singleton.BOBBLESIZE)),
                        IsActive = false,
                        _color = GetRandomColor(),
                        Location = new Vector2(i, j),
                        _bubbleState = Bubble.BubbleState.inplace,
                        Isshooting = false
                };
                    _gameObjects.Add(bubble[i, j]);
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
            if (tick >= 30)
            {
                Singleton._down += 60;

                for (int i = 0; i < 18; i += 1)
                {
                    for (int j = 0; j < 9 - (i % 2); j += 1)
                    {
                        if (bubble[i, j] != null)
                        {
                            bubble[i, j].Position.Y += 60;
                        }
                    }

                }
                tick = 0;
            }

            foreach (var gameobject in _gameObjects.ToArray())
            {
                gameobject.Update(gameTime, _gameObjects,bubble);
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
            spriteBatch.Draw(_border, destinationRectangle: new Rectangle(370, Singleton.BOBBLESIZE + Singleton._down, Singleton.BoardWidth + 70, Singleton.BoardHeight + 100 + Singleton._down));


            foreach (var gameobject in _gameObjects)
            {
                gameobject.Draw(spriteBatch);
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
            _color.A = 50;
            return _color;
        }
    }
}
