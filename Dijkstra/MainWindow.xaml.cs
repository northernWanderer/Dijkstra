using Dijkstra.Model;
using ScottPlot;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Dijkstra
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty LegendProperty;
        private Program1 Program1 { get; set; }
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
            Program1 = new Program1();
            foreach (Model.Line line in Program1.Lines)
            {
                ScottPlot.Plottables.Scatter scatter = WpfPlot1.Plot.Add.Scatter(line.Xpoints, line.Ypoints);
                scatter.Color = ScottPlot.Color.FromHex("aa98f5");
            }

            Program1.Start();

            foreach (List<string> path in Program1.Paths)
            {
                Model.Line line = FindAllPoints(path);
                if (line == null) return;

                ScottPlot.Plottables.Scatter scatter = WpfPlot1.Plot.Add.Scatter(line.Xpoints, line.Ypoints);
                scatter.Color = ScottPlot.Color.FromHex("c51d34");
                //Program1.FindLineClearLenght(path);
                //Program1.InitEdge();

            }
            //Legend 
            //List<string> path1 = Program1.Path;
            WpfPlot1.Refresh();
            InitializeComponent();
        }

        private Model.Line FindAllPoints(List<string> path)
        {
            Model.Line line = new Model.Line(new double[path.Count()], new double[path.Count()], path.FirstOrDefault(), path.LastOrDefault());
            for (int i = 0; i < path.Count; i++)
            {
                foreach (var point in Program1.Points)
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
