using Spongebot.Enums;
using System.Windows.Media;

namespace Spongebot.Objects;

class Cell
{
    public CellType Type { get; }
    public Point Position { get; }

    public Brush CellBackground { get; set; }

    public Cell(int x, int y, CellType _Type)
    {
        this.Type = _Type;
        Position = new Point(x, y);
        if (this.Type == CellType.Wall)
        {
            CellBackground = Brushes.Black;
        }
        else
        {
            CellBackground = Brushes.Transparent;
        }
    }

    public string toString()
    {
        if (Type == CellType.Start)
        {
            return "K";
        }
        else if (Type == CellType.Empty)
        {
            return "R";
        }
        else if (Type == CellType.Treasure)
        {
            return "T";
        }
        return "X";
    }
}