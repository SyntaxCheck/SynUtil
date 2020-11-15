using System;
using System.IO;

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
                        for (int i = 0; i < (line.Length / 4); i++)
                        {
                            string tileText = line.Substring(i * 4, 4);
                            short borderType = short.Parse(tileText.Substring(2, 2));
                            string tileType = tileText.Substring(1, 1);

                            MapTile tile = new MapTile();

                            tile.IsWalkable = tileText.Substring(0, 1) == "1" ? true : false;
                            tile.TileBorderType = (TileBorderType)borderType;

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