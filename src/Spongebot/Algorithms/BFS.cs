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
        public string finalRoute = new String("");
        public int visitedNodes;
        public int totalSteps ;
        public BFS(Board board)
        {
            this.board = board;
            this.visitedNodes = 0;
            this.totalSteps = 0;
            this.finalRoute = "";
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

        private bool cellIsVisited(Cell cell, MazePath path)
        {
            for (int i = 0; i < path.Length; i++)
            {
                if (cell == path[i])
                    return true;
            }
            return false;
        }

        public async Task runNonTSP(double timeInterval)
        {
            board.clearColors();

            MazePath initialPath = new MazePath(startCell);
            List<MazePath> completePath = new List<MazePath> { initialPath };
            HashSet<Cell> unvisitedTreasure = new HashSet<Cell>(treasureCells);

            Queue<MazePath> pathQ = new Queue<MazePath>();
            pathQ.Enqueue(initialPath);

            while (pathQ.Count != 0)
            {
                MazePath currentPath = pathQ.Dequeue();
                Cell lastCell = currentPath[currentPath.Length - 1];

                foreach (var path in completePath)
                {
                    path.stepColor();
                }
                currentPath.stepColor();
                lastCell.stepPathVisitingColor();

                await Task.Delay(TimeSpan.FromMilliseconds(timeInterval));

                board.clearColors();

                if (unvisitedTreasure.Contains(lastCell))
                {
                    completePath.Add(currentPath);
                    pathQ.Clear();
                    unvisitedTreasure.Remove(lastCell);

                    currentPath = new MazePath();
                }

                if (unvisitedTreasure.Count == 0)
                {
                    foreach (var path in completePath)
                    {
                        for (int i = 0; i < path.Length; i++)
                        {
                            path[i].finalPathVisitedColor();
                            await Task.Delay(TimeSpan.FromMilliseconds(timeInterval));
                        }
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

                foreach (var neighborPosition in neighborPositions)
                {
                    if (board.isValidPosition(neighborPosition) && !cellIsVisited(board[neighborPosition], currentPath) && board[neighborPosition].Type != CellType.Wall)
                    {
                        Cell neighbor = board[neighborPosition];
                        pathQ.Enqueue(new MazePath(currentPath, neighbor));
                    }
                }
            }
        }

        public async Task runTSP(double timeInterval)
        {
            board.clearColors();
            MazePath initialPath = new MazePath(startCell);

            Queue<MazePath> pathQ = new Queue<MazePath>();
            pathQ.Enqueue(initialPath);

            while (pathQ.Count != 0)
            {
                MazePath currentPath = pathQ.Dequeue();
                Cell lastCell = currentPath[currentPath.Length - 1];

                currentPath.stepColor();
                lastCell.stepPathVisitingColor();
                await Task.Delay(TimeSpan.FromMilliseconds(500));
                currentPath.clearColor();

                if (currentPath.treasureCount == treasureCells.Count && lastCell.Type == CellType.Start)
                {
                    for (int i = 0; i < currentPath.Length; i++)
                    {
                        currentPath[i].finalPathVisitedColor();
                        await Task.Delay(TimeSpan.FromMilliseconds(timeInterval));
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
                foreach (var neighborPosition in neighborPositions)
                {
                    if (board.isValidPosition(neighborPosition) && !cellIsVisited(board[neighborPosition], currentPath) && board[neighborPosition].Type != CellType.Wall)
                    {
                        hasNeighborToVisit = true;
                        Cell neighbor = board[neighborPosition];
                        pathQ.Enqueue(new MazePath(currentPath, neighbor));
                    }
                }

                if (!hasNeighborToVisit)
                {
                    currentPath.prevCells.Pop();
                    Cell previous = currentPath.prevCells.Pop();
                    pathQ.Enqueue(new MazePath(currentPath, previous));
                }
            }
        }
    }
}