using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bubble_Game_Mid.gameObject
{
   public class GameObject : ICloneable
    {


        public Texture2D _texture;

        public float Rotation;
        public float RotationVelocity = 3f;
        public float LinearVelocity = 1f;

        public Vector2 Scale;
        public Vector2 Direction;
        
        public Vector2 Position;
        public Vector2 Origin;

        public KeyboardState _currentkey;
        public KeyboardState _previouskey;



        public int radius;
        public int count = 1;

        public bool IsRemove = false;



        public Color _color;

        public Random rnd = new Random();

        public enum ObjType
        {
            bubble,
            bubble_S,
            gun
        }
        public ObjType _ObjType;


        public GameObject(Texture2D texture) 
        {
                     
            this._texture = texture;
            this.Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
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
