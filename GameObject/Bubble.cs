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
  

        Random rnd = new Random();


        public Bubble(Texture2D texture) : base(texture)
        {
            Scale = new Vector2(Singleton.BOBBLESIZE /texture.Width ,Singleton.BOBBLESIZE /texture.Width);

        }


        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {

            if (!IsAcive)
                LinearVelocity = 0;

            Position += Direction * LinearVelocity;

            if(Position.X - Origin.X <= Singleton.BOBBLESIZE * 2 && Direction.X / LinearVelocity < Singleton.BOBBLESIZE * 2)
            {
                Direction.X *= -1;
            }

            else if (Position.Y - Origin.Y <= Singleton.BOBBLESIZE && Direction.Y / LinearVelocity < Singleton.BOBBLESIZE)
            {
                LinearVelocity = 0;
                IsRemove = true;
            }

            else if (Position.X + Origin.X >= Singleton.BoardWidth - (Singleton.BOBBLESIZE * 2) && Direction.X / LinearVelocity < Singleton.BoardWidth - (Singleton.BOBBLESIZE * 4))
            {
                Direction.X *= -1;
            }



        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, _color, _rotation, Origin, Scale * 2f, SpriteEffects.None , 0);
            base.Draw(spriteBatch);
        }


     


    }
}
