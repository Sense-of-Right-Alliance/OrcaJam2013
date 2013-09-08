using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Islander.Entity;

namespace Islander.Screen
{
    using Entity;

    class MainGameScreen : BaseScreen
    {
        /*CONSTANTS FOR SCORE*/
        private const int RETURN_RESOURCE = 1000;

        protected List<Boat> boats;
        protected List<Island> islands;
        protected List<Resource> droppedResources;
        protected List<List<Bullet>> bulletLists;

        //Do we need all these vector2's? I didn't want to calculate the pos greenScoreLabel = new Vector2(blueScoreLabel.X, blueScoreLabel.Y + 50) EVERY update loop.
        private Vector2 blueScoreLabelPos;
        private Vector2 greenScoreLabelPos;
        private Vector2 redScoreLabelPos;
        private Vector2 yellowScoreLabelPos;
        private Vector2 blueScorePos;
        private Vector2 greenScorePos;
        private Vector2 redScorePos;
        private Vector2 yellowScorePos;

        public MainGameScreen(Islander.GameState gameState)
        {
            GameState = gameState;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            background = content.Load<Texture2D>("Background");
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
                        islandVector.X = width / 8;
                        islandVector.Y = height / 2;
                        player.Boat.position.X += 75;
                        break;
                    case (Colour.Green):
                        islandVector.X = width / 2;
                        islandVector.Y = 3 * height / 4;
                        player.Boat.position.Y -= 75;
                        break;
                    case (Colour.Red):
                        islandVector.X = 7 * width / 8;
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
            }
        }

        public override void Update(GameTime gameTime)
        {
            timeElapsed += gameTime.ElapsedGameTime;
            if (timeElapsed.TotalSeconds > 0.25)
                base.HandleInput(gameTime);

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
                    player.Boat.WaitForRespawn(player.Island.position);
                    if (player.Boat.CarriedResource != null)
                    {
                        droppedResources.Add(player.Boat.CarriedResource);
                        player.Boat.CarriedResource = null;
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
            foreach (var boat in boats)
            {
                if (boat.state == Boat.BoatState.alive)
                {
                    foreach (var island in islands)
                        if (boat.CollidesWith(island))
                            BoatIslandCollision(boat, island);
                    for (int i = droppedResources.Count - 1; i >= 0; i--)
                    {
                        if (boat.CollidesWith(droppedResources[i]))
                            BoatResourceCollision(boat, droppedResources[i]);
                    }

                }
            }
        }

        // check each bullet for collisions with boats and leaving the game screen
        protected void CheckBulletCollisions()
        {
            var removedBullets = new List<Bullet>();

            foreach (var bulletList in bulletLists)
            {
                foreach (var bullet in bulletList)
                    foreach (var boat in boats)
                        if (bullet.HostileToPlayer[(int)boat.Colour])
                            if (bullet.CollidesWith(boat))
                                BulletBoatCollision(bullet, boat, removedBullets);

                    // TODO: check if bullet has left game screen
            }

            // remove all bullets that were destroyed
            foreach (var bullet in removedBullets)
                PlayersByColour[(int)bullet.Colour].RemoveBullet(bullet);
        }

        protected void BoatIslandCollision(Boat boat, Island island)
        {
            if (boat.Colour == island.Colour)
            {
                if (boat.CarriedResource != null) // if carrying a resource
                {
                    players[(int)boat.Colour].CollectResource(boat.CarriedResource);
                    
                    if(boat.CarriedResource.Colour != boat.Colour)
                        players[(int)boat.Colour].score += RETURN_RESOURCE;

                    boat.CarriedResource = null;
                }
            }
            else
            {
                if (boat.CarriedResource == null) // if not carrying a resource
                {
                    boat.CarriedResource = new Resource(island.ResourceType);
                    boat.CarriedResource.IsCarried = true;
                }
            }
        }

        protected void BoatResourceCollision(Boat boat, Resource resource)
        {
            if (boat.CarriedResource == null) // if not carrying a resource
            {
                boat.CarriedResource = resource;
                boat.CarriedResource.IsCarried = true;
                droppedResources.Remove(resource);
            }
        }

        protected void BulletBoatCollision(Bullet bullet, Boat boat, List<Bullet> removedBullets)
        {
            // TODO
            if (bullet.HostileToPlayer[(int)boat.Colour])
            {
                removedBullets.Add(bullet);
                boat.Hit();
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
                updateScore(player.Colour, player.score, player.PlayerIndex);
            }

            foreach (var resource in droppedResources)
            {
                resource.Draw(spriteBatch);
            }
        }
        private void updateScore(Colour playerColour, int playerScore, PlayerIndex playerIndex)
        {
            /*TODO: Put the players score in it's corresponding textbox.*/
            switch (playerColour)
            {
                case (Colour.Blue):
                    outlineFont(playerScore, playerIndex, blueScoreLabelPos, blueScorePos);
                    spriteBatch.DrawString(scoreFont, "Player " + playerIndex + ":", blueScoreLabelPos, Color.Blue);
                    spriteBatch.DrawString(scoreFont, "" + playerScore, blueScorePos, Color.Blue);
                    break;
                case (Colour.Green):
                    outlineFont(playerScore, playerIndex, greenScoreLabelPos, greenScorePos);
                    spriteBatch.DrawString(scoreFont, "Player " + playerIndex + ":", greenScoreLabelPos, Color.Green);
                    spriteBatch.DrawString(scoreFont, "" + playerScore, greenScorePos, Color.Green);
                    break;
                case (Colour.Red):
                    outlineFont(playerScore, playerIndex, redScoreLabelPos, redScorePos);
                    spriteBatch.DrawString(scoreFont, "Player " + playerIndex + ":", redScoreLabelPos, Color.Red);
                    spriteBatch.DrawString(scoreFont, "" + playerScore, redScorePos, Color.Red);
                    break;
                case (Colour.Yellow):
                    outlineFont(playerScore, playerIndex, yellowScoreLabelPos, yellowScorePos);
                    spriteBatch.DrawString(scoreFont, "Player " + playerIndex + ":", yellowScoreLabelPos, Color.Orange);
                    spriteBatch.DrawString(scoreFont, "" + playerScore, yellowScorePos, Color.Orange);
                    break;
            }
        }
        /*Don't look at this method. Seriously just don't. Ignore the fact that it's run 4 times every frame. Don't even worry about it.
         * Alternatively, make it more general and give it to the other menu screens as well. Your call.
         * Fucking thanks yellow, you bastard.
         */
        private void outlineFont(int playerScore, PlayerIndex playerIndex, Vector2 scoreLabelPos, Vector2 scorePos)
        {
            spriteBatch.DrawString(scoreFont, "Player " + playerIndex + ":", new Vector2(scoreLabelPos.X+1,scoreLabelPos.Y), Color.Black);
            spriteBatch.DrawString(scoreFont, "" + playerScore, new Vector2(scorePos.X+1,scorePos.Y), Color.Black);
            spriteBatch.DrawString(scoreFont, "Player " + playerIndex + ":", new Vector2(scoreLabelPos.X - 1, scoreLabelPos.Y), Color.Black);
            spriteBatch.DrawString(scoreFont, "" + playerScore, new Vector2(scorePos.X - 1, scorePos.Y), Color.Black);
            spriteBatch.DrawString(scoreFont, "Player " + playerIndex + ":", new Vector2(scoreLabelPos.X, scoreLabelPos.Y + 1), Color.Black);
            spriteBatch.DrawString(scoreFont, "" + playerScore, new Vector2(scorePos.X, scorePos.Y + 1), Color.Black);
            spriteBatch.DrawString(scoreFont, "Player " + playerIndex + ":", new Vector2(scoreLabelPos.X, scoreLabelPos.Y - 1), Color.Black);
            spriteBatch.DrawString(scoreFont, "" + playerScore, new Vector2(scorePos.X, scorePos.Y - 1), Color.Black);
        }
    }
}
