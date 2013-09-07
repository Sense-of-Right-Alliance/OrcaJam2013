using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Islander.Screen
{
    enum State
    {
        Uninitialized,
        Initialized,
        Running,
        Finished
    }

    class BaseScreen
    {
        public State CurrentState { get; private set; }

        protected Texture2D background;
        protected ContentManager content;
        protected SpriteBatch spriteBatch;
        protected int width;
        protected int height;

        public BaseScreen()
        {
            CurrentState = State.Uninitialized;
        }

        public void Initialize(ContentManager content, SpriteBatch spriteBatch, int width, int height)
        {
            this.content = content;
            this.spriteBatch = spriteBatch;
            this.width = width;
            this.height = height;

            LoadContent();

            CurrentState = State.Initialized;
        }

        public void StartRunning()
        {
            CurrentState = State.Running;
        }

        public void Reset()
        {
            UnloadContent();
            CurrentState = State.Initialized;
        }

        protected virtual void LoadContent()
        {

        }

        protected virtual void UnloadContent()
        {

        }

        public virtual void Update()
        {
            //if done
            //  CurrentState = State.Finished;
        }

        public virtual void Draw()
        {

        }
    }
}
