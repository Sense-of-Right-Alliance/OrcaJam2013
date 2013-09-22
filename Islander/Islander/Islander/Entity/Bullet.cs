using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;


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

        private const float NORMAL_SPEED = 5.0f;
        private const float NORMAL_TIME = 0.5f;
        private const float NORMAL_SCALE = 0.5f;

        private const float TRIDENT_SPEED = 5.0f;
        private const float TRIDENT_TIME = 0.5f;

        private const float BUBBLE_SPEED = 4.0f;
        private const float BUBBLE_TIME = 0.5f;
        private const float BUBBLE_SCALE = 0.7f;

        private const float RAZOR_SPEED = 7.0f;
        private const float RAZOR_TIME = 0.35f;

        private const float MAGNET_SPEED = 5.0f;
        private const float MAGNET_TIME = 0.5f;


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

        // passes in texture because loading content for each bullet is slow!
        public static Bullet[] CreateBullet(Texture2D texture,BulletType bulletType,Vector2 direction, Colour colour, bool[] HostileToPlayer, Player p)
        {
            Bullet[] bullets = new Bullet[1];

            //Texture2D texture;
            float speed = NORMAL_SPEED;
            float time = NORMAL_TIME;
            float scale = NORMAL_SCALE;

            switch (bulletType)
            {
                case (Bullet.BulletType.Normal):
                    bullets[0] = new Bullet(texture, direction, speed, time, colour, HostileToPlayer, bulletType, scale, p);
                    break;
                case (Bullet.BulletType.Trident):
                    //texture = content.Load<Texture2D>("Bullets/RedTrident");
                    speed = NORMAL_SPEED;
                    time = NORMAL_TIME;
                    scale = NORMAL_SCALE;
                    bullets = new Bullet[3];
                    bullets[0] = new Bullet(texture, direction, speed, time, colour, HostileToPlayer, bulletType, scale, p);
                    bullets[1] = new Bullet(texture, RotateVector(direction, (float)Math.PI / 6), speed, time, colour, HostileToPlayer, bulletType, scale, p);
                    bullets[2] = new Bullet(texture, RotateVector(direction, -(float)Math.PI / 6), speed, time, colour, HostileToPlayer, bulletType, scale, p);
                    break;
                case (Bullet.BulletType.Bubble):
                    //texture = content.Load<Texture2D>("Bullets/BlueBubble");
                    speed = NORMAL_SPEED;
                    time = NORMAL_TIME;
                    scale = NORMAL_SCALE;
                    bullets[0] = new Bullet(texture, direction, speed, time, colour, HostileToPlayer, bulletType, scale, p);
                    break;
                case (Bullet.BulletType.Razor):
                    //texture = content.Load<Texture2D>("Bullets/YellowRazor");
                    speed = NORMAL_SPEED;
                    time = NORMAL_TIME;
                    scale = NORMAL_SCALE;
                    bullets[0] = new Bullet(texture, direction, speed, time, colour, HostileToPlayer, bulletType, scale, p);
                    break;
                case (Bullet.BulletType.Magnet):
                    //texture = content.Load<Texture2D>("Bullets/GreenMagnet");
                    speed = NORMAL_SPEED;
                    time = NORMAL_TIME;
                    scale = NORMAL_SCALE;
                    bullets[0] = new Bullet(texture, direction, speed, time, colour, HostileToPlayer, bulletType, scale, p);
                    break;
            }


            // create a new entity using the loaded sprite
            return bullets;
        }

        public static Vector2 RotateVector(Vector2 vector, float radians)
        {
            float angle = VectorToAngle(vector) + radians;
            return AngleToVector(angle) * vector.Length();
        }

        static public Vector2 AngleToVector(float angle)
        {
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        static public float VectorToAngle(Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        } 

        /* Don't need unless loading textures every time you make a bullet, which sounds stupid
        private static Texture2D LoadNormalTexture(ContentManager cm, Colour c) 
        {
             // load default bullet sprite
            String bulletFilename = "";
            switch (c)
            {
                case Colour.Blue:
                    bulletFilename = "Blue";
                    break;
                case Colour.Yellow:
                    bulletFilename = "Yellow";
                    break;
                case Colour.Red:
                    bulletFilename = "Red";
                    break;
                case Colour.Green:
                    bulletFilename = "Green";
                    break;
            }
            bulletFilename += "Default";
            return cm.Load<Texture2D>("Bullets/" + bulletFilename);  
        }*/

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
