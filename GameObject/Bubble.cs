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

        bool Ishitting = false;
        bool Inplace = false;
        bool TouchTop;

        Texture2D _texture2;

        public enum BubbleState
        {
            shooting,
            hitting,
            inplace
        }

        public BubbleState _bubbleState;






        public Rectangle Rectangle2
        {
            get
            {
                return new Rectangle((int)Position.X - 45 * 2, (int)Position.Y - 45 * 2, _texture.Width + 30 * 4, _texture.Height + 30 * 4);
            }
        }


        public Bubble(Texture2D texture) : base(texture)
        {
            Scale = new Vector2(Singleton.BOBBLESIZE /texture.Width ,Singleton.BOBBLESIZE /texture.Width);
            RotationVelocity = 0.1f;
            radius = texture.Width / 2;
            _ObjType = ObjType.bubble;

        }


        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            if (!IsActive)
            {
                LinearVelocity = 0;
                RotationVelocity = 0;
            }

            if (count >= 3)
                IsRemove = true;


            Position += Direction * LinearVelocity;
            _rotation += RotationVelocity;

            CheckColision(gameObjects);



        }



        private void CheckColision(List<GameObject> gameObjects)
        {
            foreach (var sprite in gameObjects)
            {
                if (sprite == this)
                    continue;

                Vector2 distance = this.Position - sprite.Position;
                if (distance.Length() < this.radius + sprite.radius && this.IsActive && sprite._ObjType == ObjType.bubble)
                {
                    Console.WriteLine("I'm " + this._color + " I'm hitting " + sprite._color + " And i'm at + " + this.Position);
                    IsActive = false;
                    checkhit = true;

                }
                else
                {

                }
                CheckColor(sprite);

            }

            if (Position.X - Origin.X <= 200 && Direction.X / LinearVelocity < 200)
            {
                Direction.X *= -1;
            }

            else if (Position.Y - Origin.Y <= 60 && Direction.Y / LinearVelocity < 60)
            {
                IsActive = false;
            }

            else if (Position.X + Origin.X >= Singleton.BoardWidth + 200 && Direction.X / LinearVelocity < Singleton.BoardWidth + 200)
            {
                Direction.X *= -1;
            }

        }

        private void CheckColor(GameObject sprite)
        {
            //if (this._color == sprite._color && checkhit)
            //{
            //    sprite.count++;
            //    this.count++;

            //    Console.WriteLine("yay we are the same color! " + this.Location + " and " + sprite.Location + "total count: " + this.count);
            //    checkhit = false;
            //}
        }

        public override void Draw(SpriteBatch spriteBatch) 
        {

            if (Isshooting)
            {
                spriteBatch.Draw(_texture, destinationRectangle: Rectangle2, color: Color.Red * 0.2f);
            }

            spriteBatch.Draw(_texture, Position, null, _color, _rotation, Origin, 1f, SpriteEffects.None, 0);
            
            base.Draw(spriteBatch);
        }



      


    }
}
