using Dijkstra.Model;
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
        public static readonly DependencyProperty LegendProperty;
        private DijkstraAnaliz DijkstraAnaliz { get; set; }
        public List<string> Legend
        {
            get => (List<string>)GetValue(LegendProperty);
            set => SetValue(LegendProperty, value);
        }
        static MainWindow()
        {
            LegendProperty = DependencyProperty.Register(
                nameof(Legend),
                typeof(string[]),
                typeof(MainWindow));
        }
        public MainWindow()
        {
            InitializeComponent();
            DijkstraAnaliz = new DijkstraAnaliz();
            foreach (Model.Line line in DijkstraAnaliz.Lines)
            {
                ScottPlot.Plottables.Scatter scatter = WpfPlot1.Plot.Add.Scatter(line.Xpoints, line.Ypoints);
                scatter.Color = ScottPlot.Color.FromHex("aa98f5");
            }

            DijkstraAnaliz.Start();

            foreach (List<string> path in DijkstraAnaliz.Paths)
            {
                Model.Line line = FindAllPoints(path);
                if (line == null) return;

                ScottPlot.Plottables.Scatter scatter = WpfPlot1.Plot.Add.Scatter(line.Xpoints, line.Ypoints);
                scatter.Color = ScottPlot.Color.FromHex("c51d34");                
            }
            
            WpfPlot1.Refresh();
            InitializeComponent();
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
