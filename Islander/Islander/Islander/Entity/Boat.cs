using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/* score from bringing artifacts to base, not being hostile hostile to player for 10 seconds, lose points when cargo is dropped */


namespace Islander.Entity
{
    class Boat : Entity
    {
        public Colour Colour { get; protected set; }

        private const float MAX_VELOCITY = 10;

        private float speed = 10;
        private Vector2 velocity;
        private Vector2 acceleration;
        private Vector2 dir = Vector2.Zero;

        public Boat(Texture2D sprite, Colour colour) : base(sprite)
        {
            Colour = colour;
            scale = new Vector2(0.5f);
        }

        // creates a new boat matching the specified colour, loading the sprite from the contentmanager
        public static Boat InitializeFromColour(Colour colour, ContentManager content)
        {
            // retrieve texture name matching specified colour
            string textureName = "";
            switch (colour)
            {
                case Colour.Blue:
                    textureName = "BoatBlue";
                    break;
                case Colour.Yellow:
                    textureName = "BoatYellow";
                    break;
                case Colour.Red:
                    textureName = "BoatRed";
                    break;
                case Colour.Green:
                    textureName = "BoatGreen";
                    break;
            }

            // load the texture specified from a folder named Boats
            Texture2D sprite = content.Load<Texture2D>("Boats/" + textureName);

            // create a new entity using the loaded sprite
            return new Boat(sprite, colour);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (VectorMagnitude(velocity) > 0)
            {
                dir = velocity;
                dir.Normalize();
            }

            if (VectorMagnitude(velocity) > 0 && VectorMagnitude(acceleration) < 1)
            {
                acceleration = -dir * 2.0f;
            }

            if(VectorMagnitude(velocity) < 100)
                velocity += acceleration;
            else
            {
                velocity -= 1 * dir;
            }

            rotation = GetRotation();
        }

        private float GetRotation()
        {
            Vector2 up = new Vector2(0.0f, -1.0f);
            up.Normalize();

            double x = Vector2.Dot(up, dir) / (VectorMagnitude(up) * VectorMagnitude(dir));

            float rotation = 0.0f;

            if (Math.Abs(x) <= 1)
            {
                rotation = (float)Math.Acos(x);
            }

            if (velocity.X < 0)
                rotation = 270 - rotation;

            return rotation;
        }

        private double VectorMagnitude(Vector2 v)
        {
            return Math.Sqrt((Math.Pow(v.X, 2.0) + Math.Pow(v.Y, 2.0)));
        }

        public void HandleInput(KeyboardState keyboardState)
        {
            Vector2 moveDir = Vector2.Zero;
            Vector2 shootDir = Vector2.Zero;
            
            if (keyboardState.IsKeyDown(Keys.A))
            {
                moveDir.X -= 1.0f;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                moveDir.X += 1.0f;
            }
            if (keyboardState.IsKeyDown(Keys.W))
            {
                moveDir.Y += 1.0f;
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                moveDir.Y -= 1.0f;
            }
            
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                shootDir.X -= 1.0f;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                shootDir.X += 1.0f;
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                shootDir.Y -= 1.0f;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                shootDir.Y += 1.0f;
            }

            HandleInput(moveDir, shootDir);

        }

        public void HandleInput(Vector2 leftThumbStick, Vector2 rightThumbStick)
        {
            HandleMove(leftThumbStick);
            HandleShoot(rightThumbStick);
        }

        public void HandleMove(Vector2 leftThumbStick) 
        {
            leftThumbStick.Y = -leftThumbStick.Y;

            /*dir = Vector2.Zero;
            dir.X += leftThumbStick.X;
            dir.Y -= leftThumbStick.Y;

            if(dir.Length() > 1.0f)
                dir.Normalize();*/

            acceleration = leftThumbStick * speed;
        }

        public void HandleShoot(Vector2 rightThumbStick)
        {
            double x = rightThumbStick.X;
            double y = rightThumbStick.Y;

            double mag = Math.Sqrt((Math.Pow(x,2.0) + Math.Pow(y,2.0)));
            if (mag > 0.5)
            {
                ShootBullet(rightThumbStick);
            }
        }

        private void ShootBullet(Vector2 dir)
        {

        }

        /*public void HandleInput(InputState input, int index)
        {
            KeyboardState keyboardState = input.CurrentKeyboardStates(index);
            GamePadState gamePadState = input.CurrrentGamePadStates(index);

            this.twinstick = false;
            Vector2 direction = Vector2.Zero;
            this.bulletDir = Vectror2.Zero;

            direction.X += gamePadState.Thumbsticks.Left.X;
            direction.Y -= gamePadState.ThumbSticks.Left.Y;

            bulletDir.X += gamePadState.Thumbsticks.Right.X;
            bulletDir.Y += gamePadState.Thumbsticks.Right.Y;



        }*/
    }
}
