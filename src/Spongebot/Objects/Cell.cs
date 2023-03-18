using Spongebot.Enums;
namespace Spongebot.Objects;

class Cell
{
    public bool Visited { get; set; }
    public CellType Type { get; }

    public Cell(CellType _Type)
    {
        this.Visited = false;
        this.Type = _Type;
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