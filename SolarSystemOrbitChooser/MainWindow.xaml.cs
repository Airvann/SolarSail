using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using MetaheuristicHelper;

namespace SolarSystemOrbitChooser
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Orbit targetOrbit = MetaheuristicHelper.Orbits.Mercury.Get();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MouseEnterPlanet(object sender, MouseButtonEventArgs e)
        {
            double top = Canvas.GetTop(sender as Ellipse);
            double left = Canvas.GetLeft(sender as Ellipse);

            Canvas.SetTop(Selection, top - (0.087 * Selection.Height));
            Canvas.SetLeft(Selection, left - (0.087 * Selection.Width));

            if ((sender as Ellipse) == Mercury)
            {
                targetOrbit = MetaheuristicHelper.Orbits.Mercury.Get();
                Selection.Stroke = new SolidColorBrush(Color.FromArgb(255, 77, 71, 71));
                LabelName.Content = "Название: Меркурий";
                LabelRad.Content = "Радиус орбиты: 57 909 227 км";
                LabelSatellites.Content = "Спутники: Нет";
                LabelMass.Content = "Масса: 3,33022 * 10^23 кг";
                Accept.IsEnabled = true;
            }
            else if ((sender as Ellipse) == Venus)
            {
                targetOrbit = MetaheuristicHelper.Orbits.Venus.Get();
                Selection.Stroke = new SolidColorBrush(Color.FromArgb(255, 178, 106, 0));
                LabelName.Content = "Название: Венера";
                LabelRad.Content = "Радиус орбиты: 108 209 184 км";
                LabelSatellites.Content = "Спутники: Нет";
                LabelMass.Content = "Масса: 4,8675 * 10^24 кг";
                Accept.IsEnabled = true;
            }
            else if ((sender as Ellipse) == Earth)
            {
                Selection.Stroke = new SolidColorBrush(Color.FromArgb(255, 13, 99, 201));
                LabelName.Content = "Название: Земля";
                LabelRad.Content = "Радиус орбиты: 149 598 261 км";
                LabelSatellites.Content = "Спутники: 1 (Луна)";
                LabelMass.Content = "Масса: 5,9726 * 10^24 кг";
                Accept.IsEnabled = false;
            }
            else if ((sender as Ellipse) == Mars)
            {
                targetOrbit = MetaheuristicHelper.Orbits.Mars.Get();
                Selection.Stroke = new SolidColorBrush(Color.FromArgb(255, 205, 38, 38));
                LabelName.Content = "Название: Марс";
                LabelRad.Content = "Радиус орбиты: 227 943 500 км";
                LabelSatellites.Content = "Спутники: 2 (Фобос, Деймос)";
                LabelMass.Content = "Масса: 6,4171 * 10^23 кг";
                Accept.IsEnabled = true;
            }
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            Settings.Get().orbit = targetOrbit;
            Close();
        }
    }
}