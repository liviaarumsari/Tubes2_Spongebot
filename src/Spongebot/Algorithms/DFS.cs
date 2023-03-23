using Spongebot.Objects;
using Spongebot.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Windows.Media;
using System.Diagnostics;

namespace Spongebot.Algorithms
{
    internal class DFS
    {
        private Board board;
        private Cell startCell;
        private List<Cell> treasureCells = new List<Cell>();
        public string finalRoute = new String("");
        public int visitedNodes;
        public int totalSteps ;

        public DFS(Board board)
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
        private string route(Cell cell1, Cell cell2){
            if(cell1.Position.X==cell2.Position.X-1){
                return "R";
            }else if (cell1.Position.X==cell2.Position.X+1){
                return "L";
            }else if(cell1.Position.Y==cell2.Position.Y-1){
                return "D";
            }
            return "U";
        }
        public async Task run(bool isTSP, double timeInterval)
        {
            board.clearColors();

            this.visitedNodes = 0;
            this.totalSteps = 0;
            this.finalRoute = "";

            Stack<MazePath> pathS = new Stack<MazePath>();
            MazePath initialPath = new MazePath(startCell);
            pathS.Push(initialPath);

            bool deadend = true;
            int countVisit = 0;
            while (pathS.Count != 0){
                MazePath currentPath = pathS.Pop();
                Cell lastCell = currentPath[currentPath.Length - 1];

                currentPath.clearColor();
                currentPath.stepColor();
                lastCell.stepPathVisitingColor();
                await Task.Delay(TimeSpan.FromMilliseconds(timeInterval));

                if (currentPath.treasureCount == treasureCells.Count)
                {
                    string s = new String("");

                    
                    for (int i = 0; i < currentPath.Length - 2;i++){
                        s += route(currentPath[i], currentPath[i + 1]);
                        s += "-";
                    }
                    s+=route(currentPath[currentPath.Length-2], currentPath[currentPath.Length-1]);
                    
                    board.clearColors();
                    for (int i = 0; i < currentPath.Length; i++)
                        {
                        currentPath[i].finalPathVisitedColor();
                        await Task.Delay(TimeSpan.FromMilliseconds(timeInterval));
                    }
                    
                    this.finalRoute = s;
                    this.totalSteps = currentPath.Length-1;
                    this.visitedNodes += countVisit;
                    Debug.WriteLine("route: "+this.finalRoute);
                    Debug.WriteLine("steps: "+this.totalSteps);
                    Debug.WriteLine("node: "+this.visitedNodes);
                    if (isTSP)
                    {
                        MazePath tspPath = new MazePath(TSP(currentPath[currentPath.Length - 1]));
                        for (int i = 0; i < tspPath.Length; i++)
                        {
                        tspPath[i].TSPfinalPathVisitedColor();
                        await Task.Delay(TimeSpan.FromMilliseconds(timeInterval));
                    }
                    Debug.WriteLine("route after: "+this.finalRoute);
                    Debug.WriteLine("steps after: "+this.totalSteps);
                    Debug.WriteLine("node after: "+this.visitedNodes);
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
                
                //checking neighbors cell
                deadend = true;
                foreach (var neighborPosition in neighborPositions)
                {
                    if (board.isValidPosition(neighborPosition) && !cellIsVisited(board[neighborPosition], currentPath) && board[neighborPosition].Type != CellType.Wall)
                    {
                        Cell neighbor = board[neighborPosition];
                        pathS.Push(new MazePath(currentPath, neighbor));
                        deadend = false;
                    }
                }

                // if deadend then backtrack
                if(deadend){
                    currentPath.prevCells.Pop();
                    Cell previous = currentPath.prevCells.Pop();
                    pathS.Push(new MazePath(currentPath, previous));
                }
                countVisit++;
            }
        }

        public MazePath TSP(Cell lastTreasure){
            Stack<MazePath> pathS = new Stack<MazePath>();
            MazePath initialPath = new MazePath(lastTreasure);
            pathS.Push(initialPath);
            MazePath currentPath = pathS.Peek();

            bool deadend = true;
            int countVisit = 0;
            while (pathS.Count != 0){
                currentPath = pathS.Pop();
                Cell lastCell = currentPath[currentPath.Length - 1];

                if (lastCell == startCell)
                {
                    string s = new String("-");
                    for (int i = 0; i < currentPath.Length - 2;i++){
                        s += route(currentPath[i], currentPath[i + 1]);
                        s += "-";
                    }
                    s+=route(currentPath[currentPath.Length-2], currentPath[currentPath.Length-1]);
                    Debug.WriteLine("route back: "+s);
                    Debug.WriteLine("step back: "+(currentPath.Length-1));
                    this.finalRoute += s;
                    this.totalSteps += currentPath.Length - 1;
                    break;
                    
                }

                Point[] neighborPositions = new Point[]
                {
                    new Point(lastCell.Position.X, lastCell.Position.Y - 1),
                    new Point(lastCell.Position.X + 1, lastCell.Position.Y),
                    new Point(lastCell.Position.X, lastCell.Position.Y + 1),
                    new Point(lastCell.Position.X - 1, lastCell.Position.Y)
                };
                
                //checking neighbors cell
                deadend = true;
                foreach (var neighborPosition in neighborPositions)
                {
                    if (board.isValidPosition(neighborPosition) && !cellIsVisited(board[neighborPosition], currentPath) && board[neighborPosition].Type != CellType.Wall)
                    {
                        Cell neighbor = board[neighborPosition];
                        pathS.Push(new MazePath(currentPath, neighbor));
                        deadend = false;
                    }
                }

                // if deadend then backtrack
                if(deadend){
                    currentPath.prevCells.Pop();
                    Cell previous = currentPath.prevCells.Pop();
                    pathS.Push(new MazePath(currentPath, previous));
                }
                countVisit++;
            }
            this.visitedNodes += countVisit;
            return currentPath;
        }
    }
}
