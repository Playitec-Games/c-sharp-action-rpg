using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1
{
    class Baddie : Actor
    {
        public Baddie()
            : base()
        { }

        public Baddie(Vector2 incomingPosition, Texture2D incomingGraphic, Vector2 incomingDimensions)
            : base(incomingPosition, incomingGraphic, incomingDimensions)
        { }

        public Baddie(Vector2 incomingPosition, Texture2D incomingGraphic, float incomingLeftCollision, float incomingRightCollision, float incomingTopCollision, float incomingBottomCollision)
            : base(incomingPosition, incomingGraphic, incomingLeftCollision, incomingRightCollision, incomingTopCollision, incomingBottomCollision)
        { }

        public override void update(GameTime incomingGameTime, Chunk incomingChunk)
        {
            Vector2 newPosition = position;
            Vector2 oldPosition = position;

            //randomly change direction
            Random r = new Random();
            switch (r.Next(0, 750))
            {
                case 10:
                    walking = direction.UP;
                    break;
                case 20:
                    walking = direction.DOWN;
                    break;
                case 30:
                    walking = direction.LEFT;
                    break;
                case 40:
                    walking = direction.RIGHT;
                    break;
                default:
                    break;
            }

            //advance baddie in current direction
            switch (walking)
            {
                case direction.UP:
                    newPosition.Y -= 1.0f * incomingGameTime.ElapsedGameTime.Milliseconds / 12;
                    break;
                case direction.DOWN:
                    newPosition.Y += 1.0f * incomingGameTime.ElapsedGameTime.Milliseconds / 12;
                    break;
                case direction.LEFT:
                    newPosition.X -= 1.0f * incomingGameTime.ElapsedGameTime.Milliseconds / 12;
                    break;
                case direction.RIGHT:
                    newPosition.X += 1.0f * incomingGameTime.ElapsedGameTime.Milliseconds / 12;
                    break;
                default:
                    break;
            }

            //move baddie to new position
            move(incomingChunk, newPosition, incomingGameTime);

            
            if (oldPosition == position)
            {
                switch (walking)
                {
                    case direction.UP:
                        walking = direction.RIGHT;
                        break;
                    case direction.DOWN:
                        walking = direction.LEFT;
                        break;
                    case direction.LEFT:
                        walking = direction.UP;
                        break;
                    case direction.RIGHT:
                        walking = direction.DOWN;
                        break;
                    default:
                        break;
                }
            }
            

        }

        ~Baddie()
        { }
    }
}
