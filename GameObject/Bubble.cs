using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bobble_Game_Mid;

namespace Bobble_Game_Mid.gameObject
{
    class Bubble : GameObject
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public bool IsActive = true;
        public bool Isshooting;
        private bool checkhit = false;

        private bool IsCheck = false;


        public Vector2 Location;

        bool Ishitting = false;
        bool Inplace = false;
        bool TouchTop;

        int Pcount = 0;

        Texture2D _texture2;

        public enum BubbleState
        {
            shooting,
            hitting,
            inplace
        }

        public BubbleState _bubbleState;


        public Bubble(Texture2D texture) : base(texture)
        {
            Scale = new Vector2(Singleton.BOBBLESIZE / texture.Width, Singleton.BOBBLESIZE / texture.Width);
            RotationVelocity = 0.1f;
            radius = (texture.Width + 5) / 2;
            _ObjType = ObjType.bubble;

        }


        public override void Update(GameTime gameTime, List<GameObject> gameObjects, Bubble[,] bubble)
        {

            if (_color.A < 255)
                _color.A += 1;

            if (!IsActive)
            {
                LinearVelocity = 0;
                RotationVelocity = 0;
            }
            if (count >= 3)
            {
                IsRemove = true;
                bubble[(int)Location.X, (int)Location.Y] = null;
  
            }
                

            Position += Direction * LinearVelocity;
            Rotation += RotationVelocity;
            CheckColision(gameObjects, bubble);

            base.Update(gameTime, gameObjects, bubble);
        }



        private void CheckColision(List<GameObject> gameObjects, Bubble[,] bubble)
        {
            foreach (var sprite in gameObjects)
            {
                if (sprite == this)
                    continue;

                Vector2 distance = this.Position - sprite.Position;
                if (distance.Length() < this.radius + sprite.radius && this.IsActive && sprite._ObjType == ObjType.bubble)
                {
                    Console.WriteLine("I'm " + this._color + " I'm hitting " + sprite._color + " And i'm at + " + this.Position + " " + this.Location);
                    IsActive = false;
                    checkhit = true;


                    CheckLocation(bubble);
                    CheckColor(sprite, bubble);
                    IsCheck = false;
                }
            }

            #region border colision

            if (Position.X - Origin.X <= 400 && Direction.X / LinearVelocity < 400)
            {
                Direction.X *= -1;
            }

            else if (Position.Y - Origin.Y <= 60 && Direction.Y / LinearVelocity < 60)
            {
                IsActive = false;
            }

            else if (Position.X + Origin.X >= Singleton.BoardWidth + 400 && Direction.X / LinearVelocity < Singleton.BoardWidth + 400)
            {
                Direction.X *= -1;
            }
            #endregion

        }

        private void CheckLocation(Bubble[,] bubble)
        {
            int i = (int)(this.Position.Y - 100 - Singleton._down + radius) / Singleton.BOBBLESIZE;
            int j = (int)(this.Position.X - 400 - 15 + radius - ((i % 2) == 0 ? 0 : 30)) / (Singleton.BOBBLESIZE + 5);

            this.Position = new Vector2(400 + 15 + j * (Singleton.BOBBLESIZE + 5) + ((i % 2) == 0 ? 0 : 30), Singleton._down + 100 + i * (Singleton.BOBBLESIZE));
            bubble[i, j] = this;
            Location = new Vector2(i, j);

            Console.WriteLine(i + " " + j);
        }




        private void CheckColor(GameObject sprite, Bubble[,] bubble)
        {
            if (IsCheck)
                return;

            for (int i = (int)Location.X - 1; i <= Location.X + 1; i += 1)
            {

                for (int j = (int)Location.Y - 1; j <= Location.Y + 1 ; j += 1)
                {
                    //null handeler
                    if (i < 0 || j < 0 || bubble[i, j] == null || bubble[i, j] == this)
                        continue;

                    //even row
                    if (Location.X % 2 == 0 &&( (i == Location.X - 1 && j == Location.Y + 1) || (i == Location.X + 1 && j == Location.Y + 1)) )
                        continue;

                    //odd row
                    if (Location.X % 2 != 0 && ((i == Location.X - 1 && j == Location.Y - 1) || (i == Location.X + 1 && j == Location.Y - 1)))
                        continue;


                    Console.Write("color at " + i + " " + j + " is:" + bubble[i, j]._color);

                    if (bubble[i, j]._color == bubble[(int)Location.X, (int)Location.Y]._color)
                    {
                        Console.WriteLine("yay we are the same color at " + i + " " + j);
                        bubble[(int)Location.X, (int)Location.Y].count+=1;
                        //bubble[i,j].count += 1;
                        IsCheck = true;
                        bubble[i,j].CheckColor(sprite, bubble);
                    }

                }
                Console.WriteLine("");
            }



            for (int i = 0; i < 12; i += 1)
            {
                for (int j = 0; j < 9 - (i % 2); j += 1)
                {
                    if (bubble[i, j] == null)
                        continue;

                    Console.Write("color at " + i + " " + j + " is:" + bubble[i, j]._color);
                }
                Console.WriteLine("");
            }


        }



        public override void Draw(SpriteBatch spriteBatch)
        {

            if (Isshooting)
            {
                
            }

            spriteBatch.Draw(_texture, Position, null, _color, Rotation, Origin, 1f, SpriteEffects.None, 0);

            base.Draw(spriteBatch);
        }








    }
}
