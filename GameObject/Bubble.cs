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
            radius = (texture.Width + 7) / 2;
            _ObjType = ObjType.bubble;

        }


        public override void Update(GameTime gameTime, List<GameObject> gameObjects, Bubble[,] bubble)
        {

            if (_color.A < 254)
                _color.A += 2;

            if (!IsActive)
            {
                LinearVelocity = 0;
                RotationVelocity = 0;
            }
            if (count >= 3)
                IsRemove = true;


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
            bubble[8, 8] = this;
        }

        private void CheckColor(GameObject sprite, Bubble[,] bubble)
        {

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

            //if (this._color == sprite._color && checkhit)
            //{
            //    sprite.count++;
            //    this.count = sprite.count;

            //    Console.WriteLine("yay we are the same color! " + this.Location + " and " + sprite.Location + "total count: " + this.count);
            //    checkhit = false;
            //    if (count >= 3)
            //        IsRemove = true;

            //    }
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
