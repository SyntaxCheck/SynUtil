using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SynUtil.Game.Map
{
    public static class TileSetLoader
    {
        public static Rectangle[,] GetTileSetRectangles(int columns, int rows, Texture2D tileTexture)
        {
            Rectangle[,] rectangles = new Rectangle[columns, rows];
            Point tileSize = new Point(tileTexture.Width / columns, tileTexture.Height / rows);

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    rectangles[x, y] = new Rectangle(x * tileSize.X, y * tileSize.Y, tileSize.X, tileSize.Y);
                }
            }

            return rectangles;
        }
        public static List<Rectangle> GetTileSetRectanglesList(int columns, int rows, Texture2D tileTexture)
        {
            List<Rectangle> rectangles = new List<Rectangle>();
            Point tileSize = new Point(tileTexture.Width / columns, tileTexture.Height / rows);

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    rectangles.Add(new Rectangle(x * tileSize.X, y * tileSize.Y, tileSize.X, tileSize.Y));
                }
            }

            return rectangles;
        }
    }
}