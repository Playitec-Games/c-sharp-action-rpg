using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace WindowsGame1
{
    class World
    {
        Chunk chunk;
        Player player1;

        public World(ContentManager incomingContent)
        {
            Tile[] tileSet = new Tile[]
            {
               new Tile( incomingContent.Load<Texture2D>("tiles/water"), TileTerrain.WATER ), //00
               new Tile( incomingContent.Load<Texture2D>("tiles/grass"), TileTerrain.GRASS )  //01
            };


            int[][] chunkData = new int[][]
            {
                new int[]{ 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00},
                new int[]{ 00, 00, 00, 00, 00, 00, 01, 01, 01, 00, 00, 00, 00},
                new int[]{ 00, 00, 00, 00, 01, 01, 01, 01, 01, 00, 00, 00, 00},
                new int[]{ 00, 00, 00, 01, 01, 00, 00, 01, 00, 00, 00, 00, 00},
                new int[]{ 00, 00, 00, 01, 01, 00, 00, 01, 00, 00, 00, 00, 00},
                new int[]{ 00, 01, 01, 01, 01, 01, 01, 01, 00, 00, 01, 01, 00},
                new int[]{ 00, 00, 00, 01, 01, 01, 01, 01, 01, 01, 01, 01, 00},
                new int[]{ 00, 00, 00, 01, 01, 01, 01, 01, 01, 01, 01, 01, 00},
                new int[]{ 00, 00, 00, 00, 00, 00, 01, 01, 01, 01, 01, 01, 00},
                new int[]{ 00, 00, 00, 01, 01, 01, 01, 01, 01, 01, 01, 01, 00},
                new int[]{ 00, 00, 00, 00, 00, 01, 01, 01, 00, 01, 01, 01, 00},
                new int[]{ 00, 00, 00, 00, 00, 01, 01, 01, 00, 01, 01, 01, 00},
                new int[]{ 00, 00, 00, 00, 00, 01, 01, 01, 01, 01, 01, 01, 00},
                new int[]{ 00, 00, 00, 00, 00, 00, 00, 00, 01, 01, 01, 01, 00},
                new int[]{ 00, 00, 00, 00, 00, 00, 00, 00, 01, 01, 01, 01, 00},
                new int[]{ 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00},
            }; // World Chunks

            chunk = new Chunk(chunkData, tileSet);

            //initialize doodads
            chunk.placeDoodad(new Doodad(new Vector2(386, 335), incomingContent.Load<Texture2D>("doodads/tree"), -28, 28, 16, 44));
            chunk.placeDoodad(new Doodad(new Vector2(308, 354), incomingContent.Load<Texture2D>("doodads/tree"), -28, 28, 16, 44));
            chunk.placeDoodad(new Doodad(new Vector2(340, 324), incomingContent.Load<Texture2D>("doodads/tree"), -28, 28, 16, 44));
            chunk.placeDoodad(new Doodad(new Vector2(278, 324), incomingContent.Load<Texture2D>("doodads/tree"), -28, 28, 16, 44));
            chunk.placeDoodad(new Doodad(new Vector2(610, 670), incomingContent.Load<Texture2D>("doodads/tree"), -28, 28, 16, 44));
            chunk.placeDoodad(new Doodad(new Vector2(570, 756), incomingContent.Load<Texture2D>("doodads/tree"), -28, 28, 16, 44));

            //initialize monsters
            chunk.placeBaddie(new Baddie(new Vector2(64 * 7, 64 * 7), incomingContent.Load<Texture2D>("baddies/baddie"), -28, 28, 0, 28));

            //initialize player object
            player1 = new Player(new Vector2(64 * 8, 64 * 8), incomingContent.Load<Texture2D>("characters/player"), -28, 28, 0, 28, PlayerIndex.One);
            chunk.placePlayer(player1);
        }


        public void update(GameTime incomingGameTime)
        {
            chunk.update(incomingGameTime);
        }

        public void draw(SpriteBatch incomingSpriteBatch, SpriteFont incomingSpriteFont, GraphicsDeviceManager incomingGraphics)
        {
            //Determine Draw Position
            Vector2 drawPosition = new Vector2(0, 0);
            if (incomingGraphics.PreferredBackBufferWidth < chunk.getWidth())
            {
                if (player1.getPosition().X < incomingGraphics.PreferredBackBufferWidth / 2)
                { drawPosition.X = 0; }
                else if (player1.getPosition().X > (chunk.getWidth() - incomingGraphics.PreferredBackBufferWidth / 2))
                { drawPosition.X = incomingGraphics.PreferredBackBufferWidth - chunk.getWidth(); }
                else
                { drawPosition.X = incomingGraphics.PreferredBackBufferWidth / 2 - player1.getPosition().X; }
            }
            if (incomingGraphics.PreferredBackBufferHeight < chunk.getHeight())
            {
                if (player1.getPosition().Y < incomingGraphics.PreferredBackBufferHeight / 2)
                { drawPosition.Y = 0; }
                else if (player1.getPosition().Y > (chunk.getHeight() - incomingGraphics.PreferredBackBufferHeight / 2))
                { drawPosition.Y = incomingGraphics.PreferredBackBufferHeight - chunk.getHeight(); }
                else
                { drawPosition.Y = incomingGraphics.PreferredBackBufferHeight / 2 - player1.getPosition().Y; }
            }

            // Draw World Around Player
            chunk.draw(incomingSpriteBatch, incomingSpriteFont, drawPosition);
        }
    }


}
