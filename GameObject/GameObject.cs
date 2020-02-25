using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bobble_Game_Mid.gameObject
{
    class GameObject : ICloneable
    {
        protected Texture2D _texture;

        public float _rotation;
        public Vector2 Scale;
        public Vector2 Velocity;
        public float RotationVelocity = 3f;
        public float LinearVelocity = 4f;


        public KeyboardState _currentkey;
        public KeyboardState _previouskey;

        public Vector2 Position;
        public Vector2 Origin;

        public Vector2 Direction;

        public bool IsRemove = false;

        public Color _color;

        public Rectangle Rectangle
        {
            get
            {

                return new Rectangle((int)Position.X -30 , (int)Position.Y - 30, _texture.Width, _texture.Height);
            }
        }

        public GameObject(Texture2D texture)
        {
            _texture = texture;
            Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);

        }



        public virtual void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
         
        }

        public virtual void Reset()
        {

        }



        public object Clone()
        {
            return this.MemberwiseClone();
        }




    }
}
