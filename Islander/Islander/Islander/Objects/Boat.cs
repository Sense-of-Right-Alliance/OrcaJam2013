using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Islander.Objects
{
    class Boat : Entity
    {
        private const float MAX_VELOCITY = 1000;

        private float speed = 10;
        private Vector2 velocity;
        private Vector2 acceleration;

        public Boat(Texture2D sprite) : base(sprite)
        {

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
                moveDir.X = -1.0f;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                moveDir.X = 1.0f;
            }
            if (keyboardState.IsKeyDown(Keys.W))
            {
                moveDir.Y = -1.0f;
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                moveDir.Y = 1.0f;
            }
            
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                shootDir.X = -1.0f;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                shootDir.X = 1.0f;
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                shootDir.Y = -1.0f;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                shootDir.Y = 1.0f;
            }

            HandleInput(moveDir, shootDir);

        }

        public void HandleInput(Vector2 left, Vector2 right)
        {
            HandleMove(left);
            HandleShoot(right);
        }

        public void HandleMove(Vector2 left) 
        {
            acceleration += left * speed;
        }

        public void HandleShoot(Vector2 right)
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
            direction.Y -= gamePadState.ThumbSticks.LEft.Y;

            bulletDir.X += gamePadState.Thumbsticks.Right.X;
            bulletDir.Y += gamePadState.Thumbsticks.Right.Y;



        }*/
    }
}
