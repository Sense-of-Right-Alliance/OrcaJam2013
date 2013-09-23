using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Islander.Entity
{
    class PowerUp : Entity
    {
        private float fadeToggle = 1.0f;

        public PowerUp(Texture2D sprite, Vector2 pos) : base(sprite)
        {
            Alpha = 0.5f;
            Scale = new Vector2(0.5f);
            Position = pos;
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

            if (Alpha <= 0.5f)
            {
                Alpha = 0.5f;
                fadeToggle = 1.0f;
            }

        }
    }
}
