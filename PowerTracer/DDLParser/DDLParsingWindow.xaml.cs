using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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

        private void saveMapDDLJSON_Click(object sender, RoutedEventArgs e)
        {
            // https://stackoverflow.com/questions/5136254/saving-file-using-savefiledialog-in-c-sharp
            // http://www.wpf-tutorial.com/dialogs/the-savefiledialog/
            MapDDLJSON mapDDLJSON = new MapDDLJSON();
            foreach (MapDDLDisplay display in mapDDL.displays)
            {
                if (display.isExportable == false)
                {
                    continue;
                }
                foreach (MapDDLLayer layer in display.layers)
                {
                    if (layer.isExportable == false)
                    {
                        continue;
                    }
                    MapDDLJSONLayer jsonLayer = new MapDDLJSONLayer();
                    jsonLayer.name = layer.name;
                    foreach (MapDDLPolyline line in layer.polylines)
                    {
                        MapDDLJSONPolyline jsonLine = new MapDDLJSONPolyline();
                        if (line.origin != null)
                        {
                            jsonLine.points.Add(new MapJSONPoint(line.origin.X, line.origin.Y));
                        }
                        foreach (Point pnt in line.points)
                        {
                            jsonLine.points.Add(new MapJSONPoint(pnt.X, pnt.Y));
                        }
                        jsonLine.gab = line.gab != null ? line.gab : "";
                        jsonLine.ednaId = line.eDNAId != null ? line.eDNAId : "";
                        if (line.cam != null)
                        {
                            jsonLine.cam = line.cam.name != null ? line.cam.name : "";
                            if (line.cam.compositeKey != null)
                            {
                                foreach (KeyValuePair<string, string> compositeKey in line.cam.compositeKey)
                                {
                                    // do something with entry.Value or entry.Key
                                    jsonLine.meta.Add(new Dictionary<string, string>() { { "key", compositeKey.Key }, { "value", compositeKey.Value } });
                                }
                            }
                        }
                        jsonLayer.lines.Add(jsonLine);
                    }
                    mapDDLJSON.layers.Add(jsonLayer);
                }
            }

            string jsonText = JsonConvert.SerializeObject(mapDDLJSON, Formatting.Indented);

            SaveFileDialog savefileDialog = new SaveFileDialog();
            // set a default file name
            savefileDialog.FileName = "test.json";
            // set filters - this can be done in properties as well
            savefileDialog.Filter = "JSON Files (*.json)|*.json|All files (*.*)|*.*";

            if (savefileDialog.ShowDialog() == true)
            {
                File.WriteAllText(savefileDialog.FileName, jsonText);
                dc.addItemsToConsole("Finished exporting map DDL JSON!");
            }
        }
    }
}
