using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace SynUtil.Game.Map
{
    public enum TileType
    {
        Water,
        Land
    }
    public enum TileBorderType
    {
        None,
        Right,
        Left,
        Bottom,
        Top,
        BottomRight,
        BottomLeft,
        TopRight,
        TopLeft,
        All,
        TopBottom,
        LeftRight,
        BottomLeftRight,
        TopLeftRight,
        TopBottomRight,
        TopBottomLeft
    }

    public class MapLoader
    {
        string FileName { get; set; }

        public MapLoader(string mapFileName)
        {
            FileName = mapFileName;

            if (!File.Exists(FileName))
            {
                throw new Exception("Map file does not exist");
            }
        }

        public MapTile[,] LoadMap()
        {
            MapTile[,] tileInformation = new MapTile[0, 0];

            using (StreamReader sr = File.OpenText(FileName))
            {
                string line = String.Empty;
                string firstLine = String.Empty;
                List<GridGroup> gridGroups = null;
                int gridSize = 25;
                int gridRowWidth = 80;

                int y = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    if (String.IsNullOrEmpty(firstLine))
                    {
                        firstLine = line;
                        string[] split = firstLine.Split(new char[] { ' ' }, StringSplitOptions.None);

                        tileInformation = new MapTile[int.Parse(split[0]), int.Parse(split[1])];
                    }
                    else
                    {
                        if (gridGroups == null)
                        {
                            gridGroups = new List<GridGroup>();
                            int yG = 0;
                            int xG = 0;
                            while (yG < tileInformation.GetLength(0))
                            {
                                xG = 0;
                                while (xG < tileInformation.GetLength(1))
                                {
                                    GridGroup grid = new GridGroup();
                                    grid.Bounds = new Rectangle(xG, yG, gridSize, gridSize);
                                    grid.HasChanges = true;
                                    gridGroups.Add(grid);
                                    xG += gridSize;
                                }
                                yG += gridSize;
                            }
                        }
                        for (int i = 0; i < (line.Length / 4); i++)
                        {
                            string tileText = line.Substring(i * 4, 4);
                            short borderType = short.Parse(tileText.Substring(2, 2));
                            string tileType = tileText.Substring(1, 1);

                            MapTile tile = new MapTile();

                            tile.IsWalkable = tileText.Substring(0, 1) == "1" ? true : false;
                            tile.TileBorderType = (TileBorderType)borderType;
                            //(80*(y/25)) + (x/25)
                            tile.GridSection = gridGroups[(gridRowWidth*(y/gridSize)) + (i / gridSize)];

                            //Mod(y,25) * 25 + Mod(x,25)
                            //tile.GridSection = gridGroups[((y % gridSize) * gridSize) + (i % gridSize)];
                            //tile.GridSection = gridGroups.Where(t => t.Bounds.Contains(new Point(i, y))).FirstOrDefault();

                            switch (tileType)
                            {
                                case "W":
                                    tile.TileType = TileType.Water;
                                    break;
                                case "L":
                                    tile.TileType = TileType.Land;
                                    break;
                                default:
                                    throw new Exception("Unknown tile type: " + tileType);
                            }

                            tileInformation[i, y] = tile;
                        }

                        y++;
                    }
                }
            }

            return tileInformation;
        }
    }
}