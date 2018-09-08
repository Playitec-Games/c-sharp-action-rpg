using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections;

namespace WindowsGame1
{
    public enum CollissionType
    { TERRAIN, DOODAD, BADDIE }

    class Chunk
    {
        private int[][] chunkData;
        private Tile[][] map;
        private Tile[] tileSet;
        private int tileWidth = 64;
        private int tileHeight = 64;

        private ArrayList doodads = new ArrayList();
        private ArrayList players = new ArrayList();
        private ArrayList baddies = new ArrayList();

        //constructor
        public Chunk(int[][] incomingData, Tile[] incomingTileSet)
        {
            tileSet = incomingTileSet;

            chunkData = incomingData;
            map = new Tile[incomingData.Length][];

            // load all tile
            for (int j = 0; j < incomingData.Length; j++)
            {
                map[j] = new Tile[ incomingData[j].Length ];
                for (int i = 0; i < incomingData[j].Length; i++)
                {
                    switch (incomingData[j][i])
                    { 
                        case 00:
                            //map[j][i] = tileSet[0];
                            map[j][i] = new Tile(tileSet[0].getGraphic(), tileSet[0].getTerrain()); // Water
                            break;
                        case 01:
                            //map[j][i] = tileSet[1];
                            map[j][i] = new Tile(tileSet[1].getGraphic(), tileSet[1].getTerrain()); // Grass
                            break;
                        default:
                            map[j][i] = new Tile(tileSet[0].getGraphic(), tileSet[0].getTerrain()); // Default Water
                            break;
                    }
                }
            }

            //update all tiles
            for (int j = 0; j < map.Length; j++)
            {
                for (int i = 0; i < map[0].Length; i++)
                {
                    map[j][i].update(map, i, j);
                }
            }

        }

        //Accessors
        public Tile getTile(int x, int y)
        {
            if (
                (chunkData.Length > (y / tileHeight)) && 
                (chunkData[0].Length > (x / tileHeight)) && 
                (x >= 0) && 
                (y >= 0))
            {
                return map[y/tileHeight][x/tileWidth];
            }

            return tileSet[0];
        }

        public Tile getTile(Vector2 incomingPosition)
        { 
            return getTile((int)(incomingPosition.X), (int)(incomingPosition.Y));
        }

        public int getTileWidth()
        { return tileWidth;  }
        public int getTileHeight()
        { return tileHeight; }
        public int getWidth()
        { return map[0].Length * tileWidth; }
        public int getHeight()
        { return map.Length * tileHeight; }

        public bool isPassable(Vector2 incomingPosition, GameObject incomingGameObject, GameTime incomingGameTime)
        {
            if (getTile(incomingPosition).playerUnpassable())
            { return false; }
            foreach (Doodad doodad in doodads)
            {
                if (doodad.getCollision(incomingPosition) && doodad != incomingGameObject)
                { return false; }
            }
            /*
            foreach (Player player in players)
            {
                if (player.getCollision(incomingPosition) && player != incomingGameObject)
                {
                    return false; 
                }
            }
            foreach (Baddie baddie in baddies)
            {
                if (baddie.getCollision(incomingPosition) && baddie != incomingGameObject)
                { return false; }
            }
            */
            return true;
        }

        public bool playerUnpassable(Vector2 incomingPosition, GameObject incomingGameObject, GameTime incomingGameTime)
        {
            return !(isPassable((incomingPosition), incomingGameObject, incomingGameTime));
        }

        //Mutators
        public void placeDoodad(Doodad incomingDoodad)
        {
            doodads.Add(incomingDoodad);
        }

        public void placePlayer(Player incomingPlayer)
        {
            players.Add(incomingPlayer);
        }

        public void placeBaddie(Baddie incomingBaddie)
        {
            baddies.Add(incomingBaddie);
        }

        //Update
        public void update(GameTime incomingGameTime)
        {
            
            for (int j = 0; j < map.Length; j++)
            {
                for (int i = 0; i < map[j].Length; i++)
                {
                    map[j][i].update(map, i, j);
                }
            }

            foreach (Baddie baddie in baddies)
            {
                baddie.update(incomingGameTime, this);

                Vector2 topLeft = baddie.TopLeftCorner() + new Vector2(-1,-1);
                Vector2 topRight = baddie.TopRightCorner() + new Vector2(1, -1);
                Vector2 bottomLeft = baddie.BottomLeftCorner() + new Vector2(-1, 1);
                Vector2 bottomRight = baddie.BottomRightCorner() + new Vector2(1, 1);
                foreach (Player player in players)
                {
                    if (
                        player.getCollision(topLeft) ||
                        player.getCollision(topRight) ||
                        player.getCollision(bottomLeft) ||
                        player.getCollision(bottomRight) 
                        )
                    {
                        player.takeDamage(5, this, incomingGameTime);
                    }
                }
            }

            foreach (Player player in players)
            {
                player.update(incomingGameTime, this);
            }
        }

        //Draw
        public void draw(SpriteBatch incomingSpriteBatch, SpriteFont incomingSpriteFont, Vector2 incomingDrawPosition)
        {
            // draw tiles first
            for (int j = 0; j < map.Length; j++)
            {
                for (int i = 0; i < map[0].Length; i++)
                {
                    Vector2 drawPosition = new Vector2(incomingDrawPosition.X + (tileWidth * i), incomingDrawPosition.Y + (tileHeight * j));                    
                    map[j][i].draw(incomingSpriteBatch, incomingSpriteFont, drawPosition);
                }
            }

            // draw doodads second
            foreach (Doodad doodad in doodads)
            {
                doodad.draw(incomingSpriteBatch, incomingSpriteFont, incomingDrawPosition);
            }

            // draw actors third
            foreach (Baddie baddie in baddies)
            {
                baddie.draw(incomingSpriteBatch, incomingSpriteFont, incomingDrawPosition);
            }
            foreach (Player player in players)
            {
                player.draw(incomingSpriteBatch, incomingSpriteFont, incomingDrawPosition);
            }

            // draw effects fourth
        }

        ~Chunk()
        {
            doodads.Clear();
            players.Clear();
            baddies.Clear();
        }
    }
}
