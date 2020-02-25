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
        public bool Isshooting;

        //bool Ishitting = false;
        //bool Inplace = false;
        bool TouchTop;

        Texture2D _texture2;

  

        Random rnd = new Random();


        //public Rectangle Rectangle
        //{
        //    get
        //    {

        //        //return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width  , _texture.Height );
        //        return new Rectangle((int)Position.X , (int)Position.Y , _texture.Width , _texture.Height) ;
        //    }
        //}


        public Bubble(Texture2D texture) : base(texture)
        {
            Scale = new Vector2(Singleton.BOBBLESIZE /texture.Width ,Singleton.BOBBLESIZE /texture.Width);


        }


        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {

            if (!IsAcive)
                LinearVelocity = 0;

            Position += Direction * LinearVelocity;

            CheckColision(gameObjects);

            //if(!Inplace && Ishitting && Position.X % Singleton.BOBBLESIZE != 0)
            //{
            //    this.Position.X += Position.X % Singleton.BOBBLESIZE;
            //    Ishitting = false;
            //    Inplace = true;
            //}

            if(!IsAcive && !TouchTop)
            {
                IsRemove = true;
            }

            

        }


        private void CheckColision(List<GameObject> gameObjects)
        {



            foreach (var sprite in gameObjects)
            {
                if ((this.Direction.X / this.LinearVelocity > 0 && this.IsTouchingLeft(sprite)) ||
                    (this.Direction.X / this.LinearVelocity < 0 & this.IsTouchingRight(sprite)))
                {

                    //if (this.IsAcive)
                    //{
                    //    this.Direction.X *= -1;
                    //}
                    //else
                    //this.Direction.X *= -1;
                    //Ishitting = true;
                    //LinearVelocity = 0;
                    IsAcive = false;

                }

                if ((this.Direction.Y / this.LinearVelocity > 0 && this.IsTouchingTop(sprite)) ||
                    (this.Direction.Y / this.LinearVelocity < 0 & this.IsTouchingBottom(sprite)))
                {
                    //if (this.IsAcive)
                    //{
                    //    this.Direction.Y *= -1;
                    //}
                    //else
                    //this.Direction.Y *= -1;
                    //Ishitting = true;
                    //LinearVelocity = 0;
                    IsAcive = false;
                    TouchTop = true;

                }

            }

            if (Position.X - Origin.X <= 200 && Direction.X / LinearVelocity < 200)
            {
                Direction.X *= -1;
            }

            else if (Position.Y - Origin.Y <= 60 && Direction.Y / LinearVelocity < 60)
            {
                //LinearVelocity = 0;
                //IsRemove = true;
                IsAcive = false;
            }

            else if (Position.X + Origin.X >= Singleton.BoardWidth + 200 && Direction.X / LinearVelocity < Singleton.BoardWidth + 200)
            {
                Direction.X *= -1;
            }

        }

        public override void Draw(SpriteBatch spriteBatch) 
        {

            spriteBatch.Draw(_texture, destinationRectangle: Rectangle);
            spriteBatch.Draw(_texture, Position, null, _color, _rotation, Origin, 1f, SpriteEffects.None, 0);
            
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


        //protected bool IsTouchingLeft(GameObject gameObject)
        //{
        //    return this.Rectangle.Right > gameObject.Rectangle.Left &&
        //            this.Rectangle.Left < gameObject.Rectangle.Left &&
        //            this.Rectangle.Bottom > gameObject.Rectangle.Top &&
        //            this.Rectangle.Top < gameObject.Rectangle.Bottom;
        //}

        //protected bool IsTouchingRight(GameObject gameObject)
        //{
        //    return this.Rectangle.Right > gameObject.Rectangle.Right &&
        //            this.Rectangle.Left < gameObject.Rectangle.Right &&
        //            this.Rectangle.Bottom > gameObject.Rectangle.Top &&
        //            this.Rectangle.Top < gameObject.Rectangle.Bottom;
        //}

        //protected bool IsTouchingTop(GameObject gameObject)
        //{
        //    return this.Rectangle.Right > gameObject.Rectangle.Left &&
        //            this.Rectangle.Left < gameObject.Rectangle.Right &&
        //            this.Rectangle.Bottom > gameObject.Rectangle.Top &&
        //            this.Rectangle.Top < gameObject.Rectangle.Top;
        //}

        //protected bool IsTouchingBottom(GameObject gameObject)
        //{
        //    return this.Rectangle.Right > gameObject.Rectangle.Left &&
        //            this.Rectangle.Left < gameObject.Rectangle.Right &&
        //            this.Rectangle.Bottom > gameObject.Rectangle.Bottom &&
        //            this.Rectangle.Top < gameObject.Rectangle.Bottom;
        //}

        #endregion







    }
}
