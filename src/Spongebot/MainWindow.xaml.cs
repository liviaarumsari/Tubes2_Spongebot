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

            int n_column = board.Cells.GetLength(0);
            int n_row = board.Cells.GetLength(1);
            var gridLen = n_column > n_row ? 400/n_column : 400/n_row;

            // add rows and columns to the grid
            for (int y = 0; y < n_column; y++)
            {
                RowDefinition gridRow = new RowDefinition();
                gridRow.Height = new GridLength(gridLen);
                MainGrid.RowDefinitions.Add(gridRow);
            }
            for (int x = 0; x < n_row; x++)
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
    }
}
