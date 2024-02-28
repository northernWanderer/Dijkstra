using Dijkstra.Model;
using System;
using System.Linq;

namespace Dijkstra.EconomicAnalise
{
    internal class LineAnalizer
    {
        private const double _costOneKm = 200;
        private const double _costOneRec = 75;
        public double Cost { get; set; }
        private double _lenght;
        public bool HaveConnectWithSourse { get; set; }
        public bool HaveReclouzerInStart { get; set; }
        public bool HaveReclouzerInEnd { get; set; }
        public double[] Xpoints { get; set; }
        public double[] Ypoints { get; set; }
        public string StartPointsName { get; set; }
        public string EndPointsName { get; set; }
        public double Lenght { get => _lenght; set => _lenght = value; }
        public LineAnalizer(double[] xpoints, double[] ypoints, string statName, string endName)
        {
            Xpoints = xpoints;
            Ypoints = ypoints;
            StartPointsName = statName;
            EndPointsName = endName;
            HaveConnectWithSourse = false;
            GetLenght();
        }
        public LineAnalizer(Line line, bool haveRecStart = false, bool haveRecEnd = false, bool haveSourse = false)
        {
            Xpoints = line.Xpoints;
            Ypoints = line.Ypoints;
            StartPointsName = line.StartPointsName;
            EndPointsName = line.EndPointsName;
            HaveConnectWithSourse = haveSourse;
            HaveReclouzerInEnd = haveRecEnd;
            HaveReclouzerInStart = haveRecStart;
            GetLenght();
            Cost = GetCost();
        }

        private double GetCost()
        {
            return _lenght * _costOneKm + _costOneRec * Convert.ToDouble(HaveReclouzerInEnd)
                + _costOneRec * Convert.ToDouble(HaveReclouzerInStart);
        }

        private void GetLenght()
        {
            _lenght = Math.Sqrt(Math.Pow(Xpoints.FirstOrDefault() - Xpoints.LastOrDefault(), 2) +
                Math.Pow(Ypoints.FirstOrDefault() - Ypoints.LastOrDefault(), 2));
        }
    }
}
