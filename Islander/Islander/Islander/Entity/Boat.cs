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

        private Texture2D trailTexture;
        private List<BoatTrail> trailEffects { private get; private set; }

        public Resource CarriedResource { get; set; }

        public Boat(Texture2D sprite,Texture2D trail, Colour colour) : base(sprite)
        {
            Colour = colour;
            scale = new Vector2(0.5f);
            CarriedResource = null;

            trailTexture = trail;
            trailEffects = new List<BoatTrail>();
        }

        // creates a new boat matching the specified colour, loading the sprite from the contentmanager
        public static Boat InitializeFromColour(Colour colour, ContentManager content)
        {
            // retrieve texture name matching specified colour
            string textureName = "";
            switch (colour)
            {
                case Colour.Blue:
                    textureName = "BlueBoat";
                    break;
                case Colour.Yellow:
                    textureName = "YellowBoat";
                    break;
                case Colour.Red:
                    textureName = "RedBoat";
                    break;
                case Colour.Green:
                    textureName = "GreenBoat";
                    break;
            }

            // load the texture specified from a folder named Boats
            Texture2D sprite = content.Load<Texture2D>("Boats/" + textureName);

            Texture2D trailTexture = content.Load<Texture2D>("Boats/BoatTrail");

            // create a new entity using the loaded sprite
            return new Boat(sprite, trailTexture, colour);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (velocity.Length() > 1)
            {
                dir = velocity;
                dir.Normalize();
            }

            if (VectorMagnitude(velocity) > 0 && VectorMagnitude(acceleration) < 1)
            {

                if (velocity.Length() < 1.0f)
                {
                    velocity *= 0.0f;
                    acceleration *= 0.0f;
                }
                else
                {
                    acceleration = -dir * velocity.Length() / 100;
                }
            }

            if(VectorMagnitude(velocity) < 100)
                velocity += acceleration;
            else if (velocity.Length() > 1)
            {
                Vector2 opp = velocity;
                opp.Normalize();
                velocity -= 1 * opp;
            }

            if (velocity.Length() > 0.0)
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

            if (dir.X < 0)
            {
                float deg = rotation * 180 / (float)Math.PI;
                deg = 360 - deg;
                rotation = deg * (float)Math.PI / 180;
            }

            return rotation;
        }

        private double VectorMagnitude(Vector2 v)
        {
            return Math.Sqrt((Math.Pow(v.X, 2.0) + Math.Pow(v.Y, 2.0)));
        }

        public void HandleInput(KeyboardState keyboardState)
        {
            Vector2 moveDir = Vector2.Zero;
            
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

            HandleInput(moveDir);

        }

        public void HandleInput(Vector2 leftThumbStick)
        {
            HandleMove(leftThumbStick);
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
