using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bubble_Game_Mid;
using Microsoft.Xna.Framework.Audio;

namespace Bubble_Game_Mid.gameObject
{
    public class Bubble : GameObject
    {


        public bool active = true;
        public bool Isshooting;
        public bool special = false;
        SoundEffect _hit;

        private bool IsCheck = false;
        public Vector2 Location;
        int _yeet;

        public Bubble(Texture2D texture, SoundEffect _hit) : base(texture)
        {
        
            Scale = new Vector2(Singleton.BUBBLESIZE / texture.Width, Singleton.BUBBLESIZE / texture.Width);
            RotationVelocity = 0.1f;
            radius = (texture.Width / 2) + 5;
            _ObjType = ObjType.bubble;
            this._hit = _hit;
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {

            if (_color.A < 255)
                _color.A += 1;

            if (!active)
            {
                LinearVelocity = 0;
                RotationVelocity = 0;
               
            }



            if (!active && Position.Y > Singleton.BoardHeight)
                Singleton.Instance.CurrentGameState = Singleton.GameState.GameLose;

            Position += Direction * LinearVelocity;
            Rotation += RotationVelocity;
            CheckColision(gameObjects);
            Checkneighbor();



            for (int i = 0; i < 18; i += 1)
            {
                for (int j = 0; j < 9 - (i % 2); j += 1)
                {
                    if (Singleton.Instance.GameBoard[i, j] == null)
                        continue;

                    if (Singleton.Instance.GameBoard[i, j].count >= 4)
                    {
                        Singleton.Instance.GameBoard[i, j].IsRemove = true;
                        Singleton.Instance.GameBoard[i, j] = null;
                    }
                }
            }

            base.Update(gameTime,gameObjects);
        }



        public void CheckColision(List<GameObject> gameObjects)
        {

            foreach (var sprite in gameObjects)
            {
                if (sprite == this)
                    continue;

                Vector2 distance = this.Position - sprite.Position;
                if (distance.Length() < this.radius + sprite.radius && this.active && sprite._ObjType == ObjType.bubble)
                {
                    if (this.special)
                    {
                        sprite.count += 100;
                        _hit.Play();
                    }
                    else
                    {
                        Console.WriteLine("I'm " + this._color + " I'm hitting " + sprite._color + " And i'm at + " + this.Position + " " + this.Location);
                        active = false;
                        _hit.Play();
                        CheckLocation();
                    }
                }
                
            }

            #region border colision

            if (Position.X - Origin.X <= 600 && Direction.X / LinearVelocity < 600)
            {
                Direction.X *= -1;
                _hit.Play();
            }

            else if (active && Position.Y - Origin.Y <= Singleton.ScreenDown + 100 && Direction.Y / LinearVelocity < Singleton.ScreenDown + 100)
            {
                _hit.Play();
                if (special)
                {
                    Singleton.Instance.ult = false;
                    IsRemove = true;
                }
                else
                {
                    active = false;
                    CheckLocation();
                }
            }

            else if (Position.X + Origin.X >= Singleton.BoardWidth + 600 && Direction.X / LinearVelocity < Singleton.BoardWidth + 600)
            {
                Direction.X *= -1;
                _hit.Play();
            }
            #endregion

        }

        public void CheckLocation()
        {
            int i = (int)(this.Position.Y - 100 - Singleton.ScreenDown + radius) / Singleton.BUBBLESIZE;
            int j = (int)(this.Position.X - 600 - 15 + radius - ((i % 2) == 0 ? 0 : 30)) / (Singleton.BUBBLESIZE + 5);

            this.Position = new Vector2(600 + 15 + j * (Singleton.BUBBLESIZE + 5) + ((i % 2) == 0 ? 0 : 30), Singleton.ScreenDown + 100 + i * (Singleton.BUBBLESIZE));
            Singleton.Instance.GameBoard[i, j] = this;
            Location = new Vector2(i, j);
            //Console.WriteLine(i + "  |  " + j);
            CheckColor();
            IsCheck = false;

        }

        private void CheckColor()
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
                    if (Singleton.Instance.GameBoard[i, j] == null || Singleton.Instance.GameBoard[i, j] == Singleton.Instance.GameBoard[(int)Location.X, (int)Location.Y])
                        continue;

                    //even row
                    if (Location.X % 2 == 0 && ((i == Location.X - 1 && j == Location.Y + 1) || (i == Location.X + 1 && j == Location.Y + 1)))
                        continue;

                    //odd row
                    else if (Location.X % 2 != 0 && ((i == Location.X - 1 && j == Location.Y - 1) || (i == Location.X + 1 && j == Location.Y - 1)))
                        continue;

                    if (_color == Singleton.Instance.GameBoard[i, j]._color)
                    {

                        Singleton.Instance.GameBoard[i, j].count += 1;
                        Singleton.Instance.GameBoard[(int)Location.X, (int)Location.Y].count += 1;

                        if (Singleton.Instance.GameBoard[i, j].count >= Singleton.Instance.GameBoard[(int)Location.X, (int)Location.Y].count)
                            Singleton.Instance.GameBoard[(int)Location.X, (int)Location.Y].count = Singleton.Instance.GameBoard[i, j].count;

                        else Singleton.Instance.GameBoard[i, j].count = ++Singleton.Instance.GameBoard[(int)Location.X, (int)Location.Y].count;

                        IsCheck = true;
                        Singleton.Instance.GameBoard[i, j].CheckColor();

                    }
                    //Console.Write("color at " + i + " " + j + " is:" + GameBoard[i, j]._color);
                }
                //Console.WriteLine("");
            }
            IsCheck = false;
        }

        private void Checkneighbor()
        {
            _yeet = 0;
            for (int i = (int)Location.X - 1; i <= Location.X; i += 1)
            {
                for (int j = (int)Location.Y - 1; j <= Location.Y + 1; j += 1)
                {
                    if (i < 0 || j < 0 || i > 17 || j > 8)
                        continue;

                    //evenrow
                    if (Location.X % 2 == 0 && ((i == Location.X - 1 && j == Location.Y + 1) || (i == Location.X + 1 && j == Location.Y + 1)))
                        continue;

                    //odd row
                    else if (Location.X % 2 != 0 && ((i == Location.X - 1 && j == Location.Y - 1) || (i == Location.X + 1 && j == Location.Y - 1)))
                        continue;

                    if (Singleton.Instance.GameBoard[i, j] != null)
                        _yeet += 1;
                }
            }
            if (_yeet <=1 && (int)Location.X != 0 && !active)
            {
                Singleton.Instance.GameBoard[(int)Location.X, (int)Location.Y] = null;
                IsRemove = true;
                _yeet = 0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (special)
                _color = Singleton.CurrentColor;
            spriteBatch.Draw(_texture, Position, null, _color, Rotation, Origin, 1f, SpriteEffects.None, 0);
            //spriteBatch.DrawString(_font, " " + count, Position - new Vector2(15,15), Color.Black);
            base.Draw(spriteBatch);
        }


    }
}
