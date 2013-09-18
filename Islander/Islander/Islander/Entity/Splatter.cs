using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Islander.Entity
{
    class Splatter : Entity
    {
        enum SplatterState
        {
            In,
            Idle,
            Out,
            Done
        }

        SplatterState state = SplatterState.In;

        const float IDLE_TIME = 2.0f;
        const float IN_TIME = 0.25f;
        const float OUT_TIME = 1.0f;

        float timer = 0.0f;

        public Splatter(Texture2D sprite, float s)
            : base(sprite)
        {
            scale = new Vector2(s);

            state = SplatterState.Done;

            alpha = 0.0f;
        }

        public void CreateDeathSplatter(Vector2 pos, float rotation)
        {
            base.position = pos;
            base.rotation = rotation;
            alpha = 0.0f;
            timer = 0.0f;
            state = SplatterState.In;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            switch(state)
            {
                case(SplatterState.In):
                    AnimateIn(gameTime);
                    break;
                case(SplatterState.Idle):
                    HandleIdle(gameTime);
                    break;
               case(SplatterState.Out):
                    AnimateOut(gameTime);
                    break;

            }
        }

        private void AnimateIn(GameTime gameTime)
        {
            alpha += (1.0f / IN_TIME) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (alpha >= 1.0f)
            {
                alpha = 1.0f;
                state = SplatterState.Idle;
            }
        }

        private void HandleIdle(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer >= IDLE_TIME)
            {
                state = SplatterState.Out;
            }
        }

        private void AnimateOut(GameTime gameTime)
        {
            alpha -= (1.0f / OUT_TIME) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (alpha <= 0.0f)
            {
                alpha = 0.0f;
                state = SplatterState.Done;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(state != SplatterState.Done)
                base.Draw(spriteBatch);
        }
    }
}
