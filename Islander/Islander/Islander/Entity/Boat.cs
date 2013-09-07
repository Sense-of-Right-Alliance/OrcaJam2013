using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Islander.Entity
{
    class Boat : Entity
    {
        public Colour Colour { get; protected set; }

        private const float MAX_VELOCITY = 1000;

        private float speed = 10;
        private Vector2 velocity;
        private Vector2 acceleration;

        public Boat(Texture2D sprite, Colour colour) : base(sprite)
        {
            Colour = colour;
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

            position += velocity;
            velocity += acceleration;
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
                moveDir.Y -= 1.0f;
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                moveDir.Y += 1.0f;
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
            acceleration += leftThumbStick * speed;
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
