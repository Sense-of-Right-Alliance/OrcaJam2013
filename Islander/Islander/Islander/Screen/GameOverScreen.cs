using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Islander.Screen
{
    class GameOverScreen : MenuScreen
    {

        public override void Draw()
        {
            base.Draw();

            DrawString("Game Over");
        }
    }
}
