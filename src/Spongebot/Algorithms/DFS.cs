using Spongebot.Objects;
using Spongebot.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Windows.Media;

namespace Spongebot.Algorithms
{
    internal class DFS
    {
        private Board board;
        private Cell startCell;
        private List<Cell> treasureCells = new List<Cell>();
        private Stack<DFSPath> pathS = new Stack<DFSPath>();

        public DFS(Board board)
        {
            this.board = board;
            startCell = null!;
            for (int x = 0; x < board.Cells.GetLength(0); x++)
            {
                for (int y = 0; y < board.Cells.GetLength(1); y++)
                {
                    if (board.Cells[x, y].Type == CellType.Start)
                        startCell = board.Cells[x, y];
                }
            }

            for (int x = 0; x < board.Cells.GetLength(0); x++)
            {
                for (int y = 0; y < board.Cells.GetLength(1); y++)
                {
                    if (board.Cells[x, y].Type == CellType.Treasure)
                    {
                        treasureCells.Add(board.Cells[x, y]);
                    }
                }
            }
        }

        private bool cellIsVisited(Cell cell, DFSPath path)
        {
            for (int i = 0; i < path.Length; i++)
            {
                if (cell == path[i])
                    return true;
            }
            return false;
        }

        public async void run()
        {
            DFSPath initialPath = new DFSPath(startCell);
            pathS.Push(initialPath);
            
            while (pathS.Count != 0){
                bool deadend = true;
                DFSPath currentPath = pathS.Pop();
                Cell lastCell = currentPath[currentPath.Length - 1];
   
                if(currentPath[currentPath.Length - 1].CellBackground == Brushes.Cornsilk){currentPath[currentPath.Length - 1].CellBackground = Brushes.PeachPuff;
                }else{
                    currentPath[currentPath.Length - 1].CellBackground = Brushes.Cornsilk;
                }
                await Task.Delay(TimeSpan.FromMilliseconds(500));

                if (currentPath.treasureCount == treasureCells.Count)
                {
                    for (int i = 0; i < currentPath.Length; i++)
                    {
                        if (currentPath[i].CellBackground == Brushes.Green)
                        {

                            currentPath[i].CellBackground = Brushes.DarkGreen;
                        }
                        else if(currentPath[i].CellBackground != Brushes.DarkGreen)
                        {
                            currentPath[i].CellBackground = Brushes.Green;
                        }
                        await Task.Delay(TimeSpan.FromMilliseconds(500));
                    }
                    return;
                }

                Point[] neighborPositions = new Point[]
                {
                    new Point(lastCell.Position.X - 1, lastCell.Position.Y),
                    new Point(lastCell.Position.X, lastCell.Position.Y + 1),
                    new Point(lastCell.Position.X + 1, lastCell.Position.Y),
                    new Point(lastCell.Position.X, lastCell.Position.Y - 1)
                };

                foreach (var neighborPosition in neighborPositions)
                {
                    if (board.isValidPosition(neighborPosition) && !cellIsVisited(board[neighborPosition], currentPath) && board[neighborPosition].Type != CellType.Wall)
                    {
                        Cell neighbor = board[neighborPosition];
                        pathS.Push(new DFSPath(currentPath, neighbor));
                        deadend = false;
                    }
                }
                if(deadend){
                    currentPath.prevCells.Pop();
                    Cell previous = currentPath.prevCells.Pop();
                    pathS.Push(new DFSPath(currentPath, previous));
                }

            }
        }
    }
}
