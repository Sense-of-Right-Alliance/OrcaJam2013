using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Islander.Entity
{
    class Bullet : Entity
    {
        private Vector2 velocity;

        public Bullet(Texture2D sprite, Vector2 position, float speed) : base(sprite)
        {
            velocity = position * speed;
            this.position = position;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            position += velocity;
        }
    }
}
