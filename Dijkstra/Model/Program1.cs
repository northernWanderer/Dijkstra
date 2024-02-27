﻿using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace Dijkstra.Model
{
    class Program1
    {
        public List<Point> Points { get; set; }
        public List<Line> Lines { get; set; }
        public List<string> Path { get; set; }
        public List<List<string>> Paths { get; set; }
        public Program1()
        {
            Paths = new List<List<string>>();
            Points = new List<Point>() {
            new Point("1",0,0),
            new Point("2",2,0),
            new Point("3",0.5,2.5),
            new Point("4",2,3),
            new Point("5",1,6),
            new Point("6",3,2),
            new Point("7",3.5,4.5),
            new Point("8",5,0.5),
            new Point("9",6.5,2),
            new Point("10",6,3),
            new Point("11",8,0.5),
            new Point("12",6,5),
            new Point("13",4,7),
            new Point("14",5.5,8),
            new Point("15",7,6.5),
            new Point("16",8.5,4),
            new Point("17",9,2.5),
            new Point("18",10,1),
            new Point("19",10,6),
            new Point("20",10,8),
            };

            Lines = CreateLines();
        }

        private List<Line> CreateLines()
        {
            var lines = new List<Line>();
            //foreach (Point point in Points)
            //{
            //    foreach (Point point2 in Points)
            //    {
            //        if (point == point2) continue;
            //        lines.Add(new Line(new double[] { point.X, point2.X }, new double[] { point.Y, point2.Y }, point.Name, point2.Name));
            //    }
            //}
            foreach (Point point in Points)
            {
                foreach (Point point2 in Points)
                {
                    if (point.Name.Equals("1"))
                    {
                        for (int i = 2; i < 6; i++)
                        {
                            if (point2.Name.Equals(i.ToString()) )
                                lines.Add(new Line(new double[] { point.X, point2.X }, new double[] { point.Y, point2.Y }, point.Name, point2.Name));
                        }
                    }
                    for (int i = 2; i < 6; i++)
                    {
                        if (point.Name.Equals(i.ToString()))
                        {
                            for (int j = 2; j < 11; j++)
                            {
                                if (point2.Name.Equals(j.ToString()) && i != j)
                                    lines.Add(new Line(new double[] { point.X, point2.X }, new double[] { point.Y, point2.Y }, point.Name, point2.Name));
                            }
                        }
                    }
                    for (int i = 5; i < 11; i++)
                    {
                        if (point.Name.Equals(i.ToString()))
                        {
                            for (int j = 11; j < 20; j++)
                            {
                                if (point2.Name.Equals(j.ToString()) && i != j)
                                    lines.Add(new Line(new double[] { point.X, point2.X }, new double[] { point.Y, point2.Y }, point.Name, point2.Name));
                            }
                        }
                    }
                    for (int i = 12; i < 21; i++)
                    {
                        if (point.Name.Equals(i.ToString()))
                        {
                            for (int j = 14; j < 21; j++)
                            {
                                if (point2.Name.Equals(j.ToString()) && i != j)
                                    lines.Add(new Line(new double[] { point.X, point2.X }, new double[] { point.Y, point2.Y }, point.Name, point2.Name));
                            }
                        }
                    }

                }
            }
            return lines;
        }

        public void Start()
        {
            var g = new Graph();

            //добавление вершин
            InitVertex(g);


            //добавление ребер
            InitEdge(g);

            var dijkstra = new Dijkstra(g);
            Path = dijkstra.FindShortestPath("1", "20");
            foreach (Point point in Points)
            {
                if (point.Name.Equals("1")) continue;
                Paths.Add(dijkstra.FindShortestPath("1", point.Name));
            }
            //Console.WriteLine(Path);
            //Console.ReadLine();
        }

        private void InitEdge(Graph g)
        {
            foreach (Line line in Lines)
            {

                g.AddEdge(line.StartPointsName, line.EndPointsName, line.Lenght);

            }
        }

        private void InitVertex(Graph g)
        {
            foreach (Point point in Points)
            {
                g.AddVertex(point.Name);
            }

        }
    }
}
