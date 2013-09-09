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
    using Screen;

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Islander : Microsoft.Xna.Framework.Game
    {
        public enum GameState
        {
            Uninitialized,
            OnMainMenu,
            RunningGame,
            OnGameOver
        }
        GameState currentState;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<Player> players;

        BaseScreen currentScreen;
        MainMenuScreen mainMenuScreen;
        MainGameScreen mainGameScreen;
        GameOverScreen gameOverScreen;
        Dictionary<GameState, BaseScreen> screens;
        
        public Islander()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 768;
            graphics.IsFullScreen = true;

            Content.RootDirectory = "Content";
            SetGameState(GameState.Uninitialized);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // collection of players
            players = new List<Player>()
            {
                new Player(PlayerIndex.One, Content),
                new Player(PlayerIndex.Two, Content),
                new Player(PlayerIndex.Three, Content),
                new Player(PlayerIndex.Four, Content)
            };

            // collection of screens
            mainMenuScreen = new MainMenuScreen(GameState.OnMainMenu);
            mainGameScreen = new MainGameScreen(GameState.RunningGame);
            gameOverScreen = new GameOverScreen(GameState.OnGameOver);
            screens = new Dictionary<GameState, BaseScreen>()
            {
                {mainMenuScreen.GameState, mainMenuScreen},
                {mainGameScreen.GameState, mainGameScreen},
                {gameOverScreen.GameState, gameOverScreen}
            };
            
            base.Initialize();
            //CREATE THE MEDIA PLAYER
            MediaPlayer.Volume = 1.0f;
            //MediaPlayer.Play(mainMenuScreen.menuMusic);
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
            foreach (BaseScreen screen in screens.Values)
                screen.Initialize(Content, spriteBatch, Window.ClientBounds.Width, Window.ClientBounds.Height, players);

            // TODO: use this.Content to load your game content here
            SetGameState(GameState.OnMainMenu);
            currentScreen.StartRunning();
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

            currentScreen.Update(gameTime);

            CheckForScreenChanges();

            base.Update(gameTime);
        }

        // checks if the screen's state has changed
        protected void CheckForScreenChanges()
        {
            // process screen changes
            if (currentScreen.CurrentState == BaseScreen.ScreenState.Finished)
            {
                currentScreen.Reset();

                if (currentScreen == mainMenuScreen)
                {
                    //MediaPlayer.Stop();
                    //MediaPlayer.Play(mainMenuScreen.menuMusic);   SELECT THE SONG TO PLAY FOR GAMEPLAY
                    SetGameState(GameState.RunningGame);
                }
                else if (currentScreen == mainGameScreen)
                {
                    //MediaPlayer.Stop();
                    SetGameState(GameState.OnGameOver);
                }
                else if (currentScreen == gameOverScreen)
                {
                    //MediaPlayer.Play(mainMenuScreen.menuMusic);
                    SetGameState(GameState.OnMainMenu);
                }
            }
        }

        // changes between game states
        protected void SetGameState(GameState gameState)
        {
            if (gameState == GameState.Uninitialized)
                currentScreen = null;
            else
                currentScreen = screens[gameState];

            if (gameState == GameState.RunningGame)
            {
                AssignRandomColoursToPlayers();
            }

            this.currentState = gameState;

            if (currentScreen != null)
                currentScreen.StartRunning();
        }

        protected void AssignRandomColoursToPlayers()
        {
            // generate a random ordering of the players
            var rng = new Random();
            var ordering = Enumerable.Range(0, players.Count).OrderBy(x => rng.Next()).ToList();

            // create array to index players by their assigned colours
            Player[] playersByColour = new Player[players.Count];

            // assign player colours according to ordering
            int index = 0;
            foreach (var order in ordering)
            {
                playersByColour[order] = players[index++];
                playersByColour[order].SetGameColour((Colour)order);
            }

            // provide each player and the game screen with a means to reference the other players by colour
            foreach (var player in players)
                player.PlayersByColour = playersByColour;
            currentScreen.PlayersByColour = playersByColour;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            currentScreen.Draw(gameTime, GraphicsDevice);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
