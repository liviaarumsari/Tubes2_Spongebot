using Spongebot.Enums;
using System.Diagnostics;

namespace Spongebot.Objects;

class Board
{
    public Cell[,] Cells { get; }

    public Board(Cell[,] _cells)
    {
        Cells = new Cell[_cells.GetLength(0), _cells.GetLength(1)];
        for (int y = 0; y < _cells.GetLength(1); y++)
        {
            for (int x = 0; x < _cells.GetLength(0); x++)
            {
                Cells[x,y] = _cells[x,y];
            }
        }
    }

    public Board(Board other) : this(other.Cells) { }

    public void print()
    {
        for (int y = 0; y < Cells.GetLength(1); y++)
        {
            for (int x = 0; x < Cells.GetLength(0); x++)
            {
                Debug.Write(Cells[x, y].toString() + " ");
            }
            Debug.WriteLine("");
        }
        Debug.WriteLine("");
    }

    public void clearColors()
    {
        for (int y = 0; y < Cells.GetLength(1); y++)
        {
            for (int x = 0; x < Cells.GetLength(0); x++)
            {
                if (Cells[x, y].Type != CellType.Wall)
                    Cells[x, y].clearColor();
            }
        }
    }

    public bool isValidPosition(int x, int y)
    {
        return x < Cells.GetLength(0) && x >= 0 && y < Cells.GetLength(1) && y >= 0;
    }

    public bool isValidPosition(Point p)
    {
        return isValidPosition(p.X, p.Y);
    }

    public Cell this[Point p]
    {
        get { return Cells[p.X, p.Y]; }
    }
}