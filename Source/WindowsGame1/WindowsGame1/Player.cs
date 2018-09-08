using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1
{
    // This class will encompass all functionality of player characters
    class Player : Actor
    {
        PlayerIndex index;

        //Constructors
        public Player()
            : base()
        { }

        public Player(Vector2 incomingPosition, Texture2D incomingGraphic, Vector2 incomingDimensions, PlayerIndex incomingIndex)
            : base(incomingPosition, incomingGraphic, incomingDimensions)
        { index = incomingIndex;  }

        public Player(Vector2 incomingPosition, Texture2D incomingGraphic, float incomingLeftCollision, float incomingRightCollision, float incomingTopCollision, float incomingBottomCollision, PlayerIndex incomingIndex)
            : base(incomingPosition, incomingGraphic, incomingLeftCollision, incomingRightCollision, incomingTopCollision, incomingBottomCollision)
        { index = incomingIndex; }

        //Update Functions
        //Players always update first, so the world can update around them based on their actions. Thus, they need their own update function
        public override void update(GameTime incomingGameTime, Chunk incomingChunk)
        {
            base.update(incomingGameTime, incomingChunk);

            if (movementCounter <= 0)
            {
                //initialize
                Vector2 newPosition = grabInput(incomingGameTime); // collect user input

                //move
                move(incomingChunk, newPosition, incomingGameTime); // send command for player to move to position
            }
        }

        private Vector2 grabInput(GameTime incomingGameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            GamePadState padState = GamePad.GetState(index);

            Vector2 newPosition = position;

            if (keyState.IsKeyDown(Keys.Left) && keyState.IsKeyUp(Keys.Right))
            {
                newPosition.X -= 1.0f * incomingGameTime.ElapsedGameTime.Milliseconds / 6;
                walking = direction.LEFT;
            }
            else if (keyState.IsKeyDown(Keys.Right) && keyState.IsKeyUp(Keys.Left))
            {
                newPosition.X += 1.0f * incomingGameTime.ElapsedGameTime.Milliseconds / 6;
                walking = direction.RIGHT;
            }

            if (keyState.IsKeyDown(Keys.Up) && keyState.IsKeyUp(Keys.Down))
            {
                newPosition.Y -= 1.0f * incomingGameTime.ElapsedGameTime.Milliseconds / 6;
                walking = direction.UP;
            }
            else if (keyState.IsKeyDown(Keys.Down) && keyState.IsKeyUp(Keys.Up))
            {
                newPosition.Y += 1.0f * incomingGameTime.ElapsedGameTime.Milliseconds / 6;
                walking = direction.DOWN;
            }

            return newPosition;
        }

        public override void draw(SpriteBatch incomingSpriteBatch, SpriteFont incomingSpriteFont, Vector2 incomingDrawPosition)
        {
            base.draw(incomingSpriteBatch, incomingSpriteFont, incomingDrawPosition);
            
            incomingSpriteBatch.DrawString(
                        incomingSpriteFont,
                        "HP:" + curHP,
                        new Vector2(16,16),
                        Color.Red,
                        0.0f,
                        new Vector2(0, 0),
                        new Vector2(1.0f, 1.0f),
                        new SpriteEffects(), // no sprite effects
                        (0.99f) // layer 1 is back layer;
                    );
        }


        //Destructor
        ~Player()
        { }
    }
}
