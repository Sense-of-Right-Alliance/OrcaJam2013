using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Islander.Screen
{
    /*THIS IS BASE SCREEN*/
    class BaseScreen
    {
        public enum ScreenState
        {
            Uninitialized,
            Initialized,
            Running,
            Finished
        }
        public ScreenState CurrentState { get; protected set; }
        public Player[] PlayersByColour { get; set; }
        public Islander.GameState GameState { get; protected set; }

        protected Texture2D background;
        protected ContentManager content;
        protected SpriteBatch spriteBatch;
        protected SpriteFont scoreFont;
        public int width { get; protected set; }
        public int height { get; protected set; }
        protected List<Player> players;
        protected TimeSpan timeElapsed;

        public BaseScreen()
        {
            CurrentState = ScreenState.Uninitialized;
        }

        public void Initialize(ContentManager content, SpriteBatch spriteBatch, int width, int height, List<Player> players)
        {
            this.content = content;
            this.spriteBatch = spriteBatch;
            this.width = width;
            this.height = height;
            this.players = players;

            PlayersByColour = null;

            LoadContent();
        }

        // informs the screen that it is about to be displayed
        public virtual void StartRunning()
        {
            timeElapsed = TimeSpan.Zero;
        }

        public void Reset()
        {
            UnloadContent();
            LoadContent();
        }

        protected virtual void LoadContent()
        {
            CurrentState = ScreenState.Initialized;
            scoreFont = content.Load<SpriteFont>("Arial");
        }

        protected virtual void UnloadContent()
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            timeElapsed += gameTime.ElapsedGameTime;

            if (timeElapsed.TotalSeconds > 0.25) // don't respond to input for first quarter second after creation
                HandleInput(gameTime);

            // pass Update to players
            foreach (var player in players)
                player.Update(gameTime, GameState);
        }

        public virtual void Draw(GameTime gameTime, GraphicsDevice GraphicsDevice)


        {
            if (background != null)
                spriteBatch.Draw(background, new Rectangle(0, 0, width, height), Color.White);
        }

        protected virtual void HandleInput(GameTime gameTime)
        {
            // handle each player's input
            foreach (var player in players)
            {
                player.HandleInput(GameState, gameTime);
                
                // check if the player has any messages to pass on
                switch (player.Message)
                {
                    case Player.InputMessage.SkipToNextScreen:
                        CurrentState = ScreenState.Finished;
                        break;
                }
            }
        }
    }
}
