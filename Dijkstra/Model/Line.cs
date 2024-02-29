using System;
using System.Linq;

namespace Dijkstra.Model
{
    public class Line
    {
        private double _lenght;
        
        public Line(double[] xpoints, double[] ypoints, string statName, string endName)
        {
            Xpoints = xpoints;
            Ypoints = ypoints;
            StartPointsName = statName;
            EndPointsName = endName;
            GetLenght();
        }

        public double[] Xpoints { get; set; }
        public double[] Ypoints { get; set; }
        public string StartPointsName { get; set; }
        public string EndPointsName { get; set; }
        public double Lenght { get => _lenght; set => _lenght = value; }

        private void GetLenght()
        {
            _lenght =  Math.Sqrt(Math.Pow(Xpoints.FirstOrDefault() - Xpoints.LastOrDefault(), 2) +
                Math.Pow(Ypoints.FirstOrDefault() - Ypoints.LastOrDefault(), 2));
        }
    }
}
