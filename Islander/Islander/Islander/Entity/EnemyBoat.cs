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
        private Vector2 position;

        private const float ENEMY_SPEED = 10.0f;
        //Target is the Player that the EnemyBoat moves towards
        public Colour target { get; set; }

        public EnemyBoat(Texture2D sprite, Vector2 position) : base(sprite)
        {
            this.position = position;
        }

        public EnemyBoat(Texture2D sprite, Vector2 position, Colour target) : base(sprite)
        {
            this.position = position;
            this.target = target;
        }

        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
            if (target == null)
                chooseTarget();

            //Velocity is a Vector that represents Direction and speed.
            DetermineVelocity();
            position += velocity;
        }

        private void DetermineVelocity()
        {
            //This is the basic AI method that determines where the boat is headed by using it's target.

            //The most basic first iteration simply makes the enemy boats head straight to the target.
            /*TODO: This line is just straight up wrong, couldn't quite find the math on it.
             * If we do end up using this, then the point of this line is to find the direction of the target from the enemy ship.
             * Also this class really needs access to the player list.*/
            //velocity = position - players[target];

            //Later iterations can use pathfinding/boids algorithm to follow it a little more intelligently.
        }

        public void chooseTarget()
        {
            //This should do it randomly, by highest score, or by closest player.
            //Closest Player
            Colour min = Colour.Blue;
            Vector2 minDistance = new Vector2(2000.0f);
            /*foreach (var player in players)
            {
                Vector2 distanceToPlayer = Vector2.Distance(position, player.position);
                if (distanceToPlayer < minDistance)
                {
                    minDistance = distanceToPlayer;
                    minColour = player.Colour;
                }
            }*/
        }

    }
}
