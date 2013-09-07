using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Islander.Entity
{
    class Entity
    {
        protected Texture2D sprite;
        protected Vector2 scale;
        protected float rotation;

        // Dylbro set this to public to allow the screens to manipulate it. Correct me if I'm wrong.
        public Vector2 position;
        
        public Entity(Texture2D sprite)
        {
            this.sprite = sprite;
            scale = new Vector2(1.0f);
        }

        public Rectangle HitBox()
        {
            return new Rectangle((int)position.X - sprite.Width/2, (int)position.Y - sprite.Height/2, sprite.Width, sprite.Height);
        }

        public Rectangle DrawRect()
        {
            return new Rectangle((int)position.X, (int)position.Y, (int) (sprite.Width * scale.X), (int) (sprite.Height * scale.Y));
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, DrawRect(), new Rectangle(0,0,sprite.Width,sprite.Height), Color.White, rotation, new Vector2(sprite.Width/2,sprite.Height/2), SpriteEffects.None, 1);
        }
    }
}
