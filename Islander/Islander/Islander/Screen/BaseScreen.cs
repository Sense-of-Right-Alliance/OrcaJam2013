using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Islander.Screen
{
    class BaseScreen
    {
        public enum State
        {
            None,
            Running,
            Finished
        }


        protected Texture2D background;
        protected ContentManager Content;
        protected SpriteBatch SpriteBatch;
        protected int Width;
        protected int Height;
        bool initialized = false;

        public BaseScreen()
        {

        }

        public void Initialize(ContentManager content, SpriteBatch spriteBatch, int width, int height)
        {
            initialized = true;
            Content = content;
            SpriteBatch = spriteBatch;
            Width = width;
            Height = height;

            LoadContent();
        }

        protected virtual void LoadContent()
        {

        }

        protected virtual void UnloadContent()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void Draw()
        {

        }
    }
}
