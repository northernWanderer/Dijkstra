using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dijkstra.Model
{
    internal class Line
    {
        private double lenght;

        public Line(double[] xpoints, double[] ypoints, string statName, string endName)
        {
            Xpoints = xpoints;
            Ypoints = ypoints;
            StartPointsName = statName;
            EndPointsName = endName;
        }

        public double[] Xpoints { get; set; }
        public double[] Ypoints { get; set; }
        public string StartPointsName { get; set; }
        public string EndPointsName { get; set; }
        public double Lenght { get => GetLenght(); set { lenght = GetLenght(); } }

        private double GetLenght()
        {
            return Math.Sqrt(Math.Pow(Xpoints.FirstOrDefault() - Xpoints.LastOrDefault(), 2) +
                Math.Pow(Ypoints.FirstOrDefault() - Ypoints.LastOrDefault(), 2));
        }
    }
}
