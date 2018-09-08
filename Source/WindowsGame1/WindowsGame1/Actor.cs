using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame1
{
    class Actor : GameObject
    {
        //Actors always face a direction
        protected enum direction : byte { UP, DOWN, LEFT, RIGHT, NONE };
        protected direction walking = direction.DOWN;
        protected direction facing = direction.DOWN;

        //internal counters
        protected int movementCounter = 0;
        protected float knockback;


        //Constructors
        public Actor()
            : base()
        { }

        public Actor(Vector2 incomingPosition, Texture2D incomingGraphic, Vector2 incomingDimensions)
            : base(incomingPosition, incomingGraphic, incomingDimensions)
        { }

        public Actor(Vector2 incomingPosition, Texture2D incomingGraphic, float incomingLeftCollision, float incomingRightCollision, float incomingTopCollision, float incomingBottomCollision)
            : base(incomingPosition, incomingGraphic, incomingLeftCollision, incomingRightCollision, incomingTopCollision, incomingBottomCollision)
        { }

        //Update Logic
        public virtual void update(GameTime incomingGameTime, Chunk incomingChunk)
        {
            base.update(incomingGameTime, incomingChunk);

            if (movementCounter > 0)
            {
                switch (walking)
                {
                    case direction.UP:
                        move(incomingChunk, new Vector2(position.X, position.Y + knockback), incomingGameTime);
                        break;
                    case direction.DOWN:
                        move(incomingChunk, new Vector2(position.X, position.Y - knockback), incomingGameTime);
                        break;
                    case direction.LEFT:
                        move(incomingChunk, new Vector2(position.X + knockback, position.Y), incomingGameTime);
                        break;
                    case direction.RIGHT:
                        move(incomingChunk, new Vector2(position.X - knockback, position.Y), incomingGameTime);
                        break;
                }
                --movementCounter;
            }
        }

        protected bool checkPassable(Chunk incomingChunk, Vector2 incomingPosition, GameTime incomingGameTime)
        {
            return (incomingChunk.isPassable(TopLeftCorner(incomingPosition), this, incomingGameTime) &&
                incomingChunk.isPassable(TopRightCorner(incomingPosition), this, incomingGameTime) &&
                incomingChunk.isPassable(BottomLeftCorner(incomingPosition), this, incomingGameTime) &&
                incomingChunk.isPassable(BottomRightCorner(incomingPosition), this, incomingGameTime)
                );
        }

        protected void move(Chunk incomingChunk, Vector2 incomingPosition, GameTime incomingGameTime)
        {
            if (true)
            {
                //check new position for passability move player if passable
                if (checkPassable(incomingChunk, incomingPosition, incomingGameTime))
                {
                    position = incomingPosition;
                }
                else // if position is not open, then do wall handling
                {
                    // if collision exists vertically, move only horizontally
                    if (checkPassable(incomingChunk, new Vector2(position.X, incomingPosition.Y), incomingGameTime))
                    {
                        if (position.Y > incomingPosition.Y)
                        { walking = direction.DOWN; }
                        else if (position.Y < incomingPosition.Y)
                        { walking = direction.UP; }

                        position.Y = incomingPosition.Y;
                    }

                    // if collision exists horizontally, move only vertically
                    else if (checkPassable(incomingChunk, new Vector2(incomingPosition.X, position.Y), incomingGameTime))
                    {
                        if (position.X > incomingPosition.X)
                        { walking = direction.RIGHT; }
                        else if (position.X < incomingPosition.X)
                        { walking = direction.LEFT; }

                        position.X = incomingPosition.X;
                    }

                    //enable smooth movement around corners by checking for collisions on individual sides
                    switch (walking)
                    {
                        case direction.UP:
                            if (position.X == incomingPosition.X)
                            {
                                if (incomingChunk.isPassable(TopLeftCorner(incomingPosition), this, incomingGameTime) && incomingChunk.playerUnpassable(TopRightCorner(incomingPosition), this, incomingGameTime))
                                { position.X += (incomingPosition.Y - position.Y); }
                                else if (incomingChunk.playerUnpassable(TopLeftCorner(incomingPosition), this, incomingGameTime) && incomingChunk.isPassable(TopRightCorner(incomingPosition), this, incomingGameTime))
                                { position.X -= (incomingPosition.Y - position.Y); }
                            }
                            break;
                        case direction.DOWN:
                            if (position.X == incomingPosition.X)
                            {
                                if (incomingChunk.isPassable(BottomLeftCorner(incomingPosition), this, incomingGameTime) && incomingChunk.playerUnpassable(BottomRightCorner(incomingPosition), this, incomingGameTime))
                                { position.X -= (incomingPosition.Y - position.Y); }
                                else if (incomingChunk.playerUnpassable(BottomLeftCorner(incomingPosition), this, incomingGameTime) && incomingChunk.isPassable(BottomRightCorner(incomingPosition), this, incomingGameTime))
                                { position.X += (incomingPosition.Y - position.Y); }
                            }
                            break;
                        case direction.LEFT:
                            if (position.Y == incomingPosition.Y)
                            {
                                if (incomingChunk.isPassable(TopLeftCorner(incomingPosition), this, incomingGameTime) && incomingChunk.playerUnpassable(BottomLeftCorner(incomingPosition), this, incomingGameTime))
                                { position.Y += (incomingPosition.X - position.X); }
                                else if (incomingChunk.playerUnpassable(TopLeftCorner(incomingPosition), this, incomingGameTime) && incomingChunk.isPassable(BottomLeftCorner(incomingPosition), this, incomingGameTime))
                                { position.Y -= (incomingPosition.X - position.X); }
                            }
                            break;
                        case direction.RIGHT:
                            if (position.Y == incomingPosition.Y)
                            {
                                if (incomingChunk.isPassable(TopRightCorner(incomingPosition), this, incomingGameTime) && incomingChunk.playerUnpassable(BottomRightCorner(incomingPosition), this, incomingGameTime))
                                { position.Y -= (incomingPosition.X - position.X); }
                                else if (incomingChunk.playerUnpassable(TopRightCorner(incomingPosition), this, incomingGameTime) && incomingChunk.isPassable(BottomRightCorner(incomingPosition), this, incomingGameTime))
                                { position.Y += (incomingPosition.X - position.X); }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public override void takeDamage(int incomingDamage, Chunk incomingChunk, GameTime incomingGameTime)
        {
            base.takeDamage(incomingDamage, incomingChunk, incomingGameTime);

            knockback = (float)incomingDamage * incomingGameTime.ElapsedGameTime.Milliseconds / 12;

            if (movementCounter <= 0)
            {
                movementCounter = 12;
            }
        }
         ~Actor()
        { }
    }
}
