using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace PowerTracer
{
    /// <summary>
    /// Interaction logic for LinesWindow.xaml
    /// </summary>
    public partial class LinesWindow : Window
    {
        Polyline _baseLine;
        Point _currentPoint;
        bool _newLine;
        PowerLine testline_;
        public LinesWindow()
        {
            InitializeComponent();
            testLineDraw();
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

        private void NewWindow_Click(object sender, RoutedEventArgs e)
        {
            addLinesToConsole("Opening another window...");
            LinesWindow linesWindow = new LinesWindow();
            linesWindow.Show();
        }

        private void TestBtn_Click(object sender, RoutedEventArgs e)
        {
            addLinesToConsole("You clicked 'Test Button...'");
            LinesJSONFetcher linesJSONFetcher = new LinesJSONFetcher();
            linesJSONFetcher.fetchLayers("");
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _newLine = true;
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_baseLine != null && !_newLine)
            {
                _baseLine.MouseEnter += ShowHighlight;
                _baseLine.MouseLeave += HideHighlight;
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
                        Stroke = new SolidColorBrush(Colors.Red),
                        StrokeThickness = 5.0
                    };
                    paintSurface.Children.Add(_baseLine);
                    _newLine = false;
                }

                _currentPoint = e.GetPosition(this);
                _baseLine.Points.Add(_currentPoint);
            }
        }

        private void ShowHighlight(object sender, MouseEventArgs e)
        {
            var line = sender as Polyline;
            if (line != null)
            {
                line.Opacity = 0.5;
            }
        }

        private void HideHighlight(object sender, MouseEventArgs e)
        {
            var line = sender as Polyline;
            if (line != null)
            {
                line.Opacity = 1.0;
            }
        }

        private static void ShowHideHighlight(object sender, MouseEventArgs e, EnterOrLeave enterOrLeave, PowerLine powerLine)
        {
            // http://www.c-sharpcorner.com/blogs/passing-parameters-to-events-c-sharp1
            if (powerLine != null)
            {
                if (enterOrLeave == EnterOrLeave.Enter)
                {
                    powerLine.IsHighLighted = true;
                }
                else
                {
                    powerLine.IsHighLighted = false;
                }
            }
        }

        private enum EnterOrLeave
        {
            Enter,
            Leave
        }

        public class ColorBrushConverter : IValueConverter
        {
            // https://stackoverflow.com/questions/3309709/how-do-i-convert-a-color-to-a-brush-in-xaml
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                if (value == null)
                    return null;

                if (value is Color)
                    return new SolidColorBrush((Color)value);

                throw new InvalidOperationException("Unsupported type [" + value.GetType().Name + "], ColorToSolidColorBrushValueConverter.Convert()");
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                // useful incase of two way binding. Right it doesnot work
                if (value == null)
                    return null;

                if (value is SolidColorBrush)
                    return ((SolidColorBrush)value).Color;

                throw new InvalidOperationException("Unsupported type [" + value.GetType().Name + "], ColorToSolidColorBrushValueConverter.ConvertBack()");
            }
        }

        public void testLineDraw()
        {
            // Initializing a power line
            testline_ = new PowerLine(new PointCollection());

            // Using the PowerLine Object for creating a Polyline that is bound to the object
            Polyline lineObj_ = new Polyline();

            //setting the lineObj_ MouseEnter and MouseLeave events for hover highlight effect
            lineObj_.MouseEnter += delegate (object sender, MouseEventArgs e) { ShowHideHighlight(sender, e, EnterOrLeave.Enter, testline_); };
            lineObj_.MouseLeave += delegate (object sender, MouseEventArgs e) { ShowHideHighlight(sender, e, EnterOrLeave.Leave, testline_); };

            // bind line points
            Binding pointsBinding = new Binding("LinePoints");
            pointsBinding.Source = testline_;
            lineObj_.SetBinding(Polyline.PointsProperty, pointsBinding);

            // bind line thickness
            Binding thicknessBinding = new Binding("Thickness");
            thicknessBinding.Source = testline_;
            lineObj_.SetBinding(Polyline.StrokeThicknessProperty, thicknessBinding);

            // bind line color
            Binding colorBinding = new Binding("Color");
            colorBinding.Source = testline_;
            colorBinding.Converter = new ColorBrushConverter();
            lineObj_.SetBinding(Polyline.StrokeProperty, colorBinding);

            paintSurface.Children.Add(lineObj_);

            //Changing PowerLine object parameters for testing the binding
            testline_.Power = 500;
            PointCollection pointCollection = new PointCollection();
            pointCollection.Add(new Point(0, 0));
            pointCollection.Add(new Point(200, 200));
            testline_.LinePoints = pointCollection;
            testline_.LinePoints.Add(new Point(200, 250));

            // paintSurface.Children.Remove(lineObj_);
        }


        public void drawLines(DateTime startTime)
        {
            object payLoad = new { startTime = startTime};
            // addLinesToConsole("Started fetching data");
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync(payLoad);
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            object argument = e.Argument;
            int dataRate = (int)argument.GetType().GetProperty("dataRate").GetValue(argument, null);
            DateTime startTime = (DateTime)argument.GetType().GetProperty("startTime").GetValue(argument, null);
            DateTime endTime = (DateTime)argument.GetType().GetProperty("endTime").GetValue(argument, null);
            List<int> measurementIDs = (List<int>)argument.GetType().GetProperty("measurementIDs").GetValue(argument, null);
            List<string> measurementNames = (List<string>)argument.GetType().GetProperty("measurementNames").GetValue(argument, null);
            e.Result = new { startTime = startTime, endTime = endTime, dataRate = dataRate, measurementIDs = measurementIDs, measurementNames = measurementNames };
        }

        // created by sudhir on 30.12.2017
        // worker thread ui update stuff
        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        // created by sudhir on 30.12.2017
        // worker thread completed stuff
        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            addLinesToConsole("Finished fetching data");
            object res = e.Result;
            List<int> measurementIDs = (List<int>)res.GetType().GetProperty("measurementIDs").GetValue(res, null);
            List<string> measurementNames = (List<string>)res.GetType().GetProperty("measurementNames").GetValue(res, null);
            int dataRate = (int)res.GetType().GetProperty("dataRate").GetValue(res, null);
        }


        public void addLinesToConsole(string str)
        {
            string consoleTxt = WelcomeText.Text;
            // todo limit number of lines to 10
            WelcomeText.Text = DateTime.Now.ToString() + ": " + str + "\n" + consoleTxt;
        }
    }
}
