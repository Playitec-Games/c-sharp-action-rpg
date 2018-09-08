using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections;

namespace WindowsGame1
{
    // To Hold Terrain Types
    public enum TileTerrain
    { WATER, GRASS }

    class Tile
    {
        protected Texture2D graphic;
        protected int tileWidth = 64;
        protected int tileHeight = 64;
        protected TileTerrain[][] surroundingTerrainType;
        protected Rectangle[][] tilePositions;
        protected TileTerrain terrainType;

        //Constructor
        public Tile()
            : this(null, TileTerrain.WATER)
        {}

        public Tile(Tile tile)
            :this(tile.getGraphic(), tile.getTerrain())
        {}
        public Tile( Texture2D incomingGraphic, TileTerrain incomingTerrainType)
        {
            graphic = incomingGraphic;
            terrainType = incomingTerrainType;

            //Default Tile Positions
            tilePositions = new Rectangle[][]{
                new Rectangle[]{ 
                    new Rectangle(tileWidth/2,tileHeight/2,tileWidth/2,tileHeight/2),
                    new Rectangle(tileWidth/2,tileHeight,tileWidth/2,tileHeight/2)
                },
                new Rectangle[]{ 
                    new Rectangle(tileWidth,tileHeight/2,tileWidth/2,tileHeight/2),
                    new Rectangle(tileWidth,tileHeight,tileWidth/2,tileHeight/2)
                }
            };

            //Default Terrain Types
            surroundingTerrainType = new TileTerrain[][]{
                new TileTerrain[]{new TileTerrain(),new TileTerrain(),new TileTerrain()},
                new TileTerrain[]{new TileTerrain(),terrainType,new TileTerrain()},
                new TileTerrain[]{new TileTerrain(),new TileTerrain(),new TileTerrain()}
            };
        }

        //Accessors
        public Texture2D getGraphic()
        {
            return graphic;
        }

        public TileTerrain getTerrain()
        {
            TileTerrain outgoingTerrain = new TileTerrain();
            outgoingTerrain = terrainType;
            return terrainType;
        }

        //Smart Accessors
        //Set values in this switch statement to change terrain passability
        public bool playerPassable()
        {
            switch (terrainType)
            { 
                case TileTerrain.WATER:
                    return false;

                case TileTerrain.GRASS:
                    return true;

                default:
                    return false;
            }
        }

        //Returns the opposite of the above function
        public bool playerUnpassable()
        {
            return !playerPassable();
        }

        //Updates tiles
        public void update(Tile[][] incomingMap, int incomingPosX, int incomingPosY)
        {
            //First, read in surrounding terrain data
            for (int j = -1; j <= 1; j++) // 3 rows
            {
                for (int i = -1; i <= 1; i++) // 3 columns
                    {
                    if ((incomingPosX + 1 < incomingMap[0].Length) &&
                        (incomingPosX - 1 >= 0) &&
                        (incomingPosY + 1 < incomingMap.Length) &&
                        (incomingPosY - 1 >= 0))// ensure you remain the the bounds of incomingMap
                    {
                        // This grabs the terrain data
                        surroundingTerrainType[j+1][i+1] = incomingMap[incomingPosY + j][incomingPosX + i].getTerrain();
                    }
                    else
                    {
                        // This sets the Terrain type to the center type by default
                        surroundingTerrainType[j+1][i+1] = terrainType; // assume the same tile if out of bounds
                    }
                }
            }
            // At this point, surroundingTerrainType contains this tile's TileTerrain at location [1][1]

            // Second, create rectangles that contain the quarter-tile positions based on surroundings
            // This section of code needs optimized before final release
            switch (terrainType)
            { 
                case TileTerrain.GRASS: // blending routine for grass
                    //if no water is touching this tile
                    if ((surroundingTerrainType[0][1] != TileTerrain.WATER) && //top
                        (surroundingTerrainType[1][0] != TileTerrain.WATER) && //left
                        (surroundingTerrainType[1][2] != TileTerrain.WATER) && //right
                        (surroundingTerrainType[2][1] != TileTerrain.WATER)) //bottom
                    {
                        tilePositions[0][0] = new Rectangle(32, 32, 32, 32);
                        tilePositions[0][1] = new Rectangle(64, 32, 32, 32);
                        tilePositions[1][0] = new Rectangle(32, 64, 32, 32);
                        tilePositions[1][1] = new Rectangle(64, 64, 32, 32);
                    }
                    //If water exists above and to the left
                    else if ((surroundingTerrainType[0][1] == TileTerrain.WATER) && //top
                        (surroundingTerrainType[1][0] == TileTerrain.WATER) && //left
                        (surroundingTerrainType[1][2] != TileTerrain.WATER) && //right
                        (surroundingTerrainType[2][1] != TileTerrain.WATER)) // bottom
                    {
                        tilePositions[0][0] = new Rectangle(0, 0, 32, 32);
                        tilePositions[0][1] = new Rectangle(32, 0, 32, 32);
                        tilePositions[1][0] = new Rectangle(0, 32, 32, 32);
                        tilePositions[1][1] = new Rectangle(32, 32, 32, 32);
                    }
                    //if water exists above
                    else if ((surroundingTerrainType[0][1] == TileTerrain.WATER) && //top
                        (surroundingTerrainType[1][0] != TileTerrain.WATER) && //left
                        (surroundingTerrainType[1][2] != TileTerrain.WATER) && //right
                        (surroundingTerrainType[2][1] != TileTerrain.WATER)) // bottom
                    {
                        tilePositions[0][0] = new Rectangle(32, 0, 32, 32);
                        tilePositions[0][1] = new Rectangle(32 *2, 0, 32, 32);
                        tilePositions[1][0] = new Rectangle(32, 32, 32, 32);
                        tilePositions[1][1] = new Rectangle(32 *2, 32, 32, 32);
                    }
                    //if water exists above and to the right
                    else if ((surroundingTerrainType[0][1] == TileTerrain.WATER) && //top
                        (surroundingTerrainType[1][0] != TileTerrain.WATER) && //left
                        (surroundingTerrainType[1][2] == TileTerrain.WATER) && //right
                        (surroundingTerrainType[2][1] != TileTerrain.WATER)) //bottom
                    {
                        tilePositions[0][0] = new Rectangle(32 * 2, 0, 32, 32);
                        tilePositions[0][1] = new Rectangle(32 * 3, 0, 32, 32);
                        tilePositions[1][0] = new Rectangle(32 * 2, 32, 32, 32);
                        tilePositions[1][1] = new Rectangle(32 * 3, 32, 32, 32);
                    }
                    //if water exists to the left
                    else if ((surroundingTerrainType[0][1] != TileTerrain.WATER) && //top
                        (surroundingTerrainType[1][0] == TileTerrain.WATER) && //left
                        (surroundingTerrainType[1][2] != TileTerrain.WATER) && //right
                        (surroundingTerrainType[2][1] != TileTerrain.WATER)) //bottom
                    {
                        tilePositions[0][0] = new Rectangle(0, 32, 32, 32);
                        tilePositions[0][1] = new Rectangle(32, 32*2, 32, 32);
                        tilePositions[1][0] = new Rectangle(0, 32, 32, 32);
                        tilePositions[1][1] = new Rectangle(32, 32*2, 32, 32);
                    }                    
                    //if water exists to the right
                    else if ((surroundingTerrainType[0][1] != TileTerrain.WATER) && //top
                        (surroundingTerrainType[1][0] != TileTerrain.WATER) && //left
                        (surroundingTerrainType[1][2] == TileTerrain.WATER) && //right
                        (surroundingTerrainType[2][1] != TileTerrain.WATER)) //bottom
                    {
                        tilePositions[0][0] = new Rectangle(32 * 2, 32, 32, 32);
                        tilePositions[0][1] = new Rectangle(32 * 3, 32 * 2, 32, 32);
                        tilePositions[1][0] = new Rectangle(32 * 2, 32, 32, 32);
                        tilePositions[1][1] = new Rectangle(32 * 3, 32 * 2, 32, 32);
                    }
                    //if water exists to the right and left
                    else if ((surroundingTerrainType[0][1] != TileTerrain.WATER) && //top
                        (surroundingTerrainType[1][0] == TileTerrain.WATER) && //left
                        (surroundingTerrainType[1][2] == TileTerrain.WATER) && //right
                        (surroundingTerrainType[2][1] != TileTerrain.WATER)) //bottom
                    {
                        tilePositions[0][0] = new Rectangle(0, 32, 32, 32);
                        tilePositions[0][1] = new Rectangle(32 * 3, 32 * 2, 32, 32);
                        tilePositions[1][0] = new Rectangle(0, 32, 32, 32);
                        tilePositions[1][1] = new Rectangle(32 * 3, 32 * 2, 32, 32);
                    }
                    //If water exists below and to the left
                    else if ((surroundingTerrainType[0][1] != TileTerrain.WATER) && //top
                        (surroundingTerrainType[1][0] == TileTerrain.WATER) && //left
                        (surroundingTerrainType[1][2] != TileTerrain.WATER) && //right
                        (surroundingTerrainType[2][1] == TileTerrain.WATER)) // bottom
                    {
                        tilePositions[0][0] = new Rectangle(0, 32 * 2, 32, 32);
                        tilePositions[0][1] = new Rectangle(32, 32 * 2, 32, 32);
                        tilePositions[1][0] = new Rectangle(0, 32 * 3, 32, 32);
                        tilePositions[1][1] = new Rectangle(32, 32 * 3, 32, 32);
                    }
                    //if water exists below
                    else if ((surroundingTerrainType[0][1] != TileTerrain.WATER) && //top
                        (surroundingTerrainType[1][0] != TileTerrain.WATER) && //left
                        (surroundingTerrainType[1][2] != TileTerrain.WATER) && //right
                        (surroundingTerrainType[2][1] == TileTerrain.WATER)) // bottom
                    {
                        tilePositions[0][0] = new Rectangle(32, 32 * 2, 32, 32);
                        tilePositions[0][1] = new Rectangle(32 * 2, 32 * 2, 32, 32);
                        tilePositions[1][0] = new Rectangle(32, 32 * 3, 32, 32);
                        tilePositions[1][1] = new Rectangle(32 * 2, 32 * 3, 32, 32);
                    }
                    //if water exists below and to the right
                    else if ((surroundingTerrainType[0][1] != TileTerrain.WATER) && //top
                        (surroundingTerrainType[1][0] != TileTerrain.WATER) && //left
                        (surroundingTerrainType[1][2] == TileTerrain.WATER) && //right
                        (surroundingTerrainType[2][1] == TileTerrain.WATER)) //bottom
                    {
                        tilePositions[0][0] = new Rectangle(32 * 2, 32 * 2, 32, 32);
                        tilePositions[0][1] = new Rectangle(32 * 3, 32 * 2, 32, 32);
                        tilePositions[1][0] = new Rectangle(32 * 2, 32 * 3, 32, 32);
                        tilePositions[1][1] = new Rectangle(32 * 3, 32 * 3, 32, 32);
                    }
                    //If water exists above, below and to the left
                    else if ((surroundingTerrainType[0][1] == TileTerrain.WATER) && //top
                        (surroundingTerrainType[1][0] == TileTerrain.WATER) && //left
                        (surroundingTerrainType[1][2] != TileTerrain.WATER) && //right
                        (surroundingTerrainType[2][1] == TileTerrain.WATER)) // bottom
                    {
                        tilePositions[0][0] = new Rectangle(0, 0, 32, 32);
                        tilePositions[0][1] = new Rectangle(32, 0, 32, 32);
                        tilePositions[1][0] = new Rectangle(0, 32 * 3, 32, 32);
                        tilePositions[1][1] = new Rectangle(32, 32 * 3, 32, 32);
                    }
                    //if water exists above and below
                    else if ((surroundingTerrainType[0][1] == TileTerrain.WATER) && //top
                        (surroundingTerrainType[1][0] != TileTerrain.WATER) && //left
                        (surroundingTerrainType[1][2] != TileTerrain.WATER) && //right
                        (surroundingTerrainType[2][1] == TileTerrain.WATER)) // bottom
                    {
                        tilePositions[0][0] = new Rectangle(32, 0, 32, 32);
                        tilePositions[0][1] = new Rectangle(32 * 2, 0, 32, 32);
                        tilePositions[1][0] = new Rectangle(32, 32 * 3, 32, 32);
                        tilePositions[1][1] = new Rectangle(32 * 2, 32 * 3, 32, 32);
                    }
                    //if water exists above, below and to the right
                    else if ((surroundingTerrainType[0][1] == TileTerrain.WATER) && //top
                        (surroundingTerrainType[1][0] != TileTerrain.WATER) && //left
                        (surroundingTerrainType[1][2] == TileTerrain.WATER) && //right
                        (surroundingTerrainType[2][1] == TileTerrain.WATER)) //bottom
                    {
                        tilePositions[0][0] = new Rectangle(32 * 2, 0, 32, 32);
                        tilePositions[0][1] = new Rectangle(32 * 3, 0, 32, 32);
                        tilePositions[1][0] = new Rectangle(32 * 2, 32 * 3, 32, 32);
                        tilePositions[1][1] = new Rectangle(32 * 3, 32 * 3, 32, 32);
                    }

                    break;
                default: // do nothing by default
                    break;
            }

            // Third, do other stuff
        }


        //Draws the tile to the screen
        public void draw(SpriteBatch incomingSpriteBatch,SpriteFont incomingSpriteFont, Vector2 incomingDrawPosition)
        {
            //NORMAL DRAWING
            if (true)
            {
                for (int j = 0; j < 2; j++)
                    for (int i = 0; i < 2; i++)
                    {
                        Vector2 drawPosition = new Vector2(incomingDrawPosition.X + (float)(i * tileWidth / 2), incomingDrawPosition.Y + (float)(j * tileHeight / 2));
                        incomingSpriteBatch.Draw
                            (
                                graphic,
                                drawPosition,  // Draw Position
                                tilePositions[j][i], // sections of tile sheet to use
                                Color.White,
                                0.0f, // Rotation
                                new Vector2(0, 0), // Origin
                                new Vector2(1.0f, 1.0f), // Scale
                                new SpriteEffects(), // no sprite effects
                                (1.0f) // layer 1 is back layer
                            );
                    }
            }

            /*
            //FOR DEBUG: Draw surroundingTerrainType data
            if (false)
            {
                String outputTerrain = new String(' ', 1);
                Color fontColor = new Color(0, 0, 0);
                for (int j = 0; j < 3; j++)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        switch (surroundingTerrainType[j][i])
                        {
                            case TileTerrain.WATER:
                                outputTerrain = "^";
                                fontColor = Color.Blue;
                                break;
                            case TileTerrain.GRASS:
                                outputTerrain = "#";
                                fontColor = Color.DarkGreen;
                                break;
                            default:
                                break;
                        }
                        Vector2 drawPosition = new Vector2(incomingDrawPosition.X + (float)(i * tileWidth / 3), incomingDrawPosition.Y + (float)(j * tileHeight / 3) - 3);
                        incomingSpriteBatch.DrawString(
                            incomingSpriteFont,
                            outputTerrain,
                            drawPosition,
                            fontColor,
                            0.0f,
                            new Vector2(0, 0),
                            new Vector2(1.0f, 1.0f),
                            new SpriteEffects(), // no sprite effects
                            (0.99f) // layer 1 is back layer;
                        );
                    }
                }
            }
            */
        }

        ~Tile()
        { }
    }
}
