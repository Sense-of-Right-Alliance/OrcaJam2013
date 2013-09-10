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
        PlayerIndex firstPlayerNumber;
        PlayerIndex secondPlayerNumber;
        PlayerIndex thirdPlayerNumber;

        int firstScore;
        int secondScore;
        int thirdScore;

        Vector2 firstPlayerNamePos;
        Vector2 firstPlayerScorePos;
        Vector2 secondPlayerNamePos;
        Vector2 secondPlayerScorePos;
        Vector2 thirdPlayerNamePos;

        Color firstColour;
        Color secondColour;
        Color thirdColour;
        public GameOverScreen(Islander.GameState gameState)
        {
            GameState = gameState;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            background = content.Load<Texture2D>("Splash Screens/SplashEndgame");
            /*Set PlayerName+Score Vector positions*/
            firstPlayerNamePos = new Vector2((int)(width * 0.125), (int)(height * 0.38));
            firstPlayerScorePos = new Vector2((int)(width * 0.76), (int)(height * 0.39));
            secondPlayerNamePos = new Vector2((int)(width * 0.125), (int)(height * 0.52));
            secondPlayerScorePos = new Vector2((int)(width * 0.76), (int)(height * 0.53));
            thirdPlayerNamePos = new Vector2((int)(width * 0.125), (int)(height * 0.66));
            
        }

        public override void Draw(GameTime gameTime, GraphicsDevice GraphicsDevice)
        {
            base.Draw(gameTime, GraphicsDevice);
            //TODO: We need a function for this
            firstScore = -10000;
            firstPlayerNumber = PlayerIndex.One;
            secondScore = -10000;
            secondPlayerNumber = PlayerIndex.One;
            thirdScore = -10000;
            thirdPlayerNumber = PlayerIndex.One;
            
            foreach(var player in players)
            {
                if(player.score >= firstScore){
                    thirdScore = secondScore;
                    secondScore = firstScore;
                    firstScore = player.score;
                    thirdPlayerNumber = secondPlayerNumber;
                    secondPlayerNumber = firstPlayerNumber;
                    firstPlayerNumber = player.PlayerIndex;
                }
                else if (player.score >= secondScore)
                {
                    thirdScore = secondScore;
                    secondScore = player.score;
                    thirdPlayerNumber = secondPlayerNumber;
                    secondPlayerNumber = player.PlayerIndex;
                }
                else if (player.score >= thirdScore)
                {
                    thirdScore = player.score;
                    thirdPlayerNumber = player.PlayerIndex;
                }
            }
            firstColour = GetXNAColor(players[(int)firstPlayerNumber].Colour);
            secondColour = GetXNAColor(players[(int)secondPlayerNumber].Colour);
            thirdColour = GetXNAColor(players[(int)thirdPlayerNumber].Colour);

            outlineFont(firstScore, firstPlayerNumber, firstPlayerNamePos, firstPlayerScorePos);
            spriteBatch.DrawString(scoreFont, "Player " + firstPlayerNumber, firstPlayerNamePos, firstColour);
            outlineFont(secondScore, secondPlayerNumber, secondPlayerNamePos, secondPlayerScorePos);
            spriteBatch.DrawString(scoreFont, "" + firstScore, firstPlayerScorePos, firstColour);
            spriteBatch.DrawString(scoreFont, "Player " + secondPlayerNumber, secondPlayerNamePos, secondColour);
            spriteBatch.DrawString(scoreFont, "" + secondScore, secondPlayerScorePos, secondColour);
            outlineFont(0,thirdPlayerNumber, thirdPlayerNamePos, new Vector2 (5000.0f,2000.0f));
            spriteBatch.DrawString(scoreFont, "Player " + thirdPlayerNumber, thirdPlayerNamePos, thirdColour);
            
        }


        private Color GetXNAColor(Colour c)
        {
            Color x = Color.Black;
            switch (c)
            {
                case (Colour.Blue):
                    x = Color.Blue;
                    break;
                case (Colour.Green):
                    x = Color.Green;
                    break;
                case (Colour.Red):
                    x =Color.Red;
                    break;
                case (Colour.Yellow):
                    x = Color.Yellow;
                    break;
            }
            
            return x;
        }
        /*Don't look at this method. Seriously just don't. Ignore the fact that it's run 4 times every frame. Don't even worry about it.
         * Alternatively, make it more general and give it to the other menu screens as well. Your call.
         * Fucking thanks yellow, you bastard.*/
         
        private void outlineFont(int playerScore, PlayerIndex playerIndex, Vector2 scoreLabelPos, Vector2 scorePos)
        {
            spriteBatch.DrawString(scoreFont, "Player " + playerIndex, new Vector2(scoreLabelPos.X+1,scoreLabelPos.Y), Color.Black);
            spriteBatch.DrawString(scoreFont, "" + playerScore, new Vector2(scorePos.X + 1, scorePos.Y), Color.Black);
            spriteBatch.DrawString(scoreFont, "Player " + playerIndex, new Vector2(scoreLabelPos.X - 1, scoreLabelPos.Y), Color.Black);
            spriteBatch.DrawString(scoreFont, "" + playerScore, new Vector2(scorePos.X - 1, scorePos.Y), Color.Black);
            spriteBatch.DrawString(scoreFont, "Player " + playerIndex, new Vector2(scoreLabelPos.X, scoreLabelPos.Y + 1), Color.Black);
            spriteBatch.DrawString(scoreFont, "" + playerScore, new Vector2(scorePos.X, scorePos.Y + 1), Color.Black);
            spriteBatch.DrawString(scoreFont, "Player " + playerIndex, new Vector2(scoreLabelPos.X, scoreLabelPos.Y - 1), Color.Black);
            spriteBatch.DrawString(scoreFont, "" + playerScore, new Vector2(scorePos.X, scorePos.Y - 1), Color.Black);
        }
    }
}
