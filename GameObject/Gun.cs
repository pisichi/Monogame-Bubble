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
        Color _gunColor;
        Texture2D _eye;


        public Gun(Texture2D texture) : base(texture)
        {
            _rotation = -1.6f;
            ISgun = true;

        }


        public override void Update(GameTime gameTime,List<GameObject> gameObjects)
        {
            _previouskey = _currentkey;
            _currentkey = Keyboard.GetState();

            Direction = new Vector2((float)Math.Cos(_rotation), (float)Math.Sin(_rotation));

            CheckInput();



            Console.WriteLine(_rotation);
            AddBubble(gameObjects);


        }

        private void CheckInput()
        {
            if (_currentkey.IsKeyDown(Keys.A))
            {
                if (_rotation > -2.6)
                    _rotation -= MathHelper.ToRadians(RotationVelocity);
            }
            else if (_currentkey.IsKeyDown(Keys.D))
            {
                if (_rotation < -0.6)
                    _rotation += MathHelper.ToRadians(RotationVelocity);
            }
        }


        private void AddBubble(List<GameObject> gameObjects)
        {
            var bubble = Bubble.Clone() as Bubble;
            bubble.Direction = this.Direction;
            bubble.Position = this.Position;
            bubble.LinearVelocity = 0;
            bubble._color = this.GetRandomColor();
            //bubble.Isshooting = false;

            



            if (_currentkey.IsKeyDown(Keys.Space) && _previouskey.IsKeyUp(Keys.Space))
            {   

                bubble.LinearVelocity = this.LinearVelocity *2;
                bubble.Isshooting = true;
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

              //_currentColor = rnd.Next(0, 6);

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
            _gunColor = _color;
            return _color;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {   
            if(_rotation >= -1.6f)
            spriteBatch.Draw(_texture, Position , null, Color.White, _rotation, Origin + new Vector2(-30,30), 1f, SpriteEffects.None, 0);
            else
            spriteBatch.Draw(_texture, Position, null, Color.White, _rotation, Origin + new Vector2(-30, -30), 1f, SpriteEffects.FlipVertically , 0);
            base.Draw(spriteBatch);
        }
    }
}
