using System;
using System.Collections.Generic;
using Spongebot.Enums;

namespace Spongebot.Objects
{
    internal class DFSPath
    {
        private Cell[] path;
        public Stack<Cell> prevCells = new Stack<Cell>();
        public int Length {
            get { return path.Length; }
        }
        public int treasureCount { get; }

        public DFSPath(params Cell[] cells)
        {
            path = new Cell[cells.Length];
            Array.Copy(cells, path, cells.Length);
            treasureCount = countTreasures(path);
            foreach (var cell in cells){
                prevCells.Push(cell);
            }
        }

        public DFSPath(DFSPath _path, params Cell[] cells) {
            path = new Cell[_path.Length + cells.Length];
            Array.Copy(_path.path, path, _path.Length);
            Array.Copy(cells, 0, path, _path.Length, cells.Length);
            treasureCount = countTreasures(path);
            prevCells = CloneStack(_path.prevCells);
            foreach (var cell in cells){
                prevCells.Push(cell);
            }
        }

        public Stack<Cell> CloneStack(Stack<Cell> original){
            var arr = new Cell[original.Count];
            original.CopyTo(arr, 0);
            Array.Reverse(arr);
            return new Stack<Cell>(arr);
        }

        private int countTreasures(Cell[] cells)
        {
            int count = 0;
            Queue<Cell> temp = new Queue<Cell>();
            foreach (var cell in cells){
                if (cell.Type == CellType.Treasure){
                    if(temp.Count==0){
                        temp.Enqueue(cell);
                        count++;
                    }else{
                        bool dupe = false;
                        foreach(var tempcell in temp){
                            if(tempcell==cell){
                                dupe = true;
                                break;
                            }
                        }
                        if(!dupe){
                            temp.Enqueue(cell);
                            count++;
                        }
                    }
                }
            }
            return count;
        }

        public Cell this[int index]
        {
            get { return path[index]; }
        }

    }
}