using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Islander.Screen
{
    class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen(Islander.GameState gameState)
        {
            GameState = gameState;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice GraphicsDevice)
        {
            base.Draw(gameTime, GraphicsDevice);

            DrawString("Start");
        }
    }
}
