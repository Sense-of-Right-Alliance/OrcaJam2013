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
        private enum MainState
        {
            start,
            how
        }

        public Song menuMusic;

        private MainState state;
        private Texture2D introTexture;
        private Texture2D howTexture;

        public MainMenuScreen(Islander.GameState gameState)
        {
            GameState = gameState;
            state = MainState.start;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            menuMusic = content.Load<Song>("Music/MainMusic");

            introTexture = content.Load<Texture2D>("Splash Screens/SplashIntro");
            howTexture = content.Load<Texture2D>("Splash Screens/SplashHowto");

            background = introTexture;
        }

        protected override void HandleInput(GameTime gameTime)
        {
            // handle each player's input
            foreach (var player in players)
            {
                player.HandleInput(GameState, gameTime);

                // check if the player has any messages to pass on
                switch (player.Message)
                {
                    case Player.InputMessage.SkipToNextScreen:
                        if (state == MainState.start)
                        {
                            background = howTexture;
                            state = MainState.how;
                            timeElapsed = TimeSpan.Zero;
                        }
                        else if (state == MainState.how)
                        {
                            CurrentState = ScreenState.Finished;
                        }
                        break;
                }
            }
        }

        public override void Draw(GameTime gameTime, GraphicsDevice GraphicsDevice)
        {
            base.Draw(gameTime, GraphicsDevice);
        }
    }
}
