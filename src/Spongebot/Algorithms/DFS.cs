using Spongebot.Objects;
using Spongebot.Enums;
using System.Collections.Generic;

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
                    return false;
            }
            return true;
        }

        public void run()
        {
            DFSPath initialPath = new DFSPath(startCell);
            pathS.Push(initialPath);

            while (pathS.Count != 0)
            {
                DFSPath currentPath = pathS.Pop();
                Cell lastCell = currentPath[currentPath.Length - 1];

                if (currentPath.treasureCount == treasureCells.Count)
                {
                    // Found
                    return;
                }

                Point[] neighborPositions = new Point[]
                {
                    new Point(lastCell.Position.X - 1, lastCell.Position.Y),
                    new Point(lastCell.Position.X, lastCell.Position.Y - 1),
                    new Point(lastCell.Position.X + 1, lastCell.Position.Y),
                    new Point(lastCell.Position.X, lastCell.Position.Y + 1)
                };

                foreach (var neighborPosition in neighborPositions)
                {
                    if (board.isValidPosition(neighborPosition) && !cellIsVisited(board[neighborPosition], currentPath))
                    {
                        Cell neighbor = board[neighborPosition];
                        pathS.Push(new DFSPath(currentPath, neighbor));
                    }
                }
            }
        }
    }
}
