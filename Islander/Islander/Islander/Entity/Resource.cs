using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Islander.Entity
{
    class Resource : Entity
    {
        public Colour Colour { get; protected set; }
        public bool IsCarried { get; set; }

        public Resource(Texture2D sprite, Colour colour) : base(sprite)
        {
            Colour = colour;
            scale = new Vector2(0.5f);
            IsCarried = true;
        }

        // creates a new boat matching the specified colour, loading the sprite from the contentmanager
        public static Resource InitializeFromColour(Colour colour, ContentManager content)
        {
            // retrieve texture name matching specified colour
            string textureName = "";
            switch (colour)
            {
                case Colour.Blue:
                    textureName = "ResourceBlue";
                    break;
                case Colour.Yellow:
                    textureName = "ResourceYellow";
                    break;
                case Colour.Red:
                    textureName = "ResourceRed";
                    break;
                case Colour.Green:
                    textureName = "ResourceGreen";
                    break;
            }

            // load the texture specified from a folder named Resources
            Texture2D sprite = content.Load<Texture2D>("Resources/" + textureName);

            // create a new entity using the loaded sprite
            return new Resource(sprite, colour);
        }
    }
}
