﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Bobble_Game_Mid.gameObject;


namespace Bobble_Game_Mid
{

    public class MainScreen : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private List<GameObject> _gameObjects;
        Bubble [,] bubble = new Bubble[6,9];

        Random rnd = new Random();

        Texture2D _bubble;
        Texture2D _bg;
        Texture2D _border;
        Texture2D _gun;

        int draw;

        int currentPosX = Singleton.BOBBLESIZE * 2;
        int currentPosY = Singleton.BOBBLESIZE;
        int border = Singleton.BOBBLESIZE * 2;


        float tick, moveSpeed = 10f;

        float decendSpeed;


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

            decendSpeed = 150;

            base.Initialize();

        }


        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
            _bubble = this.Content.Load<Texture2D>("ball");
            _bg = this.Content.Load<Texture2D>("bg");
            _border = this.Content.Load<Texture2D>("border");
            _gun = this.Content.Load<Texture2D>("gun");
         


            _gameObjects = new List<GameObject>()
            {
                new Gun(_gun)
                {
                    Position = new Vector2(Singleton.SCREENWIDTH/2,Singleton.SCREENHEIGHT-100),
                    Bubble = new Bubble(_bubble)
                }
            };


        }

        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Singleton.Instance.CurrentGameState = Singleton.GameState.GamePlaying;


            //for (int i = 0; i < Singleton.BoardHeight / 3; i += Singleton.BOBBLESIZE)
            //{
            //    for (int j = 0; j < Singleton.BoardHeight - 200; j += Singleton.BOBBLESIZE)
            //    {

            //        bubble[i, j] = new Bubble(_bubble)
            //        {
            //        bubble[i, j].direction = this.direction;
            //        bubble[i, j].position = this.position;
            //        bubble[i, j].linearvelocity = 0;
            //        bubble[i, j]._color = this.getrandomcolor();
            //    };
            //    _gameobjects.add(bubble[i, j]);
            //}
        


            foreach (var gameobject in _gameObjects.ToArray())
            {
                gameobject.Update(gameTime, _gameObjects);
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


            spriteBatch.Draw(_border, destinationRectangle: new Rectangle(200, Singleton.BOBBLESIZE, Singleton.BoardWidth , Singleton.BoardHeight+100));


            foreach (var gameobject in _gameObjects)
            {
                gameobject.Draw(spriteBatch);
            }

          


            spriteBatch.End();

            graphics.BeginDraw();



            base.Draw(gameTime);
        }


        //private void AddBubble(List<GameObject> gameObjects)
        //{
        //    var bubble = Bubble.Clone() as Bubble;
        //    bubble.Direction = this.Direction;
        //    bubble.Position = this.Position;
        //    bubble.LinearVelocity = 0;
        //    bubble._color = this.GetRandomColor();
        //    gameObjects.Add(bubble);



        //    if (_currentkey.IsKeyDown(Keys.Space) && _previouskey.IsKeyUp(Keys.Space))
        //    {

        //        bubble.LinearVelocity = this.LinearVelocity * 5;
        //        shooting = true;
        //    }
        //}


        public Color GetRandomColor()
        {
            Color _color = Color.Black;
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
    }
}
