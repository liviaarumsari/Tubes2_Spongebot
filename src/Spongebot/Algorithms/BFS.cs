using Spongebot.Objects;
using Spongebot.Enums;
using System.Collections.Generic;
using System.Windows.Media;
using System.Diagnostics;
using System.Threading.Tasks;
using System;

namespace Spongebot.Algorithms
{
    internal class BFS
    {
        private Board board;
        private Cell startCell;
        private List<Cell> treasureCells = new List<Cell>();
        private Queue<BFSPath> pathQ = new Queue<BFSPath>();

        public BFS(Board board)
        {
            this.board = board;

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

        private bool cellIsVisited(Cell cell, BFSPath path)
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
            BFSPath initialPath = new BFSPath(startCell);
            pathQ.Enqueue(initialPath);

            while (pathQ.Count != 0)
            {
                BFSPath currentPath = pathQ.Dequeue();
                Cell lastCell = currentPath[currentPath.Length - 1];

                if (currentPath.treasureCount == treasureCells.Count)
                {
                    for (int i = 0; i < currentPath.Length; i++)
                    {
                        if (currentPath[i].CellBackground == Brushes.Green)
                        {

                            currentPath[i].CellBackground = Brushes.DarkGreen;
                        }
                        else
                        {
                            currentPath[i].CellBackground = Brushes.Green;
                        }
                        await Task.Delay(TimeSpan.FromMilliseconds(500));
                    }
                    return;
                }

                Point[] neighborPositions = new Point[]
                {
                    new Point(lastCell.Position.X, lastCell.Position.Y - 1),
                    new Point(lastCell.Position.X + 1, lastCell.Position.Y),
                    new Point(lastCell.Position.X, lastCell.Position.Y + 1),
                    new Point(lastCell.Position.X - 1, lastCell.Position.Y)
                };

                bool hasNeighborToVisit = false;
                Debug.WriteLine("\n" + lastCell.Position.X + " " + lastCell.Position.Y);
                foreach (var neighborPosition in neighborPositions)
                {
                    Debug.WriteLine(neighborPosition.X + " " + neighborPosition.Y);
                    if (board.isValidPosition(neighborPosition) && !cellIsVisited(board[neighborPosition], currentPath) && board[neighborPosition].Type != CellType.Wall)
                    {
                        hasNeighborToVisit = true;
                        Cell neighbor = board[neighborPosition];
                        pathQ.Enqueue(new BFSPath(currentPath, neighbor));
                    }
                }

                if (!hasNeighborToVisit)
                {
                    currentPath.prevCells.Pop();
                    Cell previous = currentPath.prevCells.Pop();
                    pathQ.Enqueue(new BFSPath(currentPath, previous));
                }
            }
        }
    }
}