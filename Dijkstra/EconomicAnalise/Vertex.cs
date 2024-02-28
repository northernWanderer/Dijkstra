using System.Collections.Generic;

namespace Dijkstra.EconomicAnalise
{
    internal class Vertex
    {
        const double _costOneHourOneMV = 10;
        private double HoursOfAccident {  get; set; }
        public double CostOfAccident {  get; set; }
        public string Name {  get; set; }
        public int NumberOfSourse {  get; set; }
        public int Power {  get; set; }
        public List<LineAnalizer> ConnectedLine {  get; set; }
        public Vertex(string name, int numberOfSourse, int power)
        {
            Name = name;
            NumberOfSourse = numberOfSourse;
            Power = power;
            ConnectedLine = new List<LineAnalizer>();
            GetCostOfAccident();
        }

        private void GetCostOfAccident(double hoursOfAccident)
        {
            CostOfAccident = hoursOfAccident * _costOneHourOneMV * Power;
        }
        private void GetCostOfAccident()
        {
            CostOfAccident = HoursOfAccident * _costOneHourOneMV * Power;
        }
    }
}
