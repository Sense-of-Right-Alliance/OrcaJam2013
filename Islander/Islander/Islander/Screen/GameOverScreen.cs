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
        List<List<Vector2?>> playerTextPositions;

        static List<Vector2> outlineOffsets = new List<Vector2>()
        {
            new Vector2(1,0),
            new Vector2(-1,0),
            new Vector2(0,1),
            new Vector2(0,-1),
        };

        public GameOverScreen(Islander.GameState gameState)
        {
            GameState = gameState;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            background = content.Load<Texture2D>("Splash Screens/SplashEndgame");

            // set positions for player names and text
            playerTextPositions = new List<List<Vector2?>>()
            {
                new List<Vector2?>()
                {
                    new Vector2((int)(width * 0.125), (int)(height * 0.38)),
                    new Vector2((int)(width * 0.76), (int)(height * 0.39)),
                },
                new List<Vector2?>()
                {
                    new Vector2((int)(width * 0.125), (int)(height * 0.52)),
                    new Vector2((int)(width * 0.76), (int)(height * 0.53)),
                },
                new List<Vector2?>()
                {
                    new Vector2((int)(width * 0.125), (int)(height * 0.66)),
                    null,
                },
            };
        }

        public override void Draw(GameTime gameTime, GraphicsDevice GraphicsDevice)
        {
            base.Draw(gameTime, GraphicsDevice);

            int playerNum = 0;
            var playersByScore = players
                .OrderByDescending(u => u.Island.HasAllTokens) // order players by those who have all the tokens
                .ThenByDescending(u => u.Score) // then by those who have the highest score
                .Take(playerTextPositions.Count) // ignore the 4th player
                .Select( // organize our results
                    u => new
                    {
                        player = u,
                        score = u.Score,
                        name = u.ScoreName,
                        fontColour = GetXNAColor(u.Colour),
                        namePosition = playerTextPositions[playerNum][0],
                        scorePosition = playerTextPositions[playerNum][1],
                        resultsStanding = playerNum++,
                        tradeVictory = u.Island.HasAllTokens
                    });

            foreach (var player in playersByScore)
            {
                DrawPlayerName(player.name, player.namePosition, player.fontColour);
                if (player.tradeVictory)
                {
                    DrawOutlinedString("Trade Victory!!", player.scorePosition - new Vector2(width / 20, 0), player.fontColour);
                }
                else // draw score
                {
                    DrawPlayerScore(player.score, player.scorePosition, player.fontColour);
                }
            }
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
                    x = Color.Red;
                    break;
                case (Colour.Yellow):
                    x = Color.Yellow;
                    break;
            }
            return x;
        }

        private void DrawPlayerName(string playerName, Vector2? position, Color fontColour)
        {
            DrawOutlinedString(playerName, position, fontColour);
        }

        private void DrawPlayerScore(int playerScore, Vector2? position, Color fontColour)
        {
            DrawOutlinedString(playerScore.ToString(), position, fontColour);
        }

        // draws an outlined string at a position
        private void DrawOutlinedString(string text, Vector2? position, Color fontColour)
        {
            if (position != null)
            {
                // draw outline
                foreach (var offset in outlineOffsets)
                    spriteBatch.DrawString(scoreFont, text, position.Value + offset, Color.Black);
                // draw string
                spriteBatch.DrawString(scoreFont, text, position.Value, fontColour);
            }
        }
    }
}
