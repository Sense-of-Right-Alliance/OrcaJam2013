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
        public Colour Colour { get; protected set; }
        public Resource ResourceType { get; protected set; }

        public bool hasBlue = false;
        public bool hasRed = false;
        public bool hasYellow = false;
        public bool hasGreen = false;

        public bool HasAllResources { get { return (hasBlue && hasRed && hasYellow && hasGreen); } }

        private Token redToken;
        private Token blueToken;
        private Token yellowToken;
        private Token greenToken;

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

        public string IslandName
        {
            get { return Type.ToString(); }
        }

        public Island(Texture2D sprite, Colour colour, IslandType type, Resource resource, Texture2D[] tokens) : base(sprite)
        {
            Colour = colour;
            scale = new Vector2(0.3f);
            ResourceType = resource;
            Type = type;

            redToken = new Token(tokens[0]);
            blueToken = new Token(tokens[1]);
            yellowToken = new Token(tokens[2]);
            greenToken = new Token(tokens[3]);

            AddResource(resource);
        }

        public void StartIsland()
        {
            redToken.position = new Vector2(position.X + 3 * sprite.Width / 4 * scale.X, position.Y);
            blueToken.position = new Vector2(position.X - 3 * sprite.Width / 4 * scale.X, position.Y);
            yellowToken.position = new Vector2(position.X, position.Y - 3 * sprite.Height / 4 * scale.Y);
            greenToken.position = new Vector2(position.X, position.Y + 3 * sprite.Height / 4 * scale.Y); 
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

            // create a resource for the selected islandtype
            Resource resource = Resource.InitializeFromIslandType(colour, type, content);

            Texture2D rToken = content.Load<Texture2D>("Tokens/RedToken");
            Texture2D bToken = content.Load<Texture2D>("Tokens/BlueToken");
            Texture2D yToken = content.Load<Texture2D>("Tokens/YellowToken");
            Texture2D gToken = content.Load<Texture2D>("Tokens/GreenToken");

            Texture2D[] tokens = { rToken, bToken, yToken, gToken };

            // create a new entity using the loaded sprite
            return new Island(sprite, colour, type, resource,tokens);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (hasRed)
                redToken.Update(gameTime);
            if (hasBlue)
                blueToken.Update(gameTime);
            if (hasYellow)
                yellowToken.Update(gameTime);
            if (hasGreen)
                greenToken.Update(gameTime);
        }

        public void AddResource(Resource r)
        {
            switch (r.Colour)
            {
                case(Colour.Red):
                    hasRed = true;
                    break;
                case (Colour.Blue):
                    hasBlue = true;
                    break;
                case (Colour.Yellow):
                    hasYellow = true;
                    break;
                case (Colour.Green):
                    hasGreen = true;
                    break;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (hasRed)
                redToken.Draw(spriteBatch);
            if(hasBlue)
                blueToken.Draw(spriteBatch);
            if(hasYellow)
                yellowToken.Draw(spriteBatch);
            if(hasGreen)
                greenToken.Draw(spriteBatch);
        }
    }
}
