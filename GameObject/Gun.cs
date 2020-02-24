using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bobble_Game_Mid.gameObject
{
    class Gun : GameObject
    {
        public Bubble Bubble;
        Random rnd = new Random();
        bool shooting = true;
        int _currentColor;


        public Gun(Texture2D texture) : base(texture)
        {
        }

        public override void Update(GameTime gameTime,List<GameObject> gameObjects)
        {
            _previouskey = _currentkey;
            _currentkey = Keyboard.GetState();

            Direction = new Vector2((float)Math.Cos(_rotation), (float)Math.Sin(_rotation));

            if (_currentkey.IsKeyDown(Keys.A))
                _rotation -= MathHelper.ToRadians(RotationVelocity);
            else if(_currentkey.IsKeyDown(Keys.D))
                _rotation += MathHelper.ToRadians(RotationVelocity);

         
                AddBubble(gameObjects);
 


        }


        private void AddBubble(List<GameObject> gameObjects)
        {
            var bubble = Bubble.Clone() as Bubble;
            bubble.Direction = this.Direction;
            bubble.Position = this.Position;
            bubble.LinearVelocity = 0;
            bubble._color = this.GetRandomColor();
            



            if (_currentkey.IsKeyDown(Keys.Space) && _previouskey.IsKeyUp(Keys.Space))
            {   

                bubble.LinearVelocity = this.LinearVelocity * 5;
                shooting = true;
                gameObjects.Add(bubble);
            }
        }


        public Color GetRandomColor()
        {
            Color _color = Color.Black;

            if (shooting) {
                _currentColor = rnd.Next(0, 6);
                shooting = false;
             }

            switch (_currentColor)
            {
                case 0:
                    _color = Color.White;
                    break;
                case 1:
                    _color = Color.Blue;
                    break;
                case 2:
                    _color = Color.Yellow;
                    break;
                case 3:
                    _color = Color.Red;
                    break;
                case 4:
                    _color = Color.Green;
                    break;
                case 5:
                    _color = Color.Purple;
                    break;
            }
            return _color;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color.White , _rotation, Origin, 1, SpriteEffects.None, 0);
            base.Draw(spriteBatch);
        }
    }
}
