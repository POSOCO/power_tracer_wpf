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
            layerSelectionItemList.ItemsSource = powerMap_.powerLayers_;
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

        public void addLinesToConsoleDelete(string str)
        {
            // todo allow other threads to write here just like we did in the reporting tool
            string consoleTxt = WelcomeText.Text;
            // todo limit number of lines to 10
            WelcomeText.Text = DateTime.Now.ToString() + ": " + str + "\n" + consoleTxt;
        }

        public static void addLinesToConsole(String str)
        {
            main.Dispatcher.Invoke(new Action(() =>
            {
                string consoleTxt = main.WelcomeText.Text;
                // todo limit number of lines to 10
                main.WelcomeText.Text = DateTime.Now.ToString() + ": " + str + "\n" + consoleTxt;
            }));
        }
    }
}
