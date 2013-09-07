using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Islander.Objects
{
    class Player
    {
        public enum Message
        {
            NoMessage,
            Start
        }

        public Island.Colour Colour { get; protected set; }
        public PlayerIndex PlayerIndex { get; protected set; }
        public Boat Boat { get; protected set; }

        protected Player[] Players { get; set; }
        protected Player[] PlayersByColour { get; protected set; }

        public Message CurrentMessage;

        public Player(PlayerIndex playerIndex)
        {
            PlayerIndex = playerIndex;
            Players = new Player[4];
            PlayersByColour = new Player[4];
        }

        public void Initialize(Player[] players, Island.Colour colour)
        {
            Players = players;
            //Boat = new Boat();
        }

        public virtual void HandleInput()
        {
            // get the keyboard state
            KeyboardState keyboardState = Keyboard.GetState();

            // get the current player's gamepad state
            GamePadState gamePadState = GamePad.GetState(PlayerIndex);

            CurrentMessage = Message.NoMessage;
            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                CurrentMessage = Message.Start;
            }

            if (!gamePadState.IsConnected)
            {
                Debug.WriteLine("Player " + PlayerIndex.ToString() + "'s controller is disconnected.");
                return;
            }
            else
            {
                if (gamePadState.Buttons.Start == ButtonState.Pressed)
                    CurrentMessage = Message.Start;
            }
        }
    }
}
