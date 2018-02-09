using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PowerTracer.DDLParser
{
    /// <summary>
    /// Interaction logic for DDLParsingWindow.xaml
    /// </summary>
    public partial class DDLParsingWindow : Window
    {
        ConsoleContent dc = new ConsoleContent();
        MapDDL mapDDL { get; set; }
        public DDLParsingWindow()
        {
            InitializeComponent();
            consoleItems.ItemsSource = dc.ConsoleOutput;
            dc.addItemsToConsole("Hello User!");
            DataContext = this;
        }

        public class ConsoleContent
        {
            ObservableCollection<string> consoleOutput = new ObservableCollection<string>();

            public ObservableCollection<string> ConsoleOutput
            {
                get
                {
                    return consoleOutput;
                }
                set
                {
                    consoleOutput = value;
                }
            }

            public void addItemsToConsole(string str)
            {
                consoleOutput.Add(DateTime.Now.ToString("HH:mm:ss") + " " + str);
            }
        }

        private void Parse_Click(object sender, RoutedEventArgs e)
        {
            dc.addItemsToConsole("Started Parsing the ddl...");
            mapDDL = DDLParser.parseDDLToObject();
            mapBoardTV.ItemsSource = mapDDL.displays;
            dc.addItemsToConsole("Finished Parsing the ddl!");
        }
    }
}
