using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Islander.Entity
{
    class Token : Entity
    {
        public Colour Colour { get; set; }

        private float fadeToggle = 1.0f;

        public Token(Texture2D sprite, Colour colour) : base(sprite)
        {
            Colour = colour;
            Scale = new Vector2(0.5f);
        }

        // creates a copy of the given token
        public static Token Copy(Token token)
        {
            Token t = new Token(token.sprite, token.Colour)
            {
                Alpha = token.Alpha,
                Position = token.Position,
                Rotation = token.Rotation,
                Scale = token.Scale,
            };
            return t;
        }

        // creates a new token matching the specified colour, loading the sprite from the contentmanager
        public static Token InitializeFromColour(Colour colour, ContentManager content)
        {
            // retrieve texture name matching specified colour
            string textureName = "";
            switch (colour)
            {
                case Colour.Blue:
                    textureName = "BlueToken";
                    break;
                case Colour.Yellow:
                    textureName = "YellowToken";
                    break;
                case Colour.Red:
                    textureName = "RedToken";
                    break;
                case Colour.Green:
                    textureName = "GreenToken";
                    break;
            }

            // load the texture specified from a folder named Tokens
            Texture2D sprite = content.Load<Texture2D>("Tokens/" + textureName);

            // create a new entity using the loaded sprite
            return new Token(sprite, colour);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Alpha += fadeToggle * (1.0f / 1.0f) * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Alpha >= 1.0f)
            {
                Alpha = 1.0f;
                fadeToggle = -1.0f;
            }
            else if (Alpha <= 0.5f)
            {
                Alpha = 0.5f;
                fadeToggle = 1.0f;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
