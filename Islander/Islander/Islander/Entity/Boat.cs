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
            Uninitialized,
            Alive,
            Dead,
            Respawning
        }

        public Colour Colour { get; protected set; }

        private const float DEFAULT_MAX_VELOCITY = 50;
        private const float SPAWN_TIME = 3.0f;

        private int hits = 0;
        public BoatState State { get; protected set; }
        private float spawnTimer = 0.0f;

        private float maxVelocity = DEFAULT_MAX_VELOCITY;

        private float speed = 10;
        private Vector2 velocity;
        private Vector2 acceleration;
        private Vector2 dir = Vector2.Zero;

        protected int screenWidth;
        protected int screenHeight;

        private Texture2D trailTexture;
        private List<Splatter> trailEffects { get; set; }
        private Vector2 lastTrail;
        private int trailIndex = 0;
        private Splatter deathSplatter;

        private float powerUpTimer = 0.0f;
        private const float POWER_UP_TIME = 10.0f;
        public bool hasPowerUp = false;

        public List<Resource> CarriedResources { get; protected set; }

        public Boat(Texture2D sprite,Texture2D trail, Texture2D splatter, Colour colour,int screenWidth,int screenHeight) : base(sprite)
        {
            Colour = colour;
            Scale = new Vector2(0.5f);

            trailTexture = trail;
            trailEffects = new List<Splatter>();

            for (int i = 0; i < 15; i++)
            {
                trailEffects.Add(new Splatter(trail,0.2f));
            }
            

            deathSplatter = new Splatter(splatter,0.5f);

            State = BoatState.Alive;

            CarriedResources = new List<Resource>();

            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
        }

        // creates a new boat matching the specified colour, loading the sprite from the contentmanager
        public static Boat InitializeFromColour(Colour colour, ContentManager content,int screenWidth,int screenHeight)
        {
            // retrieve texture name matching specified colour
            string textureName = "";
            string splatterName = "";
            string trailName = "";
            switch (colour)
            {
                case Colour.Blue:
                    textureName = "BlueBoat";
                    trailName = "BlueTrail";
                    splatterName = "BlueSplatter";
                    break;
                case Colour.Yellow:
                    textureName = "YellowBoat";
                    trailName = "YellowTrail";
                    splatterName = "YellowSplatter";
                    break;
                case Colour.Red:
                    textureName = "RedBoat";
                    trailName = "RedTrail";
                    splatterName = "RedSplatter";
                    break;
                case Colour.Green:
                    textureName = "GreenBoat";
                    trailName = "GreenTrail";
                    splatterName = "GreenSplatter";
                    break;
            }

            // load the texture specified from a folder named Boats
            Texture2D sprite = content.Load<Texture2D>("Boats/" + textureName);
            Texture2D trailTexture = content.Load<Texture2D>("BoatTrails/" + trailName);
            Texture2D splatterTexture = content.Load<Texture2D>("BoatSplatter/" + splatterName);

            // create a new entity using the loaded sprite
            return new Boat(sprite, trailTexture, splatterTexture, colour,screenWidth,screenHeight);
        }

        public void Start()
        {
            lastTrail = Position;
        }

        public void Hit(Bullet b)
        {
            hits++;
            if (hits >= 5)
            {
                State = BoatState.Dead;
                deathSplatter.CreateDeathSplatter(Position + (60.0f * b.dir) , b.Rotation);
            }

            switch (b.type)
            {
                case(Bullet.BulletType.Bubble):
                    velocity += b.dir * 100.0f;
                    break;
            }
        }

        public void WaitForRespawn(Vector2 spawnPosition)
        {
            Position = spawnPosition;
            State = BoatState.Respawning;
            hits = 0;
            spawnTimer = 0.0f;
        }

        public void GainPowerUp(PowerUp p)
        {
            maxVelocity = DEFAULT_MAX_VELOCITY + 30;
            powerUpTimer = POWER_UP_TIME;
            hasPowerUp = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (State == BoatState.Respawning)
            {
                spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (spawnTimer >= SPAWN_TIME)
                {
                    State = BoatState.Alive;
                }
            }
            else if (State == BoatState.Alive)
            {
                HandleMove(gameTime);
            }

            foreach (Splatter s in trailEffects)
                s.Update(gameTime);

            deathSplatter.Update(gameTime);

            if (hasPowerUp)
            {
                powerUpTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (powerUpTimer <= 0)
                {
                    hasPowerUp = false;
                    maxVelocity = DEFAULT_MAX_VELOCITY;
                }
            }

            for (int i = 0; i < CarriedResources.Count; i++) 
            {
                Resource resource = CarriedResources[i];
                Vector2 target = Vector2.Zero;
                if (i == 0)
                    target = Position;
                else
                    target = CarriedResources[i-1].Position;

                if ((target - resource.Position).Length() > 30.0f)
                {
                    Vector2 dir = (target - resource.Position);
                    dir.Normalize();
                    resource.Position += dir * 2.0f;
                }
            }
        }

        private void HandleMove(GameTime gameTime)
        {
            Position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

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

            if (VectorMagnitude(velocity) < maxVelocity)
                velocity += acceleration;
            else if (velocity.Length() > 1)
            {
                Vector2 opp = velocity;
                opp.Normalize();
                velocity -= 1 * opp;
            }

            //Problem Solving!
            if (velocity.Length() > 0.0)
                Rotation = GetRotation();

            //Now has proper screenWidth and screenHeight
            if (Position.X < 0)
                PositionX = 0;
            else if (Position.X > screenWidth)
                PositionX = screenWidth;
            if (Position.Y < 0)
                PositionY = 0;
            else if (Position.Y > (float)screenHeight * 4.0f / 5.0f)
                PositionY = (float)screenHeight * 4.0f / 5.0f;

            HandleTrail();
        }

        private void HandleTrail()
        {
            Vector2 d = Position - lastTrail;
            if (d.Length() > 20)
            {
                trailEffects[trailIndex].CreateDeathSplatter(Position, Rotation);
                lastTrail = Position;
                trailIndex++;
                if (trailIndex >= trailEffects.Count)
                    trailIndex = 0;
            }
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

        // check if the boat carries a resource of the same type as the one specified
        public bool CarriesResource(Resource resource)
        {
            foreach (var carriedResource in CarriedResources)
                if (carriedResource.IslandType == resource.IslandType)
                    return true;

            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // draw trail
            foreach (var trailEffect in trailEffects)
                trailEffect.Draw(spriteBatch);

            // draw death splatter
            deathSplatter.Draw(spriteBatch);

            if (State == BoatState.Alive)
            {
                base.Draw(spriteBatch);
            }

            foreach (var resource in CarriedResources)
            {
                //resource.Position = this.Position;
                //resource.Rotation = this.Rotation;
                resource.Draw(spriteBatch);
            }
        }
    }
}
