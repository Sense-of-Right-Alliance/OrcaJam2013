using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Islander.Screen
{
    class MainMenuScreen : MenuScreen
    {
        public Song menuMusic;
        public MainMenuScreen(Islander.GameState gameState)
        {
            GameState = gameState;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            menuMusic = content.Load<Song>("Music/MainMusic");
        }

        public override void Draw(GameTime gameTime, GraphicsDevice GraphicsDevice)
        {
            base.Draw(gameTime, GraphicsDevice);
            DrawString("Start");
        }
    }
}
