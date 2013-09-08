using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Islander.Screen
{
    using Entity;

    class MainGameScreen : BaseScreen
    {
        protected List<Boat> boats;
        protected List<Island> islands;
        protected List<Resource> droppedResources;
        protected List<List<Bullet>> bulletLists;

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
                player.Update(gameTime, GameState);
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
                foreach (var island in islands)
                    if (boat.CollidesWith(island))
                        BoatIslandCollision(boat, island);
                foreach (var resource in droppedResources)
                    if (boat.CollidesWith(resource))
                        BoatResourceCollision(boat, resource);
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
            // TODO
        }

        protected void BulletBoatCollision(Bullet bullet, Boat boat, List<Bullet> removedBullets)
        {
            // TODO
            if (bullet.HostileToPlayer[(int)boat.Colour])
                removedBullets.Add(bullet);
        }

        public override void Draw(GameTime gameTime, GraphicsDevice GraphicsDevice)
        {
            GraphicsDevice.Clear(Color.Wheat);

            base.Draw(gameTime, GraphicsDevice);

            foreach (var player in players)
            {
                player.Island.Draw(spriteBatch);
                player.Boat.Draw(spriteBatch);
                //DrawBullets tells the player to 
                foreach (var bullet in player.Bullets)
                {
                    bullet.Draw(spriteBatch);
                }
            }
        }
    }
}
