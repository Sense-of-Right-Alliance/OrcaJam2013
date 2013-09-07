using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Islander.Screen
{
    class MainGameScreen : BaseScreen
    {
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
        
        public void StartGame()
        {
            /*Function that sets up the game, including positioning all initial boats and islands*/
            PositionPlayers();
        }

        private void PositionPlayers()
        {
            foreach (var player in players)
            {
                Vector2 islandVector = new Vector2(0.0f,0.0f);
                
                switch (player.Colour)
                {
                    //Places boats and cities in positions matching an xbox controller
                    
                    case (Colour.Blue):
                        islandVector.X = width / 4;
                        islandVector.Y = height / 2;
                        player.Boat.position.X = islandVector.X + 75;
                        break;
                    case (Colour.Green):
                        islandVector.X = width / 2;
                        islandVector.Y = 3 * height / 4;
                        player.Boat.position.Y = islandVector.Y - 75;
                        break;
                    case (Colour.Red):
                        islandVector.X = 3 * width / 4;
                        islandVector.Y = height / 2;
                        player.Boat.position.X = islandVector.X - 75;
                        break;
                    case (Colour.Yellow):
                        islandVector.X = width / 2;
                        islandVector.Y = height / 4;
                        player.Boat.position.Y = islandVector.Y + 75;
                        break;
                }
                 player.Island.position = islandVector;
                 if (player.Boat.position.X == 0)
                     player.Boat.position.X = islandVector.X;
                 else if (player.Boat.position.Y == 0)
                     player.Boat.position.Y = islandVector.Y;
            }
        }

        public override void Update(GameTime gameTime)
        {
            timeElapsed += gameTime.ElapsedGameTime;
            if (timeElapsed.TotalSeconds > 0.25)
                base.HandleInput();

            // pass Update to players
            foreach (var player in players)
                player.Update(gameTime, gameState);
        }

        public override void Draw(GameTime gameTime, GraphicsDevice GraphicsDevice)
        {
            GraphicsDevice.Clear(Color.Wheat);

            base.Draw(gameTime, GraphicsDevice);

            /*foreach (var player in players)
            {
                player.Draw(gameTime,gameState,spriteBatch);
            }*/
           
            
        }
    }
}
