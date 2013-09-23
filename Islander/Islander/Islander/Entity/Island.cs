using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Islander.Entity
{
    class Island : Entity
    {
        public enum IslandType
        {
            Bubble,
            Fantasy,
            Razor,
            Treasure,
            Love,
            Trident,
            Hermit,
            Magnet
        }

        public IslandType Type { get; protected set; }
        private Token[] Tokens { get; set; }

        public Colour Colour { get; protected set; }
        public Resource ResourceType { get; protected set; }
        public Token TokenType { get { return Tokens[(int)Colour]; } }

        public bool HasAllTokens
        {
            get
            {
                foreach (var token in Tokens) if (token == null) return false;
                return true;
            }
        }

        protected Vector2 position;
        public override Vector2 Position
        {
            get { return position; }
            set
            {
                // update all tokens so they move with the island
                Vector2 change = value - Position;
                foreach (var token in Tokens)
                    if (token != null)
                        token.Position += change;
                // update the island position
                position = value;
            }
        }

        public string IslandName
        {
            get { return Type.ToString(); }
        }

        static Dictionary<IslandType, string> blueIslands = new Dictionary<IslandType, string>()
        {
            {IslandType.Bubble, "BlueBubble"},
            {IslandType.Fantasy, "BlueFantasy"},
        };

        static Dictionary<IslandType, string> yellowIslands = new Dictionary<IslandType, string>()
        {
            {IslandType.Razor, "YellowRazor"},
            {IslandType.Treasure, "YellowTreasure"},
        };

        static Dictionary<IslandType, string> redIslands = new Dictionary<IslandType, string>()
        {
            {IslandType.Love, "RedLove"},
            {IslandType.Trident, "RedTrident"},
        };

        static Dictionary<IslandType, string> greenIslands = new Dictionary<IslandType, string>()
        {
            {IslandType.Hermit, "GreenHermit"},
            {IslandType.Magnet, "GreenMagnet"},
        };

        public Island(Texture2D sprite, Colour colour, IslandType type, Resource resource, Token token) : base(sprite)
        {
            Colour = colour;
            Scale = new Vector2(0.3f);
            ResourceType = resource;
            Type = type;

            Tokens = new Token[4];
            AddToken(token); // add the token for this colour of island
        }

        // creates a new boat matching the specified colour, loading the sprite from the contentmanager
        public static Island InitializeFromColour(Colour colour, ContentManager content)
        {
            // retrieve texture name from list that matches specified colour
            // choosing randomly using random number generator to choose fron the islands available
            var rng = new Random();
            string textureName = "";
            Dictionary<IslandType, string> islands = null;
            switch (colour)
            {
                case Colour.Blue:
                    islands = blueIslands;
                    break;
                case Colour.Yellow:
                    islands = yellowIslands;
                    break;
                case Colour.Red:
                    islands = redIslands;
                    break;
                case Colour.Green:
                    islands = greenIslands;
                    break;
            }

            // retrieve a random islandtype matching the colour and the associated texture
            IslandType type = islands.Keys.ToList()[rng.Next(islands.Count)];
            textureName = islands[type];

            // load the texture specified from a folder named Islands
            Texture2D sprite = content.Load<Texture2D>("Islands/" + textureName);
            
            //Debug.WriteLine("Island Content Loaded: " + textureName);

            // create a resource for this island
            var resource = Resource.InitializeFromIslandType(colour, type, content);

            // create a token for this island
            var token = Token.InitializeFromColour(colour, content);

            // create a new entity using the loaded sprite
            return new Island(sprite, colour, type, resource, token);
        }

        // adds a token to the island
        public void AddToken(Token token)
        {
            Tokens[(int)token.Colour] = token;
            switch (token.Colour)
            {
                case Colour.Blue:
                    token.Position = new Vector2(Position.X - 3 * sprite.Width / 4 * Scale.X, Position.Y);
                    break;
                case Colour.Green:
                    token.Position = new Vector2(Position.X, Position.Y + 3 * sprite.Height / 4 * Scale.Y);
                    break;
                case Colour.Red:
                    token.Position = new Vector2(Position.X + 3 * sprite.Width / 4 * Scale.X, Position.Y);
                    break;
                case Colour.Yellow:
                    token.Position = new Vector2(Position.X, Position.Y - 3 * sprite.Height / 4 * Scale.Y);
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (var token in Tokens)
                if (token != null)
                    token.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            foreach (var token in Tokens)
                if (token != null)
                    token.Draw(spriteBatch);
        }
    }
}
