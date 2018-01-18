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

namespace PowerTracer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Polyline _baseLine;
        Polyline _highlightLine;
        Point _currentPoint;
        bool _newLine;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void OpenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            addLinesToConsole("You clicked 'Open...'");

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            // addLinesToConsole("You clicked 'Exit...'");
            Application.Current.Shutdown();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Show();
        }

        private void Zoom_Click(object sender, RoutedEventArgs e)
        {
            addLinesToConsole("You clicked 'Zoom...'");
        }

        private void Pan_Click(object sender, RoutedEventArgs e)
        {
            addLinesToConsole("You clicked 'Pan...'");
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            addLinesToConsole("You clicked 'Reset...'");
        }

        private void TestBtn_Click(object sender, RoutedEventArgs e)
        {
            addLinesToConsole("You clicked 'Test Button...'");
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _newLine = true;
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_highlightLine != null && !_newLine)
            {
                _highlightLine.MouseEnter += ShowHighlight;
                _highlightLine.MouseLeave += HideHighlight;
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (_newLine)
                {
                    _baseLine = new Polyline
                    {
                        Stroke = SystemColors.WindowFrameBrush,
                        StrokeThickness = 1.0
                    };
                    _highlightLine = new Polyline
                    {
                        Stroke = new SolidColorBrush(Colors.Green),
                        Opacity = 0.0,
                        StrokeThickness = 10.0
                    };

                    paintSurface.Children.Add(_baseLine);
                    paintSurface.Children.Add(_highlightLine);
                    _newLine = false;
                }

                _currentPoint = e.GetPosition(this);
                _baseLine.Points.Add(_currentPoint);
                _highlightLine.Points.Add(_currentPoint);
            }
        }

        private void ShowHighlight(object sender, MouseEventArgs e)
        {
            var line = sender as Polyline;
            if (line != null)
            {
                line.Opacity = 1.0;
            }
        }

        private void HideHighlight(object sender, MouseEventArgs e)
        {
            var line = sender as Polyline;
            if (line != null)
            {
                line.Opacity = 0.0;
            }
        }

        public void addLinesToConsole(string str)
        {
            string consoleTxt = WelcomeText.Text;
            // todo limit number of lines to 10
            WelcomeText.Text = DateTime.Now.ToString() + ": " + str + "\n" + consoleTxt;
        }
    }
}
