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
        protected float alpha = 1.0f;

        // Dylbro set this to public to allow the screens to manipulate it. Correct me if I'm wrong.
        public Vector2 position;

        public float Rotation { get{ return rotation; } }
        
        public Entity(Texture2D sprite)
        {
            this.sprite = sprite;
            scale = new Vector2(1.0f);
        }

        public bool CollidesWith(Entity otherEntity)
        {
            return HitBox().Intersects(otherEntity.HitBox());
        }

        public Rectangle HitBox()
        {
            return new Rectangle((int)(position.X - sprite.Width * scale.X / 2), (int)(position.Y - sprite.Height * scale.Y / 2), (int)(sprite.Width * scale.X), (int)(sprite.Height * scale.Y));
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
            Color c = Color.White;
            c *= alpha;
            spriteBatch.Draw(sprite, DrawRect(), new Rectangle(0,0,sprite.Width,sprite.Height), c, rotation, new Vector2(sprite.Width/2,sprite.Height/2), SpriteEffects.None, 1);
        }
    }
}
