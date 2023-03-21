using Spongebot.Objects;
using Spongebot.Enums;
using Spongebot.IO;
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
using System.Diagnostics;
using System.Data;

namespace Spongebot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Board board;
        private readonly SolidColorBrush[] cellColors = {
            Brushes.Gray, // CellType.Wall
            Brushes.Red, // CellType.Empty
            Brushes.Green, // CellType.Start
            Brushes.Yellow // CellType.Treasure
        };

        public MainWindow()
        {
            InitializeComponent();

            // initialize your board object here
            FileIO configFile = new FileIO(@"C:\Users\livia\OneDrive - Institut Teknologi Bandung\IF\SEM 4\stima\Tubes 02\test\board1.txt");
            board = configFile.readBoardFromFile();

            // draw the board
            DrawBoard();
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
