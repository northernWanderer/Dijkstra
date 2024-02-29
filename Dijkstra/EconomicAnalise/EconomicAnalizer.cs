using Dijkstra.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dijkstra.EconomicAnalise
{
    public class EconomicAnalizer
    {
        private const double _shortAccidentHour = 2;
        private const double _longAccidentHour = 5;
        public double CostAllLines { get; set; }
        public double CostShotAccident { get; set; }
        public double CostLongAccident { get; set; }
        public List<LineAnalizer> LineAnalizers { get; set; }
        public List<Vertex> Vertexes { get; set; }
        public EconomicAnalizer()
        {
            CostAllLines = 0;
            Vertexes = new List<Vertex>()
            {
                new Vertex("1",0,10),
                new Vertex("2",0,12),
                new Vertex("3",0,50),
                new Vertex("4",0,76),
                new Vertex("5",0,43),
                new Vertex("6",0,45),
                new Vertex("7",0,6),
                new Vertex("8",0,45),
                new Vertex("9",0,78),
                new Vertex("10",0,23),
                new Vertex("11",0,9),
                new Vertex("12",0,32),
                new Vertex("13",0,33),
                new Vertex("14",0,44),
                new Vertex("15",0,55),
                new Vertex("16",0,66),
                new Vertex("17",0,77),
                new Vertex("18",0,88),
                new Vertex("19",0,27),
                new Vertex("20",0,29)
            };

        }

        public List<LineAnalizer> AnalizeAccident(LineAnalizer lineAnalizerBroken, List<LineAnalizer> linesInGraph)
        {
            CostShotAccident = 0;
            CostLongAccident = 0;

            List<LineAnalizer> lineAnalizersOff = new List<LineAnalizer>();
            lineAnalizersOff.Add(lineAnalizerBroken);
            linesInGraph.Where(e=> e.Equals(lineAnalizerBroken)).FirstOrDefault().HaveConnectWithSourse = false;
            lineAnalizerBroken.HaveConnectWithSourse = false;

            List<LineAnalizer> linesOnLong = FindLinesOnInAccident(linesInGraph, lineAnalizerBroken);

            List<Vertex> vertexOffLong = FindVertexOffInAccident(linesOnLong);
            if (vertexOffLong.Count > 0)
            {

                foreach (Vertex vertex in vertexOffLong)
                {
                    CostLongAccident += (vertex.GetCostOfAccident(_longAccidentHour) * lineAnalizerBroken.ProbabilityAccident);
                }
            }


            if (!lineAnalizerBroken.HaveReclouzerInStart && !lineAnalizerBroken.StartPointsName.Equals("1"))
                lineAnalizersOff.AddRange(FindConnectedLine(lineAnalizerBroken.StartPointsName, lineAnalizerBroken));
            if (!lineAnalizerBroken.HaveReclouzerInEnd && !lineAnalizerBroken.StartPointsName.Equals("1"))
                lineAnalizersOff.AddRange(FindConnectedLine(lineAnalizerBroken.EndPointsName, lineAnalizerBroken));

            foreach (var line in lineAnalizersOff)
            {
                line.HaveReclouzerInEnd = false;
            }
            List<LineAnalizer> linesOn = FindLinesOnInAccident(linesInGraph, lineAnalizerBroken);

            List<Vertex> vertexOff = FindVertexOffInAccident(linesOn);
            foreach (Vertex vertex in vertexOff)
            {
                CostShotAccident += (vertex.GetCostOfAccident(_shortAccidentHour) * lineAnalizerBroken.ProbabilityAccident);
            }

            return linesOn;
        }

        private List<LineAnalizer> FindConnectedLine(string PointsName, LineAnalizer lineAnalizerBroken)
        {
            List<LineAnalizer> lineAnalizersOff = new List<LineAnalizer>();
            if (LineAnalizers.Where(e => (e.EndPointsName.Equals(PointsName)
            || e.StartPointsName.Equals(PointsName))
            && !e.Equals(lineAnalizerBroken)
            && e.HaveConnectWithSourse).FirstOrDefault() == null) return lineAnalizersOff;

            IEnumerable<LineAnalizer> lines = LineAnalizers.Where(e => (e.EndPointsName.Equals(PointsName)
            || e.StartPointsName.Equals(PointsName))
            && !e.Equals(lineAnalizerBroken)
            && e.HaveConnectWithSourse);

            lineAnalizersOff.AddRange(lines);
            foreach (var lineOff in lines)
            {
                lineOff.HaveConnectWithSourse = false;

                if (!lineOff.HaveReclouzerInStart && !lineOff.StartPointsName.Equals("1"))
                    lineAnalizersOff.AddRange(FindConnectedLine(lineOff.StartPointsName, lineOff));
                if (!lineOff.HaveReclouzerInEnd && !lineOff.StartPointsName.Equals("1"))
                    lineAnalizersOff.AddRange(FindConnectedLine(lineOff.EndPointsName, lineOff));
            }

            return lineAnalizersOff;
        }

        public List<LineAnalizer> FindLinesOnInAccident(List<LineAnalizer> linesInGraph, LineAnalizer lineAnalizerBroken)
        {

            List<LineAnalizer> lines = new List<LineAnalizer>();
            for (int j = 0; j < linesInGraph.Count; j++)
            {

                if (linesInGraph[j].StartPointsName.Equals("1"))
                {
                    if (linesInGraph[j].HaveConnectWithSourse)
                    {
                        lines.Add(linesInGraph[j]);
                        FindNextOnLine(linesInGraph[j].EndPointsName, linesInGraph, lines);
                    }

                }
                else if (linesInGraph[j].EndPointsName.Equals("1"))
                {
                    if (linesInGraph[j].HaveConnectWithSourse)
                    {
                        lines.Add(linesInGraph[j]);
                        FindNextOnLine(linesInGraph[j].StartPointsName, linesInGraph, lines);
                    }

                }

            }            
            return lines;
        }

        private void FindNextOnLine(string startPointsName, List<LineAnalizer> linesInGraph, List<LineAnalizer> linesOn)
        {
            for (int j = 0; j < linesInGraph.Count; j++)
            {

                if (linesInGraph[j].StartPointsName.Equals(startPointsName) && !linesOn.Contains(linesInGraph[j]))
                {
                    if (linesInGraph[j].HaveConnectWithSourse)
                    {
                        linesOn.Add(linesInGraph[j]);
                        FindNextOnLine(linesInGraph[j].EndPointsName, linesInGraph, linesOn);
                    }

                }
                else if (linesInGraph[j].EndPointsName.Equals(startPointsName) && !linesOn.Contains(linesInGraph[j]))
                {
                    if (linesInGraph[j].HaveConnectWithSourse)
                    {
                        linesOn.Add(linesInGraph[j]);
                        FindNextOnLine(linesInGraph[j].StartPointsName, linesInGraph, linesOn);
                    }

                }
            }
        }

        public List<Vertex> FindVertexOffInAccident(List<LineAnalizer> linesOn)
        {
            List<string> path = new List<string>();
            if (linesOn == null) return new List<Vertex>();
            foreach (LineAnalizer line in linesOn)
            {
                if ((!path.Contains(line.StartPointsName)) && !line.StartPointsName.Equals("1"))
                    path.Add(line.StartPointsName);
                if ((!path.Contains(line.EndPointsName)) && !line.EndPointsName.Equals("1"))
                    path.Add(line.EndPointsName);
            }
            List<Vertex> vertexesOff = Vertexes;
            foreach (string ver in path)
            {
                Vertex vertexOn = Vertexes.Where(e => (e.Name.Equals(ver))).FirstOrDefault();
                vertexesOff.Remove(vertexOn);
            }
            if (vertexesOff.Count > 0) return vertexesOff;
            else return new List<Vertex>();
        }

        public static List<LineAnalizer> FindAllLinesInCurrentTopology(List<List<string>> paths, List<Point> points)
        {
            List<LineAnalizer> lines = new List<LineAnalizer>();
            foreach (List<string> path in paths)
            {
                List<LineAnalizer> linesInOnePath = FindLines(path, points);
                AddLinesIfNotAdded(linesInOnePath, lines);
            }
            return lines;
        }

        private static void AddLinesIfNotAdded(List<LineAnalizer> linesInOnePath, List<LineAnalizer> AllLines)
        {

            foreach (LineAnalizer line in linesInOnePath)
            {
                bool exist = AllLines.Select(e =>
                e.StartPointsName.Equals(line.StartPointsName)
                && e.EndPointsName.Equals(line.EndPointsName)).Where(q => q == true).FirstOrDefault();
                if (exist)
                    continue;
                AllLines.Add(line);
            }

        }

        private static List<LineAnalizer> FindLines(List<string> path, List<Point> points)
        {
            List<LineAnalizer> lineAnalizers = new List<LineAnalizer>();
            for (int i = 0; i < path.Count - 1; i++)
            {
                Line line = new Line(new double[2], new double[2], path[i], path[i + 1]);
                foreach (var point in points)
                {
                    if (point.Name.Equals(path[i]))
                    {
                        line.Xpoints[0] = point.X;
                        line.Ypoints[0] = point.Y;
                    }
                    else if (point.Name.Equals(path[i + 1]))
                    {
                        line.Xpoints[1] = point.X;
                        line.Ypoints[1] = point.Y;
                    }
                }
                lineAnalizers.Add(new LineAnalizer(line));
            }
            return lineAnalizers;
        }
    }
}
