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
        //Texture2D _eye;


        public Gun(Texture2D texture) : base(texture)
        {
            _rotation = -1.6f;
            _ObjType = ObjType.gun;
            //_eye = Bubble._texture;
        }


        public override void Update(GameTime gameTime,List<GameObject> gameObjects, Bubble[,] bubble)
        {
            _previouskey = _currentkey;
            _currentkey = Keyboard.GetState();

            Direction = new Vector2((float)Math.Cos(_rotation), (float)Math.Sin(_rotation));
        
            CheckInput();

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
            

            

            if (_currentkey.IsKeyDown(Keys.Space) && _previouskey.IsKeyUp(Keys.Space))
            {   

                bubble.LinearVelocity = this.LinearVelocity *2;
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
                    _color.A = 0;
                    break;
                case 1:
                    _color = Color.Blue;
                    _color.A = 150;
                    break;
                case 2:
                    _color = Color.Yellow;
                    _color.A = 150;
                    break;
                case 3:
                    _color = Color.Red;
                    _color.A = 150;
                    break;
                case 4:
                    _color = Color.Green;
                    _color.A = 150;
                    break;
                case 5:
                    _color = Color.Purple;
                    _color.A = 150;
                    break;
            }
            _gunColor = _color;
            return _color;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {

            //spriteBatch.Draw(_eye, Position, null, _color, _rotation, Origin, 1f, SpriteEffects.None, 0);
            if (_rotation >= -1.6f)
            spriteBatch.Draw(_texture, Position , null, Color.White, _rotation, Origin + new Vector2(-30,30), 1.2f, SpriteEffects.None, 0);
            else
            spriteBatch.Draw(_texture, Position, null, Color.White, _rotation, Origin + new Vector2(-30, -30), 1.2f, SpriteEffects.FlipVertically , 0);
            base.Draw(spriteBatch);
        }
    }
}
