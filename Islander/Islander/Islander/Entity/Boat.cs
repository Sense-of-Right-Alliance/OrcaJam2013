using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

/* score from bringing artifacts to base, not being hostile hostile to player for 10 seconds, lose points when cargo is dropped */

namespace Islander.Entity
{
    class Boat : Entity
    {
        public enum BoatState
        {
            uninitialized,
            alive,
            dead,
            respawning
        }

        public Colour Colour { get; protected set; }

        private const float MAX_VELOCITY = 10;
        private const float SPAWN_TIME = 3.0f;

        private int hits = 0;
        public BoatState state { get; protected set; }
        private float spawnTimer = 0.0f;

        private float speed = 10;
        private Vector2 velocity;
        private Vector2 acceleration;
        private Vector2 dir = Vector2.Zero;

        private Texture2D trailTexture;
        private List<BoatTrail> trailEffects { get; set; }

        public Resource CarriedResource { get; set; }

        public Boat(Texture2D sprite,Texture2D trail, Colour colour) : base(sprite)
        {
            Colour = colour;
            scale = new Vector2(0.5f);
            CarriedResource = null;

            trailTexture = trail;
            trailEffects = new List<BoatTrail>();

            state = BoatState.alive;
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

        public void Hit()
        {
            hits++;
            if (hits >= 5)
            {
                state = BoatState.dead;
            }
        }

        public void WaitForRespawn(Vector2 spawnPosition)
        {
            position = spawnPosition;
            state = BoatState.respawning;
            hits = 0;
            spawnTimer = 0.0f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (state == BoatState.respawning)
            {
                spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (spawnTimer >= SPAWN_TIME)
                {
                    state = BoatState.alive;
                }
            }
            else if (state == BoatState.alive)
            {
                HandleMove(gameTime);
            }

                
        }

        private void HandleMove(GameTime gameTime)
        {
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

            if (VectorMagnitude(velocity) < 100)
                velocity += acceleration;
            else if (velocity.Length() > 1)
            {
                Vector2 opp = velocity;
                opp.Normalize();
                velocity -= 1 * opp;
            }

            //Problem Solving!
            if (velocity.Length() > 0.0)
                rotation = GetRotation();

            //TODO: This will need to be more specific with the width of the boat. Also we should have called them ships
            if (position.X < 0)
                position.X = 0;
            else if (position.X > 1200)//TODO: SHIT WE NEED SCREEN WIDTH HERE. AAAHHH
                position.X = 1200;
            if (position.Y < 0)
                position.Y = 0;
            else if (position.Y > 620)//TODO: Seriously so hacky. Please get me screen width.
                position.Y = 620;
            //TODO: This is literally killing me. Please let's fix this. My body hurts.
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (state == BoatState.alive)
            {
                base.Draw(spriteBatch);
            }

            if (CarriedResource != null)
            {
                CarriedResource.SetPosition(this.position);
                CarriedResource.SetRotation(this.rotation);
                CarriedResource.Draw(spriteBatch);
            }
        }
    }
}
