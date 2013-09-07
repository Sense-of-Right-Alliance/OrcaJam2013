using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Islander.Screen
{
    class GameScreen : BaseScreen
    {




        protected override void HandleInput()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                CurrentState = State.Finished;
        }
    }
}
