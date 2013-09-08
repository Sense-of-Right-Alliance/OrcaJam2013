using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Islander
{
    using Entity;

    enum Colour
    {
        Blue,
        Green,
        Red,
        Yellow
    }

    class Player
    {
        public enum InputMessage
        {
            NoMessage,
            SkipToNextScreen
        }
        /*CONSTANTS FOR SCORE*/
        private const int RETURN_RESOURCE = 1000;

        private const float BULLET_SPEED = 15;

        public Colour Colour { get; protected set; }
        public PlayerIndex PlayerIndex { get; protected set; }

        public InputMessage Message { get; protected set; }

        public Boat Boat { get; protected set; }
        public Island Island { get; protected set; }
        public List<Bullet> Bullets { get; set; }
        Texture2D bulletSprite;
        SoundEffect basicAttackSound;

        public Player[] PlayersByColour { get; set; }
        public bool[] HostileToPlayer { get; protected set; }
        public Resource[] CollectedResources { get; protected set; }

        protected ContentManager content;
        protected TimeSpan bulletDelay;
        protected TimeSpan bulletTimeElapsed;

        public int score { get; set; }

        public Player(PlayerIndex playerIndex, ContentManager content)
        {
            PlayerIndex = playerIndex;
            this.content = content;
            this.score = 0;
            
            PlayersByColour = null;
            HostileToPlayer = null;
        }

        public void SetGameColour(Colour colour)
        {
            Colour = colour;
            LoadBullets();
            Boat = Boat.InitializeFromColour(colour, content);
            Island = Island.InitializeFromColour(colour, content);

            // initially hostile to all players except self
            HostileToPlayer = new bool[] { true, true, true, true };
            HostileToPlayer[(int)Colour] = false;

            // default bullet delay
            bulletDelay = new TimeSpan(5000000); // 0.5s
            bulletTimeElapsed = TimeSpan.Zero;

            CollectedResources = new Resource[] { null, null, null, null };
        }

        protected void LoadBullets()
        {
            // initialize bullet list
            Bullets = new List<Bullet>();

            // load default bullet sprite
            String bulletFilename = "";
            switch (Colour)
            {
                case Colour.Blue:
                    bulletFilename = "Blue";
                    break;
                case Colour.Yellow:
                    bulletFilename = "Yellow";
                    break;
                case Colour.Red:
                    bulletFilename = "Red";
                    break;
                case Colour.Green:
                    bulletFilename = "Green";
                    break;
            }
            bulletFilename += "Default";
            bulletSprite = content.Load<Texture2D>("Bullets/" + bulletFilename);

            //Loads the bullet sound.
            basicAttackSound = content.Load<SoundEffect>("SFX/Basic Attack2");
        }

        public void RemoveBullet(Bullet bullet)
        {
            Bullets.Remove(bullet);
        }

        public void CollectResource(Resource resource)
        {
            CollectedResources[(int)resource.Colour] = resource;
            score += RETURN_RESOURCE;
        }

        public virtual void HandleInput(Islander.GameState gameState, GameTime gameTime)
        {
            // get the keyboard state
            KeyboardState keyboardState = Keyboard.GetState();

            // get the current player's gamepad state
            GamePadState gamePadState = GamePad.GetState(PlayerIndex);

            // if no input detected, there is no message to send
            Message = InputMessage.NoMessage;

            bulletTimeElapsed += gameTime.ElapsedGameTime;

            // keyboard input
            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                Message = InputMessage.SkipToNextScreen;
            }
            else if (gameState == Islander.GameState.RunningGame)
            {
                CheckShooting(keyboardState);

                Boat.HandleInput(keyboardState);
            }

            // gamepad input
            if (!gamePadState.IsConnected)
            {
                //Debug.WriteLine("Player " + PlayerIndex.ToString() + "'s controller is disconnected.");
                return;
            }
            else
            {
                if (gamePadState.Buttons.Start == ButtonState.Pressed)
                    Message = InputMessage.SkipToNextScreen;
                else if (gameState == Islander.GameState.RunningGame)
                {
                    Boat.HandleInput(gamePadState.ThumbSticks.Left);

                    CheckShooting(gamePadState.ThumbSticks.Right);
                }
            }
        }

        
        public void CheckShooting(KeyboardState keyboardState)
        {
            Vector2 shootDir = Vector2.Zero;

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
                shootDir.Y += 1.0f;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                shootDir.Y -= 1.0f;
            }

            CheckShooting(shootDir);
        }

        public void CheckShooting(Vector2 direction)
        {
            if (direction.Length() >= 0.5f)
                if (bulletTimeElapsed > bulletDelay)
                {
                    direction.Y = -direction.Y;
                    ShootBullet(direction);
                }
        }

        private void ShootBullet(Vector2 direction)
        {
            //Bullet shooting
            basicAttackSound.Play();
            bulletTimeElapsed = TimeSpan.Zero;
            direction.Normalize();
            Bullet bullet = new Bullet(bulletSprite, direction, BULLET_SPEED, Colour, HostileToPlayer);
            bullet.position *= 5;
            bullet.position += Boat.position;
            Bullets.Add(bullet);
        }

        public virtual void Update(GameTime gameTime, Islander.GameState gameState)
        {
            if (gameState == Islander.GameState.RunningGame)
            {
                Boat.Update(gameTime);
                Island.Update(gameTime);
                /*foreach (Bullet bullet in Bullets)
                {
                    

                }*/

                for (int i = Bullets.Count-1; i >= 0; i--)
                {
                    Bullets[i].Update(gameTime);
                    if (Bullets[i].done)
                        RemoveBullet(Bullets[i]);
                }
            }
        }
    }
}
