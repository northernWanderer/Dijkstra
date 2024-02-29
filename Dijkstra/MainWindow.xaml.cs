using Dijkstra.EconomicAnalise;
using Dijkstra.Model;
using ScottPlot.Plottables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;


namespace Dijkstra
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string ColorGraphDijkstraHex = "aa98f5";
        private const string ColorPathDijkstraHex = "c51d34";
        private const string ColorLineWithoutRec = "c51d34";
        private const string ColorLineOneRecStart = "18de2c";
        private const string ColorLineOneRecEnd = "e4e805";
        private const string ColorLineTwoRec = "050ce8";
        public static readonly DependencyProperty LegendProperty;
        public static readonly DependencyProperty ShotAccidentCostProperty;
        public static readonly DependencyProperty LongAccidentCostProperty;
        private DijkstraAnaliz DijkstraAnaliz { get; set; }

        public EconomicAnalizer EconomicAnalizer { get; set; }
        private List<(double, List<LineAnalizer>)> _costs;
        public string Legend
        {
            get => (string)GetValue(LegendProperty);
            set => SetValue(LegendProperty, value);
        }
        public double ShotAccidentCost
        {
            get => (double)GetValue(ShotAccidentCostProperty);
            set => SetValue(ShotAccidentCostProperty, value);
        }
        public double LongAccidentCost
        {
            get => (double)GetValue(LongAccidentCostProperty);
            set => SetValue(LongAccidentCostProperty, value);
        }
        static MainWindow()
        {
            LegendProperty = DependencyProperty.Register(
                nameof(Legend),
                typeof(string),
                typeof(MainWindow));
            ShotAccidentCostProperty = DependencyProperty.Register(
                nameof(ShotAccidentCost),
                typeof(double),
                typeof(MainWindow));
            LongAccidentCostProperty = DependencyProperty.Register(
                nameof(LongAccidentCost),
                typeof(double),
                typeof(MainWindow));
        }
        public MainWindow()
        {
            InitializeComponent();
            DijkstraAnaliz = new DijkstraAnaliz();
            EconomicAnalizer = new EconomicAnalizer();
            foreach (Model.Line line in DijkstraAnaliz.Lines)
            {
                ScottPlot.Plottables.Scatter scatter = WpfPlot1.Plot.Add.Scatter(line.Xpoints, line.Ypoints);
                scatter.Color = ScottPlot.Color.FromHex(ColorGraphDijkstraHex);
            }

            DijkstraAnaliz.Start();

            foreach (List<string> path in DijkstraAnaliz.Paths)
            {
                Model.Line line = FindAllPoints(path);
                if (line == null) return;

                ScottPlot.Plottables.Scatter scatter = WpfPlot1.Plot.Add.Scatter(line.Xpoints, line.Ypoints);
                scatter.Color = ScottPlot.Color.FromHex(ColorPathDijkstraHex);
            }

            WpfPlot1.Refresh();

            List<LineAnalizer> linesAnalize = EconomicAnalizer.FindAllLinesInCurrentTopology(DijkstraAnaliz.Paths, DijkstraAnaliz.Points);
            
            _costs = new List<(double, List<LineAnalizer>)>();
            foreach (LineAnalizer lineRec in linesAnalize)
            {
                InitLineAnalizeSourse(linesAnalize);
                LongAccidentCost = 0;
                ShotAccidentCost = 0;
                bool notChanged = SetReclouzer(lineRec);
                EconomicAnalizer.LineAnalizers = linesAnalize;
                foreach (LineAnalizer line in linesAnalize)
                {
                    List<LineAnalizer> OnLines = EconomicAnalizer.AnalizeAccident(line, linesAnalize);
                    EconomicAnalizer.CostAllLines += line.Cost;
                    ShotAccidentCost += EconomicAnalizer.CostShotAccident;
                    LongAccidentCost += EconomicAnalizer.CostLongAccident;
                }
                var currentSignature = new LineAnalizer[linesAnalize.Count];
                linesAnalize.CopyTo(currentSignature);
                for (int i = 0; i< linesAnalize.Count; i++)
                {
                    currentSignature[i] = (LineAnalizer)linesAnalize[i].Clone();
                }
                _costs.Add((LongAccidentCost + ShotAccidentCost + EconomicAnalizer.CostAllLines, currentSignature.ToList()));
                UnSetReclouzer(lineRec, notChanged);
                //InitLineAnalizeSourse(linesAnalize);
                EconomicAnalizer.CostAllLines = 0;
                
            }
            double minCost = double.MaxValue;
            var linesAnalize1 = new List<LineAnalizer>();
            foreach (var cost in _costs)
            {
                if (cost.Item1 < minCost)
                    linesAnalize1 = (List<LineAnalizer>)cost.Item2;
            }

            foreach (var line in linesAnalize1)
            {
                if (line == null) return;
                Scatter scatter = WpfPlot2.Plot.Add.Scatter(line.Ypoints, line.Xpoints);
                EconomicAnalizer.CostAllLines += line.Cost;
                SetColorToScatter(line, scatter);
            }

            Legend = "Стоимость сооружения: " + EconomicAnalizer.CostAllLines + " в год\n" +
                "Убытки до приезда опер бригады: " + ShotAccidentCost + " в год\n" +
                "Убытки после приезда опер бригады: " + LongAccidentCost + " в год\n" +
                "Убытки всего: " + (LongAccidentCost + ShotAccidentCost + EconomicAnalizer.CostAllLines) + " в год\n";

            WpfPlot2.Refresh();
            InitializeComponent();
        }

        private void InitLineAnalizeSourse(List<LineAnalizer> linesAnalize)
        {
            foreach (LineAnalizer line in linesAnalize)
            {
                line.HaveConnectWithSourse = true;
            };
        }

        private static void UnSetReclouzer(LineAnalizer lineRec, bool notChanged)
        {
            if (notChanged)
            {
                notChanged = false;
                return;
            }
            else if (lineRec.HaveReclouzerInStart && !lineRec.HaveReclouzerInEnd)
                lineRec.HaveReclouzerInStart = false;
            else if (lineRec.HaveReclouzerInStart && lineRec.HaveReclouzerInEnd)
                lineRec.HaveReclouzerInEnd = false;
        }

        private static bool SetReclouzer(LineAnalizer lineRec)
        {
            bool notChanged = false;
            if (lineRec.HaveReclouzerInStart && !lineRec.HaveReclouzerInEnd)
                lineRec.HaveReclouzerInEnd = true;
            else if (!lineRec.HaveReclouzerInStart)
                lineRec.HaveReclouzerInStart = true;
            else notChanged = true;
            return notChanged;
        }

        private static void SetColorToScatter(LineAnalizer line, Scatter scatter)
        {
            if (line.HaveReclouzerInStart && line.HaveReclouzerInEnd)
                scatter.Color = ScottPlot.Color.FromHex(ColorLineTwoRec);
            else if (line.HaveReclouzerInStart)
                scatter.Color = ScottPlot.Color.FromHex(ColorLineOneRecStart);
            else if (line.HaveReclouzerInEnd)
                scatter.Color = ScottPlot.Color.FromHex(ColorLineOneRecEnd);
            else
                scatter.Color = ScottPlot.Color.FromHex(ColorLineWithoutRec);
        }

        private Model.Line FindAllPoints(List<string> path)
        {
            Model.Line line = new Model.Line(new double[path.Count()], new double[path.Count()], path.FirstOrDefault(), path.LastOrDefault());
            for (int i = 0; i < path.Count; i++)
            {
                foreach (var point in DijkstraAnaliz.Points)
                {
                    if (point.Name.Equals(path[i]))
                    {
                        line.Xpoints[i] = point.X;
                        line.Ypoints[i] = point.Y;
                    }
                }
            }
            return line;
        }
    }
}
