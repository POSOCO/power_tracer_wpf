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
        PowerMap powerMap_;
        internal static LinesWindow main;
        public LinesWindow()
        {
            InitializeComponent();
            main = this;
            DataContext = this;
            powerMap_ = new PowerMap(paintSurface);
            powerMap_.PropertyChanged += HandlePropertyChanged;
            layerSelectionItemList.ItemsSource = powerMap_.powerLayers_;
            PanValue.DataContext = powerMap_.painter_;
            ZoomValue.DataContext = powerMap_.painter_;
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

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            addLinesToConsole("You clicked 'Reset...'");
            powerMap_.painter_.pan_ = new Point(PowerLayerPainter.defPan_.X, PowerLayerPainter.defPan_.Y);
            powerMap_.painter_.Zoom = new Point(PowerLayerPainter.defZoom_.X, PowerLayerPainter.defZoom_.Y);
        }

        private void NewWindow_Click(object sender, RoutedEventArgs e)
        {
            addLinesToConsole("Opening another window...");
            LinesWindow linesWindow = new LinesWindow();
            linesWindow.Show();
        }

        private void View_Click(object sender, RoutedEventArgs e)
        {
            DDLParser.DDLParser.parseDDLToJSON();
        }

        public void addLinesToConsole(string str)
        {
            // todo allow other threads to write here just like we did in the reporting tool
            string consoleTxt = WelcomeText.Text;
            // todo limit number of lines to 10
            WelcomeText.Text = DateTime.Now.ToString() + ": " + str + "\n" + consoleTxt;
        }

        public static void addLinesToConsoleDelete(String str)
        {
            main.Dispatcher.Invoke(new Action(() =>
            {
                string consoleTxt = main.WelcomeText.Text;
                // todo limit number of lines to 10
                main.WelcomeText.Text = DateTime.Now.ToString() + ": " + str + "\n" + consoleTxt;
            }));
        }

        private void ZoomPlus_click(object sender, RoutedEventArgs e)
        {
            Point initialZoom = powerMap_.painter_.zoom_;
            powerMap_.painter_.Zoom = new Point(initialZoom.X + 0.01, initialZoom.Y + 0.01);
        }

        private void ZoomMinus_Click(object sender, RoutedEventArgs e)
        {
            Point initialZoom = powerMap_.painter_.zoom_;
            powerMap_.painter_.Zoom = new Point(initialZoom.X - 0.01, initialZoom.Y - 0.01);
        }

        private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "IsHighLighted":
                    // get the lineDataObject and update the line stats
                    PowerLayerLineObj lineObj = sender as PowerLayerLineObj;
                    if (lineObj != null)
                    {
                        UIObjStats.Content = String.Format("id : {0}, power : {1}, voltage : {2}", lineObj.lineDataObj_.address_, lineObj.lineDataObj_.power_, lineObj.lineDataObj_.voltage_);
                    }
                    break;
                default:
                    // do something
                    break;
            }
        }        
    }
}
