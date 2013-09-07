using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Islander.Entity
{
    class EnemyBoat :Entity
    {
        /*This is on the back burner. No boids for now :(*/
        private Vector2 velocity;
        //Target is the Player that the EnemyBoat moves towards
        public Colour target { get; set; }

        public EnemyBoat(Texture2D sprite, Vector2 position) : base(sprite)
        {
            
        }

        public EnemyBoat(Texture2D sprite, Vector2 position, Colour target) : base(sprite)
        {

        }

        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
            position += velocity;
        }

    }
}
