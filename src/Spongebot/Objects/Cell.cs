using Spongebot.Enums;
using System.ComponentModel;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Spongebot.Objects;

class Cell : INotifyPropertyChanged
{
    public CellType Type { get; }
    public Point Position { get; }

    private Brush cellBackground;
    public Brush CellBackground
    {
        get { return cellBackground; }
        set
        {
            if (value != cellBackground)
            {
                cellBackground = value;
                OnPropertyChanged(nameof(CellBackground));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

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
            CellBackground = Brushes.White;
        }
    }

    public void clearColor()
    {
        if (this.Type == CellType.Wall)
        {
            CellBackground = Brushes.Black;
        }
        else
        {
            CellBackground = Brushes.White;
        }
    }

    public void finalPathVisitedColor()
    {
        if (CellBackground == Brushes.LightGreen)
        {
            CellBackground = Brushes.Green;
        }
        else if (CellBackground != Brushes.Green)
        {
            CellBackground = Brushes.LightGreen;
        }
    }

    public void stepPathVisitedColor()
    {
        if (CellBackground == Brushes.Cornsilk)
        {
            CellBackground = Brushes.PeachPuff;
        }
        else if (CellBackground != Brushes.PeachPuff)
        {
            CellBackground = Brushes.Cornsilk;
        }
    }

    public void stepPathVisitingColor()
    {
        CellBackground = Brushes.LightBlue;
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