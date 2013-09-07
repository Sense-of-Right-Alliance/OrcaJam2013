using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Islander.Entity
{
    class Island : Entity
    {
        public Colour Colour { get; protected set; }

        public Island(Texture2D sprite):base(sprite)
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

    }
}
