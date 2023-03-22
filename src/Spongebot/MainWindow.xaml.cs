using Microsoft.Win32;
using Spongebot.Enums;
using Spongebot.Exceptions;
using Spongebot.IO;
using Spongebot.Objects;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Spongebot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Board board;
        private readonly SolidColorBrush[] cellColors = {
            Brushes.Gray, // CellType.Wall
            Brushes.Red, // CellType.Empty
            Brushes.Green, // CellType.Start
            Brushes.Yellow // CellType.Treasure
        };

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
                    FileIO configFile = new FileIO(fileDialog.FileName, fileDialog.SafeFileName);
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

            // add rows and columns to the grid
            for (int y = 0; y < board.Cells.GetLength(0); y++)
            {
                RowDefinition gridRow = new RowDefinition();
                gridRow.Height = new GridLength(45);
                MainGrid.RowDefinitions.Add(gridRow);
            }
            for (int x = 0; x < board.Cells.GetLength(1); x++)
            {
                ColumnDefinition gridCol = new ColumnDefinition();
                gridCol.Width = new GridLength(45);
                MainGrid.ColumnDefinitions.Add(gridCol);
            }

            // add cells to the grid
            for (int y = 0; y < board.Cells.GetLength(1); y++)
            {
                for (int x = 0; x < board.Cells.GetLength(0); x++)
                {
                    Cell cell = board.Cells[x, y];
                    Border border = new Border();
                    if (cell.Type == CellType.Start)
                    {
                        TextBlock txtBlock1 = new TextBlock();
                        txtBlock1.Text = "Start";
                        txtBlock1.FontSize = 14;
                        Grid.SetColumn(txtBlock1, x);
                        Grid.SetRow(txtBlock1, y);
                        border.Child = txtBlock1;
                        border.Background = Brushes.AliceBlue;
                    }
                    else if (cell.Type == CellType.Treasure)
                    {
                        TextBlock txtBlock1 = new TextBlock();
                        txtBlock1.Text = "Treasure";
                        txtBlock1.FontSize = 14;
                        Grid.SetColumn(txtBlock1, x);
                        Grid.SetRow(txtBlock1, y);
                        border.Child = txtBlock1;
                        border.Background = Brushes.Orange;
                    }
                    else if (cell.Type == CellType.Empty)
                    {
                        border.Background = Brushes.White;
                    }
                    else
                    {
                        border.Background = Brushes.Black;
                    }
                    Grid.SetColumn(border, x);
                    Grid.SetRow(border, y);
                    MainGrid.Children.Add(border);
                }
            }
        }

        //private void DrawBoard()
        //{
        //    DataTable boardData = new DataTable();
        //    //dataGridBoard.Width = 300;


        //    for (int x = 0; x < board.Cells.GetLength(1); x++)
        //    {
        //        boardData.Columns.Add(new DataColumn("Column" + x, typeof(string)));
        //    }

        //    for (int i = 0; i < board.Cells.GetLength(0); i++)
        //    {
        //        DataRow row = boardData.NewRow();
        //        for (int j = 0; j < board.Cells.GetLength(1); j++)
        //        {
        //            CellType tes = board.Cells[i, j].Type;
        //            if (tes == CellType.Wall)
        //            {
        //                row[j] = "XXX";
        //            }
        //            else if (tes == CellType.Start)
        //            {
        //                row[j] = "Start";
        //            }
        //            else if (tes == CellType.Empty)
        //            {
        //                row[j] = "";
        //            }
        //            else
        //            {
        //                row[j] = "Treasure!";
        //            }
        //            //row[j] = board.Cells[i, j];
        //            //dataGridBoard.Items.Refresh();
        //        }
        //        boardData.Rows.Add(row);
        //    }

        //    dataGridBoard.DataContext = boardData;
        //}
    }
}
