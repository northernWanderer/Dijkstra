namespace Dijkstra.Model
{
    public class Point
    {
        private double _x;
        private double _y;
        private string _name;

        public string Name { get => _name; set => _name = value; }
        public double Y { get => _y; set => _y = value; }
        public double X { get => _x; set => _x = value; }
        public Point(string name, double x, double y)
        {
            Name = name;
            Y = y;
            X = x;
        }
    }
}
