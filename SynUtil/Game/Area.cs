using System.Drawing;

namespace SynUtil.Game
{
    public class Area
    {
        public Point[] Polygon { get; set; }

        public Area()
        {
        }
        public Area(Point[] pointsIn)
        {
            Polygon = pointsIn;
        }
        public Area(Tile[] tiles)
        {
            Polygon = new Point[tiles.Length];
            for(int i = 0; i < tiles.Length; i++)
            {
                Polygon[i] = new Point(tiles[i].X, tiles[i].Y);
            }
        }

        public bool IsInPolygon(Point p)
        {
            Point p1, p2;
            bool inside = false;

            if (Polygon.Length < 3)
            {
                return inside;
            }

            var oldPoint = new Point(
                Polygon[Polygon.Length - 1].X, Polygon[Polygon.Length - 1].Y);

            for (int i = 0; i < Polygon.Length; i++)
            {
                var newPoint = new Point(Polygon[i].X, Polygon[i].Y);

                if (newPoint.X > oldPoint.X)
                {
                    p1 = oldPoint;
                    p2 = newPoint;
                }
                else
                {
                    p1 = newPoint;
                    p2 = oldPoint;
                }

                if ((newPoint.X < p.X) == (p.X <= oldPoint.X)
                    && (p.Y - (long)p1.Y) * (p2.X - p1.X)
                    < (p2.Y - (long)p1.Y) * (p.X - p1.X))
                {
                    inside = !inside;
                }

                oldPoint = newPoint;
            }

            return inside;
        }
    }
}
