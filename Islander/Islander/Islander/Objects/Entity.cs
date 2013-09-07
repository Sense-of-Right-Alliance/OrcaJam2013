using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Islander.Objects
{
    class Entity
    {
        protected Texture2D sprite;
        protected Vector2 position;
        protected int width;
        protected int height;

        public Entity(Texture2D sprite)
        {

        }

        public Rectangle HitBox()
        {
            return new Rectangle((int)position.X, (int)position.Y, width, height);
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, HitBox(), Color.White);
        }
    }
}
