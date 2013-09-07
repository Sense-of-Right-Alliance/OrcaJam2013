using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Islander.Objects
{
    class Island : Entity
    {
        public enum Colour
        {
            Blue,
            Yellow,
            Red,
            Green
        }

        public Colour Colour { get; protected set; }
    }
}
