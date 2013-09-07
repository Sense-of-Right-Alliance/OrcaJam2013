using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Islander.Screen
{
    class MenuScreen : BaseScreen
    {

        SpriteFont font;



        protected override void LoadContent()
        {
            base.LoadContent();

            font = content.Load<SpriteFont>("Menu");
        }

        public override void Draw()
        {
            base.Draw();
            //stupid comment

        }

        protected void DrawString(string text)
        {
            Vector2 textDimensions = font.MeasureString(text);
            spriteBatch.DrawString(font, text, new Vector2(width/2 - textDimensions.X, height/2 - textDimensions.Y), Color.White);
        }

    }
}
