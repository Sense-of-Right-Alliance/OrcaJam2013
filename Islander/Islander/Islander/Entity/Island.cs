using System;
using System.Collections.Generic;
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

        // lists of our different island textures
        static List<string> blueIslands = new List<string>()
        {
            "IslandBlueWave",
            "IslandBlueBubble",
            // etc.
        };
        static List<string> yellowIslands = new List<string>()
        {
            "IslandYellowTranquility",
            // etc.
        };
        static List<string> redIslands = new List<string>()
        {
            "IslandRedBattle",
            // etc.
        };
        static List<string> greenIslands = new List<string>()
        {
            "IslandGreenLeaf",
            // etc.
        };

        public Island(Texture2D sprite, Colour colour) : base(sprite)
        {
            Colour = colour;
        }

        // creates a new boat matching the specified colour, loading the sprite from the contentmanager
        public static Island InitializeFromColour(Colour colour, ContentManager content)
        {
            // retrieve texture name from list that matches specified colour
            // choosing randomly using random number generator to choose fron the islands available
            var rng = new Random();
            string textureName = "";
            switch (colour)
            {
                case Colour.Blue:
                    textureName = blueIslands[rng.Next(blueIslands.Count)];
                    break;
                case Colour.Yellow:
                    textureName = yellowIslands[rng.Next(yellowIslands.Count)];
                    break;
                case Colour.Red:
                    textureName = redIslands[rng.Next(redIslands.Count)];
                    break;
                case Colour.Green:
                    textureName = greenIslands[rng.Next(greenIslands.Count)];
                    break;
            }

            // load the texture specified from a folder named Islands
            Texture2D sprite = content.Load<Texture2D>("Islands/" + textureName);

            // create a new entity using the loaded sprite
            return new Island(sprite, colour);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
