using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Islander.Screen
{
    class GameOverScreen : MenuScreen
    {
        public GameOverScreen()
        {
            gameState = Islander.GameState.OnGameOver;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice GraphicsDevice)
        {
            base.Draw(gameTime, GraphicsDevice);

            DrawString("Game Over");
        }
    }
}
