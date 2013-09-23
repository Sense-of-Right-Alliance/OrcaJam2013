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
        public virtual Vector2 Position { get; set; }
        public float PositionX { get { return Position.X; } set { Position = new Vector2(value, Position.Y); } }
        public float PositionY { get { return Position.Y; } set { Position = new Vector2(Position.X, value); } }
        public Vector2 Scale { get; set; }
        public float Rotation { get; set; }
        public float Alpha { get; set; }
        protected Texture2D sprite;
        
        public Entity(Texture2D sprite)
        {
            this.sprite = sprite;
            Scale = new Vector2(1.0f);
            Alpha = 1.0f;
        }

        public bool CollidesWith(Entity otherEntity)
        {
            return HitBox().Intersects(otherEntity.HitBox());
        }

        public Rectangle HitBox()
        {
            return new Rectangle((int)(Position.X - sprite.Width * Scale.X / 2), (int)(Position.Y - sprite.Height * Scale.Y / 2), (int)(sprite.Width * Scale.X), (int)(sprite.Height * Scale.Y));
        }

        public Rectangle DrawRect()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, (int) (sprite.Width * Scale.X), (int) (sprite.Height * Scale.Y));
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Color c = Color.White;
            c *= Alpha;
            spriteBatch.Draw(sprite, DrawRect(), new Rectangle(0,0,sprite.Width,sprite.Height), c, Rotation, new Vector2(sprite.Width/2,sprite.Height/2), SpriteEffects.None, 1);
        }
    }
}
