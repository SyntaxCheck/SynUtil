namespace SynUtil.Game.Map
{
    public class MapTile
    {
        public TileType TileType { get; set; }
        public TileBorderType TileBorderType { get; set; }
        public bool IsWalkable { get; set; }

        public MapTile()
        {
        }
    }
}