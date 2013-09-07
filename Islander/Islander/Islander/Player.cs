using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Islander
{
    using Entity;

    enum Colour
    {
        Blue,
        Yellow,
        Red,
        Green
    }

    class Player
    {
        public enum InputMessage
        {
            NoMessage,
            SkipToNextScreen
        }

        public Colour Colour { get; protected set; }
        public PlayerIndex PlayerIndex { get; protected set; }

        public InputMessage Message { get; protected set; }

        public Boat Boat { get; protected set; }
        public Island Island { get; protected set; }

        public Player[] Players { get; set; }

        protected ContentManager content;

        public Player(PlayerIndex playerIndex, ContentManager content)
        {
            PlayerIndex = playerIndex;
            this.content = content;
            Players = null;
        }

        public void SetGameColour(Colour colour)
        {
            Colour = colour;
            Boat = Boat.InitializeFromColour(colour, content);
            Island = Island.InitializeFromColour(colour, content);
        }

        public virtual void HandleInput(Islander.GameState gameState)
        {
            // get the keyboard state
            KeyboardState keyboardState = Keyboard.GetState();

            // get the current player's gamepad state
            GamePadState gamePadState = GamePad.GetState(PlayerIndex);

            // if no input detected, there is no message to send
            Message = InputMessage.NoMessage;


            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                Message = InputMessage.SkipToNextScreen;
            }
            else if (gameState == Islander.GameState.RunningGame)
            {
                //Boat.HandleInput(keyboardState);
            }

            if (!gamePadState.IsConnected)
            {
                Debug.WriteLine("Player " + PlayerIndex.ToString() + "'s controller is disconnected.");
                return;
            }
            else
            {
                if (gamePadState.Buttons.Start == ButtonState.Pressed)
                    Message = InputMessage.SkipToNextScreen;
                else if (gameState == Islander.GameState.RunningGame)
                {
                    Boat.HandleInput(gamePadState.ThumbSticks.Left, gamePadState.ThumbSticks.Right);
                }
            }
        }

        public virtual void Update(GameTime gameTime, Islander.GameState gameState)
        {
            if (gameState == Islander.GameState.RunningGame)
            {
                //Boat.Update(gameTime);
                //Island.Update(gameTime);
            }
        }

        /*TODO:Should this need the gamestate? I think the only time draw is getting calle on Players should be explicitly 
         * in MainGameScreen, which is only ever in the running state.
        */
        public virtual void Draw(GameTime gameTime, Islander.GameState gameState, SpriteBatch spriteBatch)
        {
            if (gameState == Islander.GameState.RunningGame)
            {
                //Boat.Draw(spriteBatch);
                //Island.Draw(spriteBatch);
            }
        }
    }
}
