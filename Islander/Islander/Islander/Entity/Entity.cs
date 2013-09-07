using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Islander.Entity
{
    class Entity
    {
        protected Texture2D sprite;
        public Vector2 position;

        public Entity(Texture2D sprite)
        {
            this.sprite = sprite;
        }

        public Rectangle HitBox()
        {
            return new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);
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
