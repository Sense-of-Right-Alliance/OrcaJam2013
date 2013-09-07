using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Islander.Screen
{
    class MainMenuScreen : MenuScreen
    {
        public override void Draw()
        {
            base.Draw();

            DrawString("Start");
        }

    }
}
