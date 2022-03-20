using System;

namespace SynUtil.Game.Map
{
    public class MapTile
    {
        public TileType TileType { get; set; }
        public TileBorderType TileBorderType { get; set; }
        public GridGroup GridSection { get; set; }
        public bool IsWalkable { get; set; }

        public MapTile()
        {
        }

        public virtual string PackObject()
        {
            return String.Empty;
        }
        public virtual void UnpackObject(string messageIn)
        {

        }
    }
}