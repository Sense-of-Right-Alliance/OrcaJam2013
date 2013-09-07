using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Islander
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Islander : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<Player> players;

        Screen.BaseScreen currentScreen;

        Screen.GameScreen gameScreen;
        Screen.MainMenuScreen mainMenuScreen;
        Screen.GameOverScreen gameOverScreen;
        List<Screen.BaseScreen> screens;
        
        public Islander()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            players = new List<Player>()
            {
                new Player(PlayerIndex.One),
                new Player(PlayerIndex.Two),
                new Player(PlayerIndex.Three),
                new Player(PlayerIndex.Four)
            };

            gameOverScreen = new Screen.GameOverScreen();
            mainMenuScreen = new Screen.MainMenuScreen();
            gameScreen = new Screen.GameScreen();

            screens = new List<Screen.BaseScreen>()
            {
                gameOverScreen,
                mainMenuScreen,
                gameScreen
            };
           
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // initialize all screens
            foreach (Screen.BaseScreen screen in screens)
                screen.Initialize(Content, spriteBatch, players, Window.ClientBounds.Width, Window.ClientBounds.Height);

            currentScreen = mainMenuScreen;

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            currentScreen.Update();

            CheckForScreenChanges();

            base.Update(gameTime);
        }

        private void CheckForScreenChanges()
        {
            // process screen changes
            if (currentScreen.CurrentState == Screen.State.Finished)
            {
                currentScreen.Reset();

                if (currentScreen == mainMenuScreen)
                    currentScreen = gameScreen;
                else if (currentScreen == gameScreen)
                    currentScreen = gameOverScreen;
                else if (currentScreen == gameOverScreen)
                    currentScreen = mainMenuScreen;

                currentScreen.StartRunning();
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            currentScreen.Draw();

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
