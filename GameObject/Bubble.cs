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

        bool IsAcive = true;

        Texture2D _texture2;

  

        Random rnd = new Random();


        public Bubble(Texture2D texture) : base(texture)
        {
            Scale = new Vector2(Singleton.BOBBLESIZE /texture.Width ,Singleton.BOBBLESIZE /texture.Width);


        }


        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {


            foreach (var sprite in gameObjects)
            {
                if (sprite == this)
                    continue;

                if ((this.Direction.X / this.LinearVelocity > 0 && this.IsTouchingLeft(sprite)) ||
                    (this.Direction.X / this.LinearVelocity < 0 & this.IsTouchingRight(sprite)))
                {
                    this.Direction.X *= -1;
                    //LinearVelocity = 0;
                    //IsAcive = false;

                }

                if ((this.Direction.Y / this.LinearVelocity > 0 && this.IsTouchingTop(sprite)) ||
                    (this.Direction.Y / this.LinearVelocity < 0 & this.IsTouchingBottom(sprite)))
                {
                    this.Direction.Y *= -1;
                    //LinearVelocity = 0;
                    //IsAcive = false;

                }
                    
            }




            if (!IsAcive)
                LinearVelocity = 0;

            Position += Direction * LinearVelocity;

            if(Position.X - Origin.X <= Singleton.BOBBLESIZE * 2 && Direction.X / LinearVelocity < Singleton.BOBBLESIZE * 2)
            {
                Direction.X *= -1;
            }

            else if (Position.Y - Origin.Y <= Singleton.BOBBLESIZE && Direction.Y / LinearVelocity < Singleton.BOBBLESIZE)
            {
                //LinearVelocity = 0;
                //IsRemove = true;
                IsAcive = false;
            }

            else if (Position.X + Origin.X >= Singleton.BoardWidth - (Singleton.BOBBLESIZE * 2) && Direction.X / LinearVelocity < Singleton.BoardWidth - (Singleton.BOBBLESIZE * 4))
            {
                Direction.X *= -1;
            }



        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            //spriteBatch.Draw(_hitbox, new Rectangle((int)Position.X, (int)Position.Y, _texture.Width + (int)Origin.X, _texture.Height + (int)Origin.Y), Color.White);
            spriteBatch.Draw(_texture, Position, null, _color, _rotation, Origin, Scale * 2f , SpriteEffects.None , 0);
            base.Draw(spriteBatch);
        }



        #region Colloision
        protected bool IsTouchingLeft(GameObject sprite)
        {
            return this.Rectangle.Right + this.Direction.X / this.LinearVelocity > sprite.Rectangle.Left &&
              this.Rectangle.Left < sprite.Rectangle.Left &&
              this.Rectangle.Bottom > sprite.Rectangle.Top &&
              this.Rectangle.Top < sprite.Rectangle.Bottom;
        }

        protected bool IsTouchingRight(GameObject sprite)
        {
            return this.Rectangle.Left + this.Direction.X / this.LinearVelocity < sprite.Rectangle.Right &&
              this.Rectangle.Right > sprite.Rectangle.Right &&
              this.Rectangle.Bottom > sprite.Rectangle.Top &&
              this.Rectangle.Top < sprite.Rectangle.Bottom;
        }

        protected bool IsTouchingTop(GameObject sprite)
        {
            return this.Rectangle.Bottom + this.Direction.Y / this.LinearVelocity > sprite.Rectangle.Top &&
              this.Rectangle.Top < sprite.Rectangle.Top &&
              this.Rectangle.Right > sprite.Rectangle.Left &&
              this.Rectangle.Left < sprite.Rectangle.Right;
        }

        protected bool IsTouchingBottom(GameObject sprite)
        {
            return this.Rectangle.Top + this.Direction.Y / this.LinearVelocity < sprite.Rectangle.Bottom &&
              this.Rectangle.Bottom > sprite.Rectangle.Bottom &&
              this.Rectangle.Right > sprite.Rectangle.Left &&
              this.Rectangle.Left < sprite.Rectangle.Right;
        }

        #endregion







    }
}
