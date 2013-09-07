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

        //Dylbro set this to public to allow the screens to manipulate it. Correct me if I'm wrong.
        public Vector2 position;

        public Entity(Texture2D sprite)
        {
            this.sprite = sprite;
        }

        public Rectangle HitBox()
        {
            return new Rectangle((int)position.X - sprite.Width/2, (int)position.Y - sprite.Height/2, sprite.Width, sprite.Height);
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
