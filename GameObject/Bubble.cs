using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bobble_Game_Mid;
using Microsoft.Xna.Framework.Audio;

namespace Bobble_Game_Mid.gameObject
{
    class Bubble : GameObject
    {
        SpriteFont _font;

        public bool IsActive = true;
        public bool Isshooting;
        public bool special = false;
        SoundEffect _hit;

        private bool IsCheck = false;
        public Vector2 Location;
        int _yeet;
        public Bubble(Texture2D texture,SpriteFont font, SoundEffect _hit) : base(texture)
        {
        
            Scale = new Vector2(Singleton.BUBBLESIZE / texture.Width, Singleton.BUBBLESIZE / texture.Width);
            RotationVelocity = 0.1f;
            radius = (texture.Width / 2 );
            _ObjType = ObjType.bubble;
            this._font = font;
            this._hit = _hit;
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, Bubble[,] GameBoard)
        {

            if (_color.A < 255)
                _color.A += 1;

            if (!IsActive)
            {
                LinearVelocity = 0;
                RotationVelocity = 0;
                if(Position.Y > Singleton.BoardHeight)
                     Singleton.Instance.CurrentGameState = Singleton.GameState.GameLose;
            }

           if (count >= 4)
            {
                GameBoard[(int)Location.X, (int)Location.Y] = null;
                IsRemove = true;
            }


            Position += Direction * LinearVelocity;
            Rotation += RotationVelocity;
            CheckColision(gameObjects, GameBoard);
            Checkneighbor(GameBoard);
            base.Update(gameTime,gameObjects, GameBoard);
        }



        public void CheckColision(List<GameObject> gameObjects, Bubble[,] GameBoard)
        {

            foreach (var sprite in gameObjects)
            {
                if (sprite == this)
                    continue;

                Vector2 distance = this.Position - sprite.Position;
                if (distance.Length() < this.radius + sprite.radius && this.IsActive && sprite._ObjType == ObjType.bubble)
                {
                    if (this.special)
                    {
                        sprite.count += 100;
                        _hit.Play();
                    }
                    else
                    {
                        Console.WriteLine("I'm " + this._color + " I'm hitting " + sprite._color + " And i'm at + " + this.Position + " " + this.Location);
                        IsActive = false;
                        _hit.Play();
                        CheckLocation(GameBoard);
                    }
                }
                
            }

            #region border colision

            if (Position.X - Origin.X <= 600 && Direction.X / LinearVelocity < 600)
            {
                Direction.X *= -1;
                _hit.Play();
            }

            else if (IsActive && Position.Y - Origin.Y <= Singleton.ScreenDown + 100 && Direction.Y / LinearVelocity < Singleton.ScreenDown + 100)
            {
                _hit.Play();
                if (special)
                {
                    Singleton.Instance.ult = false;
                    IsRemove = true;
                }
                else
                {
                    IsActive = false;
                    CheckLocation(GameBoard);
                }
            }

            else if (Position.X + Origin.X >= Singleton.BoardWidth + 600 && Direction.X / LinearVelocity < Singleton.BoardWidth + 600)
            {
                Direction.X *= -1;
                _hit.Play();
            }
            #endregion

        }

        public void CheckLocation(Bubble[,] GameBoard)
        {
            int i = (int)(this.Position.Y - 100 - Singleton.ScreenDown + radius) / Singleton.BUBBLESIZE;
            int j = (int)(this.Position.X - 600 - 15 + radius - ((i % 2) == 0 ? 0 : 30)) / (Singleton.BUBBLESIZE + 5);

            this.Position = new Vector2(600 + 15 + j * (Singleton.BUBBLESIZE + 5) + ((i % 2) == 0 ? 0 : 30), Singleton.ScreenDown + 100 + i * (Singleton.BUBBLESIZE));
            GameBoard[i, j] = this;
            Location = new Vector2(i, j);
            //Console.WriteLine(i + "  |  " + j);
            CheckColor(GameBoard);
            IsCheck = false;

        }

        private void CheckColor( Bubble[,] GameBoard)
        {

            if (IsCheck)
                return;

            for (int i = (int)Location.X - 1; i <= Location.X + 1; i += 1)
            {
                for (int j = (int)Location.Y - 1; j <= Location.Y + 1; j += 1)
                {

                    if (i < 0 || j < 0 || i > 17 || j > 8)
                        continue;

                    //null handeler
                    if (GameBoard[i, j] == null || GameBoard[i, j] == GameBoard[(int)Location.X, (int)Location.Y])
                        continue;

                    //even row
                    if (Location.X % 2 == 0 && ((i == Location.X - 1 && j == Location.Y + 1) || (i == Location.X + 1 && j == Location.Y + 1)))
                        continue;

                    //odd row
                    else if (Location.X % 2 != 0 && ((i == Location.X - 1 && j == Location.Y - 1) || (i == Location.X + 1 && j == Location.Y - 1)))
                        continue;

                    if (_color == GameBoard[i, j]._color)
                    {

                        GameBoard[i, j].count += 1;
                        GameBoard[(int)Location.X, (int)Location.Y].count += 1;

                        if (GameBoard[i, j].count >= GameBoard[(int)Location.X, (int)Location.Y].count)
                            GameBoard[(int)Location.X, (int)Location.Y].count = GameBoard[i, j].count;

                        else GameBoard[i, j].count = GameBoard[(int)Location.X, (int)Location.Y].count;
                        IsCheck = true;
                        GameBoard[i, j].CheckColor(GameBoard);

                    }
                    //Console.Write("color at " + i + " " + j + " is:" + GameBoard[i, j]._color);
                }
                //Console.WriteLine("");
            }
        }

        private void Checkneighbor(Bubble[,] GameBoard)
        {
            _yeet = 0;
            for (int i = (int)Location.X - 1; i <= Location.X; i += 1)
            {
                for (int j = (int)Location.Y - 1; j <= Location.Y + 1; j += 1)
                {
                    if (i < 0 || j < 0 || i > 17 || j > 8)
                        continue;

                    if (Location.X % 2 == 0 && ((i == Location.X - 1 && j == Location.Y + 1) || (i == Location.X + 1 && j == Location.Y + 1)))
                        continue;

                    //odd row
                    else if (Location.X % 2 != 0 && ((i == Location.X - 1 && j == Location.Y - 1) || (i == Location.X + 1 && j == Location.Y - 1)))
                        continue;
                    if (GameBoard[i, j] != null)
                        _yeet += 1;
                }
            }
            if (_yeet <=1 && (int)Location.X != 0 && !IsActive)
            {
                GameBoard[(int)Location.X, (int)Location.Y] = null;
                IsRemove = true;
                _yeet = 0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (special)
                _color = Singleton.CurrentColor;
            spriteBatch.Draw(_texture, Position, null, _color, Rotation, Origin, 1f, SpriteEffects.None, 0);
           //spriteBatch.DrawString(_font, " " + count, Position, Color.Black);
            base.Draw(spriteBatch);
        }


    }
}
