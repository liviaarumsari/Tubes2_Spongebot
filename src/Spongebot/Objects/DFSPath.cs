using System;
using Spongebot.Enums;

namespace Spongebot.Objects
{
    internal class DFSPath
    {
        private Cell[] path;
        public int Length { get; }
        public int treasureCount { get; }

        public DFSPath(params Cell[] cells)
        {
            path = new Cell[cells.Length];
            Array.Copy(cells, path, cells.Length);
            treasureCount = countTreasures(path);
        }

        public DFSPath(DFSPath _path, params Cell[] cells) {
            path = new Cell[_path.Length + cells.Length];
            Array.Copy(_path.path, path, _path.Length);
            Array.Copy(cells, 0, path, _path.Length, cells.Length);
            treasureCount = countTreasures(path);
        }

        private int countTreasures(Cell[] cells)
        {
            int count = 0;
            foreach (var cell in cells)
            {
                if (cell.Type == CellType.Treasure)
                    count++;
            }
            return count;
        }

        public Cell this[int index]
        {
            get { return path[index]; }
        }
    }
}