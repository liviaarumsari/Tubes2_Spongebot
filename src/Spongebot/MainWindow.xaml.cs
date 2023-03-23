using Microsoft.Win32;
using Spongebot.Algorithms;
using Spongebot.Enums;
using Spongebot.Exceptions;
using Spongebot.IO;
using Spongebot.Objects;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace Spongebot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Board? board;

        private FileIO? configFile;

        private double timeInterval = 0;

        public event PropertyChangedEventHandler? PropertyChanged;

        private string warningMessageSearchFile = "";
        public string WarningMessageSearchFile
        {
            get
            {
                return warningMessageSearchFile;
            }
            set
            {
                warningMessageSearchFile = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WarningMessageSearchFile"));
            }
        }

        private string chosenFileName;
        public string ChosenFileName
        {
            get
            {
                return chosenFileName;
            }
            set
            {
                chosenFileName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ChosenFileName"));
            }
        }

        private string warningMessageVisualize = "";
        public string WarningMessageVisualize
        {
            get
            {
                return warningMessageVisualize;
            }
            set
            {
                warningMessageVisualize = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WarningMessageVisualize"));
            }
        }


        private string warningMessageTreasure = "";
        public string WarningMessageTreasure
        {
            get
            {
                return warningMessageTreasure;
            }
            set
            {
                warningMessageTreasure = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WarningMessageTreasure"));
            }
        }

        private bool isDFS = false;
        public bool IsDFS
        {
            get
            {
                return isDFS;
            }
            set
            {
                isDFS = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsDFS"));
            }
        }

        private bool isBFS = true;
        public bool IsBFS
        {
            get
            {
                return isBFS;
            }
            set
            {
                isBFS = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsBFS"));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void ChooseFileButton_Click(object sender, RoutedEventArgs e)
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string deafultPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(currentDirectory, @"..\..\..\..\..\test\"));

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = deafultPath;
            fileDialog.Filter = "Maze config file | *.txt";
            fileDialog.Title = "Please pick your maze config file";

            bool? success = fileDialog.ShowDialog();

            if (success == true)
            {
                try
                {
                    configFile = new FileIO(fileDialog.FileName, fileDialog.SafeFileName);
                    ChosenFileName = fileDialog.SafeFileName;
                    board = configFile.readBoardFromFile();
                    WarningMessageSearchFile = "";
                }
                catch (FileNotFoundException ex)
                {
                    WarningMessageSearchFile = ex.Message + " Please enter a valid file.";
                    configFile = null;
                }
                catch (InvalidFileFormatException ex)
                {
                    WarningMessageSearchFile = ex.Message + " Please fix the config file format.";
                    configFile = null;
                }
                catch (Exception ex)
                {
                    WarningMessageSearchFile = ex.Message;
                    configFile = null;
                }
            }
        }

        private void SearchFileButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // initialize your board object here
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string configPath = System.IO.Path.Combine(currentDirectory, @"..\..\..\..\..\test\", InputFileTextBox.Text);
                configFile = new FileIO(System.IO.Path.GetFullPath(configPath), InputFileTextBox.Text);
                WarningMessageSearchFile = "File successfully found!";
                InputFileWarning.Foreground = Brushes.White;
                board = configFile.readBoardFromFile();
            }
            catch (FileNotFoundException ex)
            {
                WarningMessageSearchFile = ex.Message + " Please enter a valid file.";
                configFile = null;
            }
            catch (InvalidFileFormatException ex)
            {
                WarningMessageSearchFile = ex.Message + " Please fix the config file format.";
                configFile = null;
            }
            catch (Exception ex)
            {
                WarningMessageSearchFile = ex.Message;
                configFile = null;
            }
        }

        private void VisualizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (configFile == null)
            {
                WarningMessageVisualize = "Please choose your file first.";
            }
            else
            {
                WarningMessageVisualize = "";
                outputColumn.Visibility = Visibility.Visible;
                DrawBoard();
            }
        }

        private void DrawBoard()
        {
            // clear the grid
            boardGrid.Children.Clear();
            boardGrid.RowDefinitions.Clear();
            boardGrid.ColumnDefinitions.Clear();

            int n_column = board.Cells.GetLength(0);
            int n_row = board.Cells.GetLength(1);
            var gridLen = n_column > n_row ? 400 / n_column : 400 / n_row;

            // add rows and columns to the grid
            for (int y = 0; y < n_row; y++)
            {
                RowDefinition gridRow = new RowDefinition();
                gridRow.Height = new GridLength(gridLen);
                boardGrid.RowDefinitions.Add(gridRow);
            }
            for (int x = 0; x < n_column; x++)
            {
                ColumnDefinition gridCol = new ColumnDefinition();
                gridCol.Width = new GridLength(gridLen);
                boardGrid.ColumnDefinitions.Add(gridCol);
            }

            // add cells to the grid
            for (int y = 0; y < n_row; y++)
            {
                for (int x = 0; x < n_column; x++)
                {
                    Cell cell = board.Cells[x, y];
                    Border border = new Border();
                    border.DataContext = cell;
                    border.SetBinding(Border.BackgroundProperty, new Binding("CellBackground")); // Bind Border to Cell's CellBackground property
                    border.BorderBrush = Brushes.Transparent;
                    border.Margin = new Thickness(1);
                    if (cell.Type == CellType.Start)
                    {
                        System.Windows.Controls.Image startImage = new System.Windows.Controls.Image();
                        startImage.Source = new BitmapImage(new Uri("./Images/startImg.png", UriKind.Relative));
                        startImage.Style = (Style)FindResource("ImageBoardStyle");
                        startImage.Height = (int)gridLen / 2.5;
                        Grid.SetColumn(startImage, x);
                        Grid.SetRow(startImage, y);
                        border.Child = startImage;
                    }
                    else if (cell.Type == CellType.Treasure)
                    {
                        System.Windows.Controls.Image treasureImage = new System.Windows.Controls.Image();
                        treasureImage.Source = new BitmapImage(new Uri("./Images/treasureImg.png", UriKind.Relative));
                        treasureImage.Style = (Style)FindResource("ImageBoardStyle");
                        treasureImage.Height = (int)gridLen / 2.5;
                        Grid.SetColumn(treasureImage, x);
                        Grid.SetRow(treasureImage, y);
                        border.Child = treasureImage;
                    }
                    Grid.SetColumn(border, x);
                    Grid.SetRow(border, y);
                    boardGrid.Children.Add(border);
                }
            }
        }

        private void TimeIntervalSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            timeInterval = TimeIntervalSlider.Value;
        }

        private async void SearchTreasureButton_Click(object sender, RoutedEventArgs e)
        {
            if (board != null)
            {
                outputColumn.Visibility = Visibility.Visible;
                DrawBoard();
                WarningMessageTreasure = "";
                RouteLabel.Content = "-";
                NodesLabel.Content = 0;
                StepsLabel.Content = 0;
                ExecutionTimeLabel.Content = "0 ms";
                var watch = new System.Diagnostics.Stopwatch();
                if (isBFS)
                {
                    // Ambil attr IsTSP, timeInterval
                    BFS bfs = new BFS(board);
                    if (TSPCheckbox.IsChecked == true)
                    {
                        await bfs.runTSP(timeInterval);
                        Debug.WriteLine("TSP");
                    }
                    else
                    {
                        await bfs.runNonTSP(timeInterval);
                    }
                    // Set output Result
                    // RouteLabel.Content = 
                    //NodesLabel.Content =
                    //StepsLabel.Content =
                    //ExecutionTimeLabel.Content =

                }
                else
                {
                    watch.Start();
                    
                    DFS DFS = new DFS(board);
                    if (TSPCheckbox.IsChecked == true)
                    {
                        await DFS.run(true, timeInterval);
                        Debug.WriteLine("TSP");
                    }
                    else
                    {
                        await DFS.run(false,timeInterval);
                        Debug.WriteLine("Non TSP");
                    }
                    watch.Stop();

                    Debug.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
                    // Set output Result
                    RouteLabel.Content = DFS.finalRoute;
                    NodesLabel.Content = DFS.visitedNodes;
                    StepsLabel.Content = DFS.totalSteps;
                    ExecutionTimeLabel.Content = watch.ElapsedMilliseconds + " ms";
                }
                resultOutput.Visibility = Visibility.Visible;
            }
            else
            {
                WarningMessageTreasure = "Please input and visualize your file first!";
            }

        }
    }
}
