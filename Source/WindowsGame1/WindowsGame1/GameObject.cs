using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1
{
    class GameObject
    {
        protected Vector2 position = new Vector2(0,0);
        protected Vector2 drawOffset = new Vector2(0, 0);

        protected Texture2D graphic;

        protected float zPosition = 0;

        protected float topCollision = 0;
        protected float bottomCollision = 0;
        protected float leftCollision = 0;
        protected float rightCollision = 0;

        protected int maxHP = 100;
        protected int curHP;

        protected int damageCounter = 0;

        public GameObject()
        {
            curHP = maxHP;
        }

        public GameObject( Vector2 incomingPosition, Texture2D incomingGraphic, Vector2 incomingDimensions )
        {
            position = incomingPosition;
            graphic = incomingGraphic;

            leftCollision = 0;
            rightCollision = incomingDimensions.X;
            topCollision = incomingDimensions.Y/2;
            bottomCollision = incomingDimensions.Y;

            curHP = maxHP;
        }

        public GameObject(Vector2 incomingPosition, Texture2D incomingGraphic, float incomingLeftCollision, float incomingRightCollision, float incomingTopCollision, float incomingBottomCollision)
        {
            position = incomingPosition;
            graphic = incomingGraphic;

            leftCollision = incomingLeftCollision; 
            rightCollision = incomingRightCollision; 
            topCollision = incomingTopCollision; 
            bottomCollision = incomingBottomCollision;

            curHP = maxHP;
        }

        protected Texture2D setGraphic(Texture2D incomingGraphic)
        {
            return graphic = incomingGraphic;
        }

        public virtual void update(GameTime incomingGameTime, Chunk incomingChunk)
        {
            if (damageCounter > 0)
            {
                --damageCounter;
            }
            if (curHP <= 0)
            {
                //DIE!!!
            }
        
        }

        public virtual void draw(SpriteBatch incomingSpriteBatch, SpriteFont incomingSpriteFont, Vector2 incomingDrawPosition)
        {
            /*incomingSpriteBatch.Draw // this will draw a shadow in the collision box
                (
                    graphic,
                    getCollisionRectangle(incomingDrawPosition),                    
                    new Rectangle(0,0,graphic.Width,graphic.Height),
                    Color.Black,
                    0.0f,
                    new Vector2(0,0),
                    new SpriteEffects(),
                    0.9f // layer .9
                );
             */

            Color drawColor = Color.White;

            if (damageCounter % 2 == 1)
            {
                drawColor = Color.Blue;
            }

            incomingSpriteBatch.Draw // this will draw the actual graphic
                (
                    graphic, // the stored graphic
                    getDrawRectangle(incomingDrawPosition), // offset by draw position
                    new Rectangle(0,0,graphic.Width,graphic.Height), // use whole image
                    drawColor, // no color blending
                    0.0f, // no rotation
                    new Vector2(graphic.Width/2,graphic.Height/2), // origin
                    new SpriteEffects(), // no sprite effects
                    (float)(1 - (0.001 * (position.Y + topCollision))) //
                );
        }

        // Accessors used to check collision for current position
        public Vector2 getPosition()
        {
            return position;
        }

        public Rectangle getCollisionRectangle()
        {
            return new Rectangle((int)(position.X + leftCollision), (int)(position.Y + topCollision), (int)(rightCollision - leftCollision), (int)(bottomCollision - topCollision));
        }

        public Vector2 TopLeftCorner()
        {
            return new Vector2(position.X + leftCollision, position.Y + topCollision);
        }

        public Vector2 TopRightCorner()
        {
            return new Vector2(position.X + rightCollision, position.Y + topCollision);
        }

        public Vector2 BottomLeftCorner()
        {
            return new Vector2(position.X + leftCollision, position.Y + bottomCollision);
        }

        public Vector2 BottomRightCorner()
        {
            return new Vector2(position.X + rightCollision, position.Y + bottomCollision);
        }

        // Accessors used to check collision for possible positions (for movement and teleportation)
        public bool getCollision( Vector2 incomingPosition )
        {
            if 
                (
                    (incomingPosition.X >= (position.X + leftCollision)) && 
                    (incomingPosition.X <= (position.X + rightCollision)) &&
                    (incomingPosition.Y >= (position.Y + topCollision)) &&
                    (incomingPosition.Y <= (position.Y + bottomCollision))
                )
            { return true; }
            else
            { return false; }
        }

        public Rectangle getCollisionRectangle( Vector2 incomingOffset )
        {
            return new Rectangle((int)(position.X + leftCollision + incomingOffset.X), (int)(position.Y + topCollision + incomingOffset.Y), (int)(rightCollision - leftCollision), (int)(bottomCollision - topCollision));
        }

        protected Vector2 TopLeftCorner( Vector2 incomingPosition)
        {
            return new Vector2(incomingPosition.X + leftCollision, incomingPosition.Y + topCollision);
        }

        protected Vector2 TopRightCorner( Vector2 incomingPosition)
        {
            return new Vector2(incomingPosition.X + rightCollision, incomingPosition.Y + topCollision);
        }

        protected Vector2 BottomLeftCorner( Vector2 incomingPosition )
        {
            return new Vector2(incomingPosition.X + leftCollision, incomingPosition.Y + bottomCollision);
        }

        protected Vector2 BottomRightCorner( Vector2 incomingPosition )
        {
            return new Vector2(incomingPosition.X + rightCollision, incomingPosition.Y + bottomCollision);
        }

        // Accressors used to get drawing data
        public Rectangle getDrawRectangle()
        {
            return new Rectangle((int)position.X, (int)position.Y, graphic.Width, graphic.Height);
        }

        public Rectangle getDrawRectangle( Vector2 incomingOffset )
        {
            return new Rectangle((int)position.X + (int)incomingOffset.X, (int)position.Y + (int)incomingOffset.Y, graphic.Width, graphic.Height);
        }

        //Mutators
        public virtual void takeDamage(int incomingDamage, Chunk incomingChunk, GameTime incomingGameTime)
        {
            if (damageCounter <= 0)
            {
                damageCounter = 32;
                curHP -= incomingDamage;
            }
        }

        ~GameObject()
        {
            
        }
    }
}
