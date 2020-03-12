using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bubble_Game_Mid.gameObject
{
    class Dragon : GameObject
    {

        public Bubble Bubble;
        Random rnd = new Random();
        bool shooting = true;
        float cooldowntime = 0;

        SoundEffect _shoot;
        SoundEffect _shoot_S;
        SoundEffect _skill;
        SoundEffect _skill_S;

        Texture2D _texture2;
        int _currentColor;

        public Dragon(Texture2D texture,Texture2D texture2, SoundEffect _shoot, SoundEffect _shoot_S, SoundEffect _skill, SoundEffect _skill_S) : base(texture)
        {
            Rotation = -1.6f;
            _ObjType = ObjType.gun;
            this._texture2 = texture2;
            this._shoot = _shoot;
            this._shoot_S = _shoot_S;
            this._skill = _skill;
            this._skill_S = _skill_S;
        }


        public override void Update(GameTime gameTime,List<GameObject> gameObjects)
        {


            _previouskey = _currentkey;
            _currentkey = Keyboard.GetState();

            Direction = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
            cooldowntime += gameTime.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond;
            CheckInput();

            if(Singleton.Instance.ult)
                Singleton.CurrentColor = GetRandomColor();


                AddBubble(gameObjects);

            base.Update(gameTime, gameObjects);


        }

        private void CheckInput()
        {
            if (_currentkey.IsKeyDown(Keys.A))
            {
                if (Rotation > -2.6)
                    Rotation -= MathHelper.ToRadians(RotationVelocity);
            }
            else if (_currentkey.IsKeyDown(Keys.D))
            {
                if (Rotation < -0.6)
                    Rotation += MathHelper.ToRadians(RotationVelocity);
            }

        }


        private void AddBubble(List<GameObject> gameObjects)
        {
            var bubble = Bubble.Clone() as Bubble;
            bubble.Direction = this.Direction;
            bubble.Position = this.Position;
            
           

            bubble._color = this.GetRandomColor();

            if (cooldowntime >= 0.8 && _currentkey.IsKeyDown(Keys.Space) && _previouskey.IsKeyUp(Keys.Space))
            {


                if (Singleton.Instance.ult)
                {
                    _shoot_S.Play();
                    bubble.special = true;
                    bubble.LinearVelocity = this.LinearVelocity * 30;
                    bubble.RotationVelocity = 50f;
                }
                else
                {
                    _shoot.Play();
                    bubble.LinearVelocity = this.LinearVelocity * 15;
                }

                shooting = true;
                gameObjects.Add(bubble);
                cooldowntime = 0;
            }

            else if (_currentkey.IsKeyDown(Keys.X) && _previouskey.IsKeyUp(Keys.X) && Singleton.Charge >= 1)
            {
                _skill.Play();
                Singleton.Charge -= 1;
                bubble._color = this.GetRandomColor(); 
                shooting = true;
            }

            else if (_currentkey.IsKeyDown(Keys.Z) && _previouskey.IsKeyUp(Keys.Z) && Singleton.Charge >= 12)
            {
                _skill_S.Play();
                Singleton.Charge -= 12;
                Singleton.Instance.ult = true;
            }
        }

        public Color GetRandomColor()
        {
            Color _color = Color.White;

            if (shooting || Singleton.Instance.ult) {
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
            Singleton.CurrentColor = _color;
            return _color;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {

          
            if (Rotation >= -1.6f)
            {
                spriteBatch.Draw(_texture, Position, null, Color.White, Rotation - 0.2f, Origin + new Vector2(-30, 30), 1f, SpriteEffects.None, 0);
                spriteBatch.Draw(_texture2, Position, null, Singleton.CurrentColor, Rotation - 0.2f, Origin + new Vector2(-30, 30), 1f, SpriteEffects.None, 0);
            }
            else
            {
                spriteBatch.Draw(_texture, Position, null, Color.White, Rotation, Origin + new Vector2(-30, -30), 1f, SpriteEffects.FlipVertically, 0);
                spriteBatch.Draw(_texture2, Position, null, Singleton.CurrentColor, Rotation, Origin + new Vector2(-30, -30), 1f, SpriteEffects.FlipVertically, 0);
            }
            base.Draw(spriteBatch);
        }
    }
}
