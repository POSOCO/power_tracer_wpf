using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        PowerLine testline_;
        ObservableCollection<PowerLineLayer> layers_;
        LinesJSONFetcher jsonParser_ = new LinesJSONFetcher();
        PowerMap powerMap_;

        public LinesWindow()
        {
            InitializeComponent();
            DataContext = this;

            //layers_ = new ObservableCollection<PowerLineLayer>(jsonParser_.fetchLayers(File.ReadAllText("lines_ddl.json")));
            //layerSelectionItemList.ItemsSource = layers_;

            // testLineDraw();
            //paintLayers();
            powerMap_ = new PowerMap(paintSurface);
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

        private void Draw_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
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
            testline_ = new PowerLine();

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

        public void paintLayers()
        {
            foreach (PowerLineLayer layer in layers_)
            {
                if (layer.isVisible_ != true)
                {
                    continue;
                }
                foreach (PowerLine line in layer.layerLines_)
                {
                    // Using the PowerLine Object for creating a Polyline that is bound to the object
                    Polyline lineObj_ = new Polyline();
                    //setting the lineObj_ MouseEnter and MouseLeave events for hover highlight effect
                    lineObj_.MouseEnter += delegate (object sender, MouseEventArgs e) { ShowHideHighlight(sender, e, EnterOrLeave.Enter, line); };
                    lineObj_.MouseLeave += delegate (object sender, MouseEventArgs e) { ShowHideHighlight(sender, e, EnterOrLeave.Leave, line); };

                    // bind line points
                    // todo set points explicitly for eacch repaint and remove lines from canvas children if out of bounds
                    Binding pointsBinding = new Binding("LinePoints");
                    pointsBinding.Source = line;
                    lineObj_.SetBinding(Polyline.PointsProperty, pointsBinding);

                    // bind line thickness
                    Binding thicknessBinding = new Binding("Thickness");
                    thicknessBinding.Source = line;
                    lineObj_.SetBinding(Polyline.StrokeThicknessProperty, thicknessBinding);

                    // bind line color
                    Binding colorBinding = new Binding("Color");
                    colorBinding.Source = line;
                    colorBinding.Converter = new ColorBrushConverter();
                    lineObj_.SetBinding(Polyline.StrokeProperty, colorBinding);

                    // draw line on the canvas
                    paintSurface.Children.Add(lineObj_);

                    // initialize line power for testing
                    line.Power = 100;
                }

            }
        }

        public void drawLines()
        {
            object payLoad = new { dataRate = 25 };
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
            e.Result = new { dataRate = 25 };
        }

        // worker thread ui update stuff
        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        // worker thread completed stuff
        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            addLinesToConsole("Finished task");
            object res = e.Result;
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
