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
        public enum BulletType
        {
            Normal,
            Trident,
            Bubble,
            Razor,
            Magnet
        }

        public BulletType type = BulletType.Normal;

        private Vector2 velocity;
        private float timer = 0.0f;
        private float lifeTime = 0.5f;
        private Vector2 start;
        public bool[] HostileToPlayer { get; set; }
        public Colour Colour { get; protected set; }
        public bool done = false;
        private Player player;

        public Vector2 dir;

        public Bullet(Texture2D sprite, Vector2 direction, float speed, float time, Colour colour, bool[] hostileToPlayer, BulletType type, float scale, Player p) : base(sprite)
        {
            
            this.velocity = direction * speed;
            this.rotation = GetRotation(direction);
            this.position = direction;
            this.Colour = colour;
            this.HostileToPlayer = hostileToPlayer;
            this.scale = new Vector2(scale);
            this.type = type;
            this.player = p;
            this.lifeTime = time;


            dir = direction;

            start = position;
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
                float deg =  rotation * 180 / (float)Math.PI;
                deg = 360 - deg;
                rotation = deg * (float)Math.PI/180;

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

            if (type == BulletType.Magnet)
                UpdateMagnet(gameTime);


            position += velocity;


            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(timer >= lifeTime) 
            {
                done = true;
            }
        }

        private void UpdateMagnet(GameTime gameTime)
        {
            if (timer > 0.2f)
            {
                Boat b = GetClosestBoatInRadius(position, 100.0f);
                if (b != null)
                {
                    Vector2 towards = b.position - position;
                    towards.Normalize();
                    velocity += towards * 0.2f;

                    dir = velocity;
                    dir.Normalize();
                }
            }
        }

        public Boat GetClosestBoatInRadius(Vector2 pos, float r)
        {
            Boat b = null;
            float dist = 999.0f;
            foreach (Player p in player.PlayersByColour)
            {
                if(p != player)
                {
                    float d = (p.Boat.position - pos).Length();
                    if (d < r && d < dist)
                    {
                        dist = d;
                        b = p.Boat;
                    }
                }
            }

            return b;
        }
    }
}
