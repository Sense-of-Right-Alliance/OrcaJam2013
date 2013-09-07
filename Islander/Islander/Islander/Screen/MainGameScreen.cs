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
        protected new Dictionary<Colour, List<Bullet>> bulletLists;

        public MainGameScreen()
        {
            /*This line should be unnecessary, because All of the screens are created in Islander.Initialize*/
            gameState = Islander.GameState.RunningGame;
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
            bulletLists = new Dictionary<Colour, List<Bullet>>();
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
                player.Island.position = islandVector;
                player.Boat.position = islandVector;
            }
        }

        public override void Update(GameTime gameTime)
        {
            timeElapsed += gameTime.ElapsedGameTime;
            if (timeElapsed.TotalSeconds > 0.25)
                base.HandleInput();

            CheckCollisions();

            // pass Update to players
            foreach (var player in players)
                player.Update(gameTime, gameState);
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

        // check each bullet for collisions with boats
        protected void CheckBulletCollisions()
        {
            foreach (var colourBulletList in bulletLists)
            {
                //foreach (var boat in boats)
                //    if (colourBulletList.Key
                //    foreach (var colourBulletList in bulletList)
                //        if (PlayersByColour[colourBulletList
                //        if (bullet.CollidesWith(boat))
                //            BulletBoatCollision(bullet, boat);
            }
        }

        protected void BoatIslandCollision(Boat boat, Island island)
        {
            // TODO
        }

        protected void BoatResourceCollision(Boat boat, Resource resource)
        {
            // TODO
        }

        protected void BulletBoatCollision(Bullet bullet, Boat boat)
        {
            // TODO
        }

        public override void Draw(GameTime gameTime, GraphicsDevice GraphicsDevice)
        {
            GraphicsDevice.Clear(Color.Wheat);

            base.Draw(gameTime, GraphicsDevice);

            foreach (var player in players)
            {
                player.Island.Draw(spriteBatch);
                player.Boat.Draw(spriteBatch);
            }
        }
    }
}
