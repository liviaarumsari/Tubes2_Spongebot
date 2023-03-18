using Spongebot.Enums;
using System.Diagnostics;

namespace Spongebot.Objects;

class Board
{
    private Cell[,] cells;

    public Board(Cell[,] _cells)
    {
        cells = new Cell[_cells.GetLength(0), _cells.GetLength(1)];
        for (int r = 0; r < _cells.GetLength(0); r++)
        {
            for (int c = 0; c < _cells.GetLength(1); c++)
            {
                cells[r,c] = _cells[r,c];
            }
        }
    }

    public Board(Board other) : this(other.cells) { }

    public void print()
    {
        for (int r = 0; r < cells.GetLength(0); r++)
        {
            for (int c = 0; c < cells.GetLength(1); c++)
            {
                Debug.Write(cells[r, c].toString() + " ");
            }
            Debug.WriteLine("");
        }
        Debug.WriteLine("");
    }
}