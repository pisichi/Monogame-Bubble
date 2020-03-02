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

        SpriteFont _font;

        public bool IsActive = true;
        public bool Isshooting;


        private bool IsCheck = false;


        public Vector2 Location;

        bool Ishitting = false;
        bool Inplace = false;
        bool TouchTop;


        int Pcount = 0;
        int _yeet  = 1;

        Texture2D _texture2;

        public enum BubbleState
        {
            shooting,
            hitting,
            inplace
        }

        public BubbleState _bubbleState;

        public Bubble(Texture2D texture,SpriteFont font) : base(texture)
        {
            Scale = new Vector2(Singleton.BOBBLESIZE / texture.Width, Singleton.BOBBLESIZE / texture.Width);
            RotationVelocity = 0.1f;
            radius = (texture.Width) / 2;
            _ObjType = ObjType.bubble;

            this._font = font;

        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects, Bubble[,] bubble)
        {

            if (_color.A < 255)
                _color.A += 1;

            if (!IsActive)
            {
                LinearVelocity = 0;
                RotationVelocity = 0;
            }


            Position += Direction * LinearVelocity;
            Rotation += RotationVelocity;
            CheckColision(gameObjects, bubble);
            

            if (count > 4)
            {
                bubble[(int)Location.X, (int)Location.Y] = null;
                //IsRemove = true;
                IsActive = true;
            }

            //Checkneighbor(bubble);

            base.Update(gameTime, gameObjects, bubble);
        }

        private void CheckColision(List<GameObject> gameObjects, Bubble[,] bubble)
        {

            foreach (var sprite in gameObjects)
            {
                if (sprite == this)
                    continue;

                Vector2 distance = this.Position - sprite.Position;
                if (distance.Length() < this.radius + sprite.radius && this.IsActive && sprite._ObjType == ObjType.bubble)
                {
                    Console.WriteLine("I'm " + this._color + " I'm hitting " + sprite._color + " And i'm at + " + this.Position + " " + this.Location);
                    IsActive = false;

                    Console.WriteLine(" ===================================================================== ");
                    DebugPosition(bubble);

                    CheckLocation(bubble);
                    CheckColor(bubble);
                    IsCheck = false;



                }
            }

            #region border colision

            if (Position.X - Origin.X <= 600 && Direction.X / LinearVelocity < 600)
            {
                Direction.X *= -1;
            }

            else if (IsActive && Position.Y - Origin.Y <= Singleton._down + 100 && Direction.Y / LinearVelocity < Singleton._down + 100)
            {
                IsActive = false;
                CheckLocation(bubble);
                CheckColor(bubble);
                IsCheck = false;
            }

            else if (Position.X + Origin.X >= Singleton.BoardWidth + 600 && Direction.X / LinearVelocity < Singleton.BoardWidth + 600)
            {
                Direction.X *= -1;
            }
            #endregion

        }

        private void CheckLocation(Bubble[,] bubble)
        {
            IsCheck = false;
            int i = (int)(this.Position.Y - 100 - Singleton._down + radius) / Singleton.BOBBLESIZE;
            int j = (int)(this.Position.X - 600 - 15 + radius - ((i % 2) == 0 ? 0 : 30)) / (Singleton.BOBBLESIZE + 5);

            this.Position = new Vector2(600 + 15 + j * (Singleton.BOBBLESIZE + 5) + ((i % 2) == 0 ? 0 : 30), Singleton._down + 100 + i * (Singleton.BOBBLESIZE));
            bubble[i, j] = this;
            Location = new Vector2(i, j);

            Console.WriteLine(i + "  |  " + j);
        }

        private void CheckColor( Bubble[,] bubble)
        {

            if (IsCheck)
            {
                return;
            }

          

            for (int i = (int)Location.X - 1; i <= Location.X + 1; i += 1)
            {

                for (int j = (int)Location.Y - 1; j <= Location.Y + 1; j += 1)
                {

                    if (i < 0 || j < 0 || i > 17 || j > 8 )
                        continue;

                    //null handeler
                    if (bubble[i, j] == null || bubble[i, j] == bubble[(int)Location.X, (int)Location.Y])
                        continue;
                    
                        
                    //even row
                    if (Location.X % 2 == 0 && ((i == Location.X - 1 && j == Location.Y + 1) || (i == Location.X + 1 && j == Location.Y + 1)))
                        continue;

                    //odd row
                    else if (Location.X % 2 != 0 && ((i == Location.X - 1 && j == Location.Y - 1) || (i == Location.X + 1 && j == Location.Y - 1)))
                        continue;

                    if (_color == bubble[i, j]._color)
                    {

                        Console.Write("   yay we are the same color at " + i + " " + j + " ");

                        bubble[i, j].count +=1 ;
                        bubble[(int)Location.X, (int)Location.Y].count += 1;

                        if (bubble[i, j].count >= bubble[(int)Location.X, (int)Location.Y].count)
                            bubble[(int)Location.X, (int)Location.Y].count = bubble[i, j].count;

                        else bubble[i, j].count = bubble[(int)Location.X, (int)Location.Y].count;

                        IsCheck = true;

                        bubble[i, j].CheckColor(bubble);

                    }


                    Console.Write("color at " + i + " " + j + " is:" + bubble[i, j]._color);

                }
                Console.WriteLine("");
            }
          
            DebugPosition(bubble);
        }

        //private void Checkneighbor(Bubble[,] bubble)
        //{
        //    int _yeet = 0;

        //    for (int i = (int)Location.X - 1; i <= Location.X; i += 1)
        //    {
        //        for (int j = (int)Location.Y - 1; j <= Location.Y + 1; j += 1)
        //        {

        //            if (i < 0 || j < 0 || i > 17 || j > 8)
        //                continue;

        //            if (bubble[i, j] != null)
        //                _yeet += 1;



        //            if (_yeet == 0 && (int)Location.X != 0)
        //            {
        //                bubble[(int)Location.X, (int)Location.Y] = null;
        //                IsRemove = true;
        //            }

        //        }
        //    }
        //}

        private static void DebugPosition(Bubble[,] bubble)
        {
            for (int i = 0; i < 12; i += 1)
            {
                for (int j = 0; j < 9 - (i % 2); j += 1)
                {
                    if (bubble[i, j] == null)
                    {
                        Console.Write("*                  *");
                        continue;
                    }

                    Console.Write("color at " + i + " " + j + " is:  " + bubble[i, j]._yeet + " ");
                }
                Console.WriteLine("");
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            if (Isshooting)
            {
                
            }
          

            spriteBatch.Draw(_texture, Position, null, _color, Rotation, Origin, 1f, SpriteEffects.None, 0);

            spriteBatch.DrawString(_font, " " + count, Position, Color.Black);

            base.Draw(spriteBatch);
        }








    }
}
