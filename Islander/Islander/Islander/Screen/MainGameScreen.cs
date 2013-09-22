using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Islander.Screen
{
    using Entity;

    class MainGameScreen : BaseScreen
    {
        protected List<Boat> boats;
        protected List<Island> islands;
        protected List<Resource> droppedResources;
        protected List<List<Bullet>> bulletLists;
        protected List<PowerUp> powerUps;

        private float powerUpTimer = 0.0f;
        private Texture2D speedPowerUp;

        //Do we need all these vector2's? I didn't want to calculate the pos greenScoreLabel = new Vector2(blueScoreLabel.X, blueScoreLabel.Y + 50) EVERY update loop.
        private Vector2 blueScoreLabelPos;
        private Vector2 greenScoreLabelPos;
        private Vector2 redScoreLabelPos;
        private Vector2 yellowScoreLabelPos;
        private Vector2 blueScorePos;
        private Vector2 greenScorePos;
        private Vector2 redScorePos;
        private Vector2 yellowScorePos;

        private const float GAME_TIME = 180.0f;
        private float gameTimer = 180.0f;

        public Song gameMusic;

        private SoundEffect takeCargo;
        private SoundEffect scoreCargo;
        private SoundEffect impactSound;
        private SoundEffect dieSound;


        public const int RETURN_RESOURCE = 50;


        public MainGameScreen(Islander.GameState gameState)
        {
            GameState = gameState;
        }

        protected override void LoadContent()
        {
            SoundEffect.MasterVolume = 0.5f;
            base.LoadContent();

            gameMusic = content.Load<Song>("Music/The Zandali");

            background = content.Load<Texture2D>("Background");
            takeCargo = content.Load<SoundEffect>("SFX/Take Cargo");
            scoreCargo = content.Load<SoundEffect>("SFX/Score Cargo2");
            impactSound = content.Load<SoundEffect>("SFX/Impact");
            dieSound = content.Load<SoundEffect>("SFX/Die");

            speedPowerUp = content.Load<Texture2D>("Cargo/BlueCargo");
        }

        // Function that sets up the game, including positioning all initial boats and islands
        public override void StartRunning()
        {
            base.StartRunning();
            PositionPlayers();

            // initialize collections of boats, islands, bullets
            boats = new List<Boat>();
            islands = new List<Island>();
            bulletLists = new List<List<Bullet>>();
            droppedResources = new List<Resource>();
            powerUps = new List<PowerUp>();
            foreach (var player in players)
            {
                boats.Add(player.Boat);
                islands.Add(player.Island);
                bulletLists.Add(player.Bullets);
            }

            //Initialize UI elements.
            blueScoreLabelPos.Y = (13 * height / 16) + 2; //MAGIC NUMBERS
            blueScoreLabelPos.X = 4 * width / 11;
            blueScorePos.X = blueScoreLabelPos.X + width/4;
            blueScorePos.Y = blueScoreLabelPos.Y;
            yellowScoreLabelPos.Y = blueScoreLabelPos.Y + (height / 20) - 2; //MAGIC NUMBERS
            yellowScoreLabelPos.X = blueScoreLabelPos.X;
            yellowScorePos.X = blueScoreLabelPos.X + width / 4;
            yellowScorePos.Y = yellowScoreLabelPos.Y;
            redScoreLabelPos.Y = blueScoreLabelPos.Y + (height / 10) - 3; //MAGIC NUMBERS
            redScoreLabelPos.X = blueScoreLabelPos.X;
            redScorePos.X = blueScoreLabelPos.X + width / 4;
            redScorePos.Y = redScoreLabelPos.Y;
            greenScoreLabelPos.Y = blueScoreLabelPos.Y + (height / 7); //GREEN LABEL
            greenScoreLabelPos.X = blueScoreLabelPos.X;
            greenScorePos.X = blueScoreLabelPos.X + width / 4;
            greenScorePos.Y = greenScoreLabelPos.Y;


        }

        private void PositionPlayers()
        {
            foreach (var player in players)
            {
                Vector2 islandVector = new Vector2(0.0f, 0.0f);
                player.Boat.position = new Vector2(0.0f, 0.0f);
                
                switch (player.Colour)
                {
                    // Place boats and cities in positions matching an xbox controller
                    case (Colour.Blue):
                        islandVector.X = width / 4;
                        islandVector.Y = height / 2;
                        player.Boat.position.X += 75;
                        break;
                    case (Colour.Green):
                        islandVector.X = width / 2;
                        islandVector.Y = 3 * height / 4;
                        player.Boat.position.Y -= 75;
                        break;
                    case (Colour.Red):
                        islandVector.X = 3 * width / 4;
                        islandVector.Y = height / 2;
                        player.Boat.position.X -= 75;
                        break;
                    case (Colour.Yellow):
                        islandVector.X = width / 2;
                        islandVector.Y = height / 4;
                        player.Boat.position.Y += 75;
                        break;
                }
                islandVector.Y -= 70;
                player.Island.position = islandVector;
                player.Boat.position = islandVector;
                player.Boat.Start();
                player.Island.StartIsland();
            }
        }

        protected override void HandleInput(GameTime gameTime)
        {
            base.HandleInput(gameTime);
            // handle each player's input
            /*foreach (var player in players)
            {
                player.HandleInput(GameState, gameTime);
            }*/
        }

        public override void Update(GameTime gameTime)
        {
            timeElapsed += gameTime.ElapsedGameTime;
            if (timeElapsed.TotalSeconds > 0.25)
                HandleInput(gameTime);

            gameTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (gameTimer <= 0)
            {
                //TODO: end game
                gameTimer = GAME_TIME;
                CurrentState = ScreenState.Finished;
            }

            powerUpTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (powerUpTimer >= 30.0f)
            {
                powerUpTimer = 0.0f;
                powerUps.Add(new PowerUp(speedPowerUp, new Vector2(width/2, 3*height/7)));
            }

            foreach (var powerUp in powerUps)
            {
                powerUp.Update(gameTime);
            }

            // check if player has collected all resources
            foreach (Player p in players)
            {
                if (p.Island.HasAllResources)
                {
                    gameTimer = GAME_TIME;
                    CurrentState = ScreenState.Finished;
                }
            }

            CheckCollisions();

            // pass Update to players
            foreach (var player in players)
            {
                /*This maybe should only be updated from Player? That would mean that it would only refresh 
                 * when the player increases or decreases their score, as opposed to every update.
                 * On the flip side, that would mean that Player would be in control of UI elements, which really should be in
                 * their own class. /shrug */
                player.Update(gameTime, GameState);

                if (player.Boat.state == Boat.BoatState.dead)
                {
                    dieSound.Play();
                    player.Boat.WaitForRespawn(player.Island.position);
                    if (player.Boat.carriedResources.Count > 0)
                    {
                        foreach (Resource r in player.Boat.carriedResources)
                        {
                            droppedResources.Add(r);
                        }
                        player.Boat.carriedResources.Clear();
                    }
                }
            }

            foreach (var resource in droppedResources)
            {
                resource.Update(gameTime);
            }
        }

        // checks entities for collisions with other entities
        protected void CheckCollisions()
        {
            CheckBoatCollisions();
            CheckBulletCollisions();
        }

        // check each boat for collisions with islands and resources
        protected void CheckBoatCollisions()
        {
            var collectedResources = new List<Resource>();
            var collectedPowerUps = new List<PowerUp>();

            foreach (var boat in boats)
            {
                if (boat.state == Boat.BoatState.alive)
                {
                    foreach (var island in islands)
                        if (boat.CollidesWith(island))
                            BoatIslandCollision(boat, island);
                    foreach (var resource in droppedResources)
                        if (boat.CollidesWith(resource))
                            BoatResourceCollision(boat, resource, collectedResources);
                    foreach (var powerUp in powerUps)
                        if (boat.CollidesWith(powerUp))
                            BoatPowerUpCollision(boat, powerUp, collectedPowerUps);
                }
            }

            // remove all resources that were retrieved
            foreach (var resource in collectedResources)
                droppedResources.Remove(resource);
            // remove all powerups that were retrieved
            foreach (var powerUp in collectedPowerUps)
                powerUps.Remove(powerUp);
        }

        protected void BoatPowerUpCollision(Boat boat, PowerUp powerUp, List<PowerUp> collectedPowerUps)
        {
            boat.GainPowerUp(powerUp);
            collectedPowerUps.Add(powerUp);
        }

        // check each bullet for collisions with boats and leaving the game screen
        protected void CheckBulletCollisions()
        {
            var removedBullets = new List<Bullet>();

            foreach (var bulletList in bulletLists)
            {
                foreach (var bullet in bulletList)
                    foreach (var boat in boats)
                        if (boat.state == Boat.BoatState.alive)
                        {
                            if (bullet.HostileToPlayer[(int)boat.Colour])
                                if (bullet.CollidesWith(boat))
                                    BulletBoatCollision(bullet, boat, removedBullets);
                        }
            }

            // remove all bullets that were destroyed
            foreach (var bullet in removedBullets)
                PlayersByColour[(int)bullet.Colour].RemoveBullet(bullet);
        }

        protected void BoatIslandCollision(Boat boat, Island island)
        {
            if (boat.Colour == island.Colour)
            {
                if (boat.carriedResources.Count > 0)//(boat.CarriedResource != null) // if carrying a resource
                {
                    //Plays the collect resource sound. This should maybe be in the CollectResource method
                    scoreCargo.Play();

                    foreach (Resource r in boat.carriedResources)
                    {
                        PlayersByColour[(int)boat.Colour].CollectResource(r);
                        island.AddResource(r);
                    }
                    //PlayersByColour[(int)boat.Colour].CollectResource(boat.CarriedResource);
                    //island.AddResource(boat.CarriedResource);


                    
                   // if(boat.CarriedResource.Colour != boat.Colour)
                     //   PlayersByColour[(int)boat.Colour].score += RETURN_RESOURCE;

                    //boat.CarriedResource = null;
                    boat.carriedResources.Clear();
                }
            }
            else
            {
                if(!boat.CheckResourceIsCarried(island.ResourceType.islandType))//!boat.carriedResources.Contains(island.ResourceType))//if (boat.CarriedResource == null) // if not carrying a resource
                {
                    //PLAY THE SOUND
                    takeCargo.Play();
                    PlayersByColour[(int)island.Colour].score -= 200;
                    Resource r = new Resource(island.ResourceType);
                    boat.carriedResources.Add(r);
                    r.IsCarried = true;
                    r.position = boat.position;
                    //boat.CarriedResource = new Resource(island.ResourceType);
                    //boat.CarriedResource.IsCarried = true;
                }
            }
        }

        protected void BoatResourceCollision(Boat boat, Resource resource, List<Resource> collectedResources)
        {
            if (boat.Colour == resource.Colour)
            {
                takeCargo.Play(); // should be returnCargo
                PlayersByColour[(int)boat.Colour].score += 200;
                collectedResources.Add(resource);
            }
            else
            {
                //if (boat.CarriedResource == null) // if not carrying a resource
                //{
                    //PLAY THE SOUND
                    takeCargo.Play();
                    boat.carriedResources.Add(resource);
                    resource.IsCarried = true;
                    collectedResources.Add(resource);

                    //boat.CarriedResource = resource;
                    //boat.CarriedResource.IsCarried = true;
                    //collectedResources.Add(resource);
                //}
            }
        }

        protected void BulletBoatCollision(Bullet bullet, Boat boat, List<Bullet> removedBullets)
        {
            // TODO
            if (bullet.HostileToPlayer[(int)boat.Colour])
            {
                //Plays the sound!
                impactSound.Play();
                removedBullets.Add(bullet);
                boat.Hit(bullet);
            }
        }



        public override void Draw(GameTime gameTime, GraphicsDevice GraphicsDevice)
        {
            GraphicsDevice.Clear(Color.Wheat);

            base.Draw(gameTime, GraphicsDevice);

            foreach (var player in players)
            {
                player.Island.Draw(spriteBatch);
                player.Boat.Draw(spriteBatch);
                foreach (var bullet in player.Bullets)
                {
                    bullet.Draw(spriteBatch);
                }
                updateScore(player);
            }

            foreach (var resource in droppedResources)
            {
                resource.Draw(spriteBatch);
            }

            foreach (var powerUp in powerUps)
            {
                powerUp.Draw(spriteBatch);
            }

            float t = (float)Math.Floor((double)gameTimer);

            spriteBatch.DrawString(scoreFont, "" + t, new Vector2(width / 2 - scoreFont.MeasureString("" + t).X / 2, height / 60), Color.Black);
        }

        private void updateScore(Player player)
        {
            Colour playerColour = player.Colour;
            int playerScore = player.score;
            string playerName = player.ScoreName;

            switch (playerColour)
            {
                case (Colour.Blue):
                    outlineFont(playerScore, playerName, blueScoreLabelPos, blueScorePos);
                    spriteBatch.DrawString(scoreFont, playerName + ":", blueScoreLabelPos, Color.Blue);
                    spriteBatch.DrawString(scoreFont, "" + playerScore, blueScorePos, Color.Blue);
                    break;
                case (Colour.Green):
                    outlineFont(playerScore, playerName, greenScoreLabelPos, greenScorePos);
                    spriteBatch.DrawString(scoreFont, playerName + ":", greenScoreLabelPos, Color.Green);
                    spriteBatch.DrawString(scoreFont, "" + playerScore, greenScorePos, Color.Green);
                    break;
                case (Colour.Red):
                    outlineFont(playerScore, playerName, redScoreLabelPos, redScorePos);
                    spriteBatch.DrawString(scoreFont, playerName + ":", redScoreLabelPos, Color.Red);
                    spriteBatch.DrawString(scoreFont, "" + playerScore, redScorePos, Color.Red);
                    break;
                case (Colour.Yellow):
                    outlineFont(playerScore, playerName, yellowScoreLabelPos, yellowScorePos);
                    spriteBatch.DrawString(scoreFont, playerName + ":", yellowScoreLabelPos, Color.Orange);
                    spriteBatch.DrawString(scoreFont, "" + playerScore, yellowScorePos, Color.Orange);
                    break;
            }
        }
        /*Don't look at this method. Seriously just don't. Ignore the fact that it's run 4 times every frame. Don't even worry about it.
         * Alternatively, make it more general and give it to the other menu screens as well. Your call.
         * Fucking thanks yellow, you bastard.
         */
        private void outlineFont(int playerScore, string playerName, Vector2 scoreLabelPos, Vector2 scorePos)
        {
            spriteBatch.DrawString(scoreFont, playerName + ":", new Vector2(scoreLabelPos.X + 1, scoreLabelPos.Y), Color.Black);
            spriteBatch.DrawString(scoreFont, "" + playerScore, new Vector2(scorePos.X+1,scorePos.Y), Color.Black);
            spriteBatch.DrawString(scoreFont, playerName + ":", new Vector2(scoreLabelPos.X - 1, scoreLabelPos.Y), Color.Black);
            spriteBatch.DrawString(scoreFont, "" + playerScore, new Vector2(scorePos.X - 1, scorePos.Y), Color.Black);
            spriteBatch.DrawString(scoreFont, playerName + ":", new Vector2(scoreLabelPos.X, scoreLabelPos.Y + 1), Color.Black);
            spriteBatch.DrawString(scoreFont, "" + playerScore, new Vector2(scorePos.X, scorePos.Y + 1), Color.Black);
            spriteBatch.DrawString(scoreFont, playerName + ":", new Vector2(scoreLabelPos.X, scoreLabelPos.Y - 1), Color.Black);
            spriteBatch.DrawString(scoreFont, "" + playerScore, new Vector2(scorePos.X, scorePos.Y - 1), Color.Black);
        }
        /*I told you not to fucking look at it now your eyes are sad.*/
    }
}
