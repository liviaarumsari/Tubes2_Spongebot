using Microsoft.Win32;
using Spongebot.Algorithms;
using Spongebot.Enums;
using Spongebot.Exceptions;
using Spongebot.IO;
using Spongebot.Objects;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Spongebot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Board? board;

        public event PropertyChangedEventHandler? PropertyChanged;

        private string warningMessage = "";
        public string WarningMessage
        {
            get
            {
                return warningMessage;
            }
            set
            {
                warningMessage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WarningMessage"));
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

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string deafultPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(currentDirectory, @"..\..\..\..\..\test\board2.txt"));

            FileIO configFile = new FileIO(deafultPath);
            board = configFile.readBoardFromFile();

            // draw the board
            DrawBoard();
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
                    FileIO configFile = new FileIO(fileDialog.FileName, fileDialog.SafeFileName);
                    ChosenFileName = fileDialog.SafeFileName;
                    board = configFile.readBoardFromFile();

                    // draw the board
                    DrawBoard();
                }
                catch (FileNotFoundException ex)
                {
                    WarningMessage = ex.Message + " Please enter a valid file.";
                }
                catch (InvalidFileFormatException ex)
                {
                    WarningMessage = ex.Message + " Please fix the config file format.";
                }
                catch (Exception ex)
                {
                    WarningMessage = ex.Message;
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
                FileIO configFile = new FileIO(System.IO.Path.GetFullPath(configPath), InputFileTextBox.Text);
                board = configFile.readBoardFromFile();

                // draw the board
                DrawBoard();
            }
            catch (FileNotFoundException ex)
            {
                WarningMessage = ex.Message + " Please enter a valid file.";
            }
            catch (InvalidFileFormatException ex)
            {
                WarningMessage = ex.Message + " Please fix the config file format.";
            }
            catch (Exception ex)
            {
                WarningMessage = ex.Message;
            }
        }

        private void DrawBoard()
        {
            // clear the grid
            MainGrid.Children.Clear();
            MainGrid.RowDefinitions.Clear();
            MainGrid.ColumnDefinitions.Clear();

            int n_column = board.Cells.GetLength(0);
            int n_row = board.Cells.GetLength(1);
            var gridLen = n_column > n_row ? 400/n_column : 400/n_row;

            // add rows and columns to the grid
            for (int y = 0; y < n_row; y++)
            {
                RowDefinition gridRow = new RowDefinition();
                gridRow.Height = new GridLength(gridLen);
                MainGrid.RowDefinitions.Add(gridRow);
            }
            for (int x = 0; x < n_column; x++)
            {
                ColumnDefinition gridCol = new ColumnDefinition();
                gridCol.Width = new GridLength(gridLen);
                MainGrid.ColumnDefinitions.Add(gridCol);
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
                    if (cell.Type == CellType.Start)
                    {
                        TextBlock txtBlock1 = new TextBlock();
                        txtBlock1.Text = "Start";
                        txtBlock1.FontSize = 14;
                        txtBlock1.HorizontalAlignment = HorizontalAlignment.Center;
                        txtBlock1.VerticalAlignment = VerticalAlignment.Center;
                        Grid.SetColumn(txtBlock1, x);
                        Grid.SetRow(txtBlock1, y);
                        border.Child = txtBlock1;
                    }
                    else if (cell.Type == CellType.Treasure)
                    {
                        TextBlock txtBlock1 = new TextBlock();
                        txtBlock1.Text = "Treasure";
                        txtBlock1.FontSize = 14;
                        txtBlock1.HorizontalAlignment = HorizontalAlignment.Center;
                        txtBlock1.VerticalAlignment = VerticalAlignment.Center;
                        Grid.SetColumn(txtBlock1, x);
                        Grid.SetRow(txtBlock1, y);
                        border.Child = txtBlock1;
                    }
                    Grid.SetColumn(border, x);
                    Grid.SetRow(border, y);
                    MainGrid.Children.Add(border);
                }
            }
        }

        private void VisualizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (board != null)
            {
                DFS DFS = new DFS(board);
                DFS.run();
            }
        }
    }
}
