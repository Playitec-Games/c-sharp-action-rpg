using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1
{
    class Doodad : GameObject
    {
        public Doodad()
        { }

        public Doodad(Vector2 incomingPosition, Texture2D incomingGraphic, Vector2 incomingDimensions)
            : base(incomingPosition, incomingGraphic, incomingDimensions)
        { }

        public Doodad(Vector2 incomingPosition, Texture2D incomingGraphic, float incomingLeftCollision, float incomingRightCollision, float incomingTopCollision, float incomingBottomCollision)
            : base(incomingPosition, incomingGraphic, incomingLeftCollision, incomingRightCollision, incomingTopCollision, incomingBottomCollision)
        { }

        ~Doodad()
        { }
    }
}
