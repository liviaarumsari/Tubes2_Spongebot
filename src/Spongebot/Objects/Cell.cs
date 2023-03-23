using Spongebot.Enums;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Spongebot.Objects;

class Cell : INotifyPropertyChanged
{
    public CellType Type { get; }
    public Point Position { get; }

    private SolidColorBrush cellBackground;
    public SolidColorBrush CellBackground
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
        SolidColorBrush greenBrush = new SolidColorBrush(Color.FromRgb(93, 190, 116));
        if (CellBackground is SolidColorBrush solidColorBrush && solidColorBrush.Color.Equals(greenBrush.Color) && solidColorBrush.Opacity > 0.25)
        {
            CellBackground = new SolidColorBrush(greenBrush.Color) { Opacity = solidColorBrush.Opacity - 0.25 };
        }
        else if (CellBackground.Equals(Brushes.White))
        {
            CellBackground = new SolidColorBrush(greenBrush.Color) { Opacity = 1 };
        }
    }

    public void stepPathVisitedColor()
    {
        SolidColorBrush yellowBrush = new SolidColorBrush(Color.FromRgb(240, 220, 60));
        if (CellBackground is SolidColorBrush solidColorBrush && solidColorBrush.Color.Equals(yellowBrush.Color) && solidColorBrush.Opacity > 0.25)
        {
            CellBackground = new SolidColorBrush(yellowBrush.Color) { Opacity = solidColorBrush.Opacity - 0.25 };
        }
        else if (CellBackground.Equals(Brushes.White))
        {
            CellBackground = new SolidColorBrush(yellowBrush.Color) { Opacity = 1 };
        }
    }

    public void stepPathVisitingColor()
    {
        SolidColorBrush blueBrush = new SolidColorBrush(Color.FromRgb(161, 191, 250));
        CellBackground = blueBrush;
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