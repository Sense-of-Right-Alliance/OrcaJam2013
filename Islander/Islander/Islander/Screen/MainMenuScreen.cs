﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Islander.Screen
{
    class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen()
        {
            gameState = Islander.GameState.OnMainMenu;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice GraphicsDevice)
        {
            base.Draw(gameTime, GraphicsDevice);

            DrawString("Start");
        }
    }
}
