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

        private const float BULLET_SPEED = 5;

        public Colour Colour { get; protected set; }
        public PlayerIndex PlayerIndex { get; protected set; }

        public InputMessage Message { get; protected set; }

        public Boat Boat { get; protected set; }
        public Island Island { get; protected set; }
        public List<Bullet> Bullets { get; set; }
        Texture2D bulletSprite;
        Texture2D tridentSprite;
        Texture2D bubbleSprite;
        Texture2D razorSprite;
        Texture2D magnetSprite;
        SoundEffect basicAttackSound;

        public Bullet.BulletType bulletType = Bullet.BulletType.Normal;

        public Player[] PlayersByColour { get; set; }
        public bool[] HostileToPlayer { get; protected set; }
        public Resource[] CollectedResources { get; protected set; }

        protected ContentManager content;
        protected TimeSpan bulletDelay;
        protected TimeSpan bulletTimeElapsed;

        public int Score { get; set; }
        public int screenWidth { get; set; }
        public int screenHeight { get; set; }

        public Player(PlayerIndex playerIndex, ContentManager content)
        {
            PlayerIndex = playerIndex;
            this.content = content;
            this.Score = 0;
            
            PlayersByColour = null;
            HostileToPlayer = null;
        }

        // returns name to display on scoreboard
        public string ScoreName
        {
            get
            {
                if (Island == null)
                    return "Player " + PlayerIndex;
                else
                    return "P" + ((int)PlayerIndex + 1) + " " + Island.IslandName + " Island";
            }
        }

        public void SetGameColour(Colour colour)
        {
            Colour = colour;
            LoadBullets();
            Boat = Boat.InitializeFromColour(colour, content,screenWidth,screenHeight);
            Island = Island.InitializeFromColour(colour, content);

            // initially hostile to all players except self
            HostileToPlayer = new bool[] { true, true, true, true };
            HostileToPlayer[(int)Colour] = false;

            // default bullet delay
            bulletDelay = new TimeSpan(8000000); // 0.5s
            bulletTimeElapsed = TimeSpan.Zero;

            bulletType = Bullet.BulletType.Normal;

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

            tridentSprite = content.Load<Texture2D>("Bullets/RedTrident");
            bubbleSprite = content.Load<Texture2D>("Bullets/BlueBubble");
            razorSprite = content.Load<Texture2D>("Bullets/YellowRazor");
            magnetSprite = content.Load<Texture2D>("Bullets/GreenMagnet");

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
            Score += RETURN_RESOURCE;


            switch (resource.IslandType)
            {
                case(Island.IslandType.Trident):
                    bulletType = Bullet.BulletType.Trident;
                    break;
                case (Island.IslandType.Bubble):
                    bulletType = Bullet.BulletType.Bubble;
                    break;
                case (Island.IslandType.Razor):
                    bulletType = Bullet.BulletType.Razor;
                    break;
                case (Island.IslandType.Magnet):
                    bulletType = Bullet.BulletType.Magnet;
                    break;
            }
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
            if (Boat.State == Boat.BoatState.Alive && direction.Length() >= 0.5f)
            {
                if (bulletTimeElapsed > bulletDelay)
                {
                    direction.Y = -direction.Y;
                    ShootBullet(direction);
                }
            }
        }

        private void ShootBullet(Vector2 direction)
        {
            // Bullet shooting
            basicAttackSound.Play();
            bulletTimeElapsed = TimeSpan.Zero;
            direction.Normalize();

            Bullet[] bullet = new Bullet[0];
            switch (bulletType)
            {
                case(Bullet.BulletType.Normal):
                    bullet = Bullet.CreateBullet(bulletSprite, bulletType, direction, Colour, HostileToPlayer, this);
                    break;
                case(Bullet.BulletType.Trident):
                    bullet = Bullet.CreateBullet(tridentSprite, bulletType, direction, Colour, HostileToPlayer, this);
                    break;
                case (Bullet.BulletType.Bubble):
                    bullet = Bullet.CreateBullet(bubbleSprite, bulletType, direction, Colour, HostileToPlayer, this);
                    break;
                case (Bullet.BulletType.Razor):
                    bullet = Bullet.CreateBullet(razorSprite, bulletType, direction, Colour, HostileToPlayer, this);
                    break;
                case (Bullet.BulletType.Magnet):
                    bullet = Bullet.CreateBullet(magnetSprite, bulletType, direction, Colour, HostileToPlayer, this);
                    break;
            }

            for(int i = 0; i < bullet.Length; i++)
            {
                bullet[i].Position *= 5;
                bullet[i].Position += Boat.Position;
                Bullets.Add(bullet[i]);
            }
        }

        public virtual void Update(GameTime gameTime, Islander.GameState gameState)
        {
            if (gameState == Islander.GameState.RunningGame)
            {
                Boat.Update(gameTime);
                Island.Update(gameTime);

                // update bullets and remove any that have expired
                var expiredBullets = new List<Bullet>();
                foreach (var bullet in Bullets)
                {
                    bullet.Update(gameTime);
                    if (bullet.Expired) expiredBullets.Add(bullet);
                }
                foreach (var bullet in expiredBullets)
                    RemoveBullet(bullet);
            }
        }
    }
}
