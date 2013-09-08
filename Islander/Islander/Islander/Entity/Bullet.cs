﻿using System;
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
        public bool[] HostileToPlayer { get; set; }
        public Colour Colour { get; protected set; }

        public Bullet(Texture2D sprite, Vector2 direction, float speed, Colour colour, bool[] hostileToPlayer) : base(sprite)
        {
            
            this.velocity = direction * speed;
            this.rotation = GetRotation(direction);
            this.position = direction;
            this.Colour = colour;
            this.HostileToPlayer = hostileToPlayer;
            this.scale = new Vector2(0.5f);
        }

        private float GetRotation(Vector2 direction)
        {
            Vector2 up = new Vector2(0.0f, -1.0f);
            up.Normalize();

            double x = Vector2.Dot(up, direction) / (VectorMagnitude(up) * VectorMagnitude(direction));

            float rotation = 0.0f;

            if (Math.Abs(x) <= 1)
            {
                rotation = (float)Math.Acos(x);
            }

            if (velocity.X < 0)
            {
                float deg =  rotation * 100 / (float) Math.PI;
                rotation = 270.5f - rotation;

            }

            return rotation;
        }

        private double VectorMagnitude(Vector2 v)
        {
            return Math.Sqrt((Math.Pow(v.X, 2.0) + Math.Pow(v.Y, 2.0)));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            position += velocity;
        }
    }
}
