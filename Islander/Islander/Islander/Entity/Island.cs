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

        public enum IslandType
        {
            BlueBubble,
            BlueFantasy,
            YellowRazor,
            YellowTreasure,
            RedLove,
            RedTrident,
            GreenHermit,
            GreenMagnet
        }

        public IslandType Type { get; protected set; }

        static Dictionary<IslandType, string> blueIslands = new Dictionary<IslandType, string>()
        {
            {IslandType.BlueBubble, "BlueBubble"},
            {IslandType.BlueFantasy, "BlueFantasy"},
        };

        static Dictionary<IslandType, string> yellowIslands = new Dictionary<IslandType, string>()
        {
            {IslandType.YellowRazor, "YellowRazor"},
            {IslandType.YellowTreasure, "YellowTreasure"},
        };

        static Dictionary<IslandType, string> redIslands = new Dictionary<IslandType, string>()
        {
            {IslandType.RedLove, "RedLove"},
            {IslandType.RedTrident, "RedTrident"},
        };

        static Dictionary<IslandType, string> greenIslands = new Dictionary<IslandType, string>()
        {
            {IslandType.GreenHermit, "GreenHermit"},
            {IslandType.GreenMagnet, "GreenMagnet"},
        };

        public Island(Texture2D sprite, Colour colour, IslandType type, Resource resource) : base(sprite)
        {
            Colour = colour;
            scale = new Vector2(0.5f);
            ResourceType = resource;
            Type = type;
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

            // create a new entity using the loaded sprite
            return new Island(sprite, colour, type, resource);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
