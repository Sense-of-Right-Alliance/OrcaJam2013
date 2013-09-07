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
            //gameState = Islander.GameState.RunningGame;
      
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
                switch (player.Colour)
                {
                    //TODO: Place boats and cities where they belong
                    
                    case (Colour.Blue):
                        player.Island.position.X = width / 4;
                        player.Island.position.Y = height / 2;
                        break;
                    case (Colour.Green):
                        player.Island.position.X = width / 2;
                        player.Island.position.Y = 3 * height / 4;
                        break;
                    case (Colour.Red):
                        player.Island.position.X = 3 * width / 4;
                        player.Island.position.Y = height / 2;
                        break;
                    case (Colour.Yellow):
                        player.Island.position.X = width / 2;
                        player.Island.position.Y = width / 4;
                        break;
                }
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
        }
    }
}
