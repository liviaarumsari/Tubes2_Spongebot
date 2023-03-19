using Spongebot.Enums;
using Spongebot.Objects;
using Spongebot.Exceptions;
using System.IO;
using System.Collections.Generic;

namespace Spongebot.IO;

class FileIO
{
    private string filePath { get; }

    public FileIO(string _filePath)
    {
        if (!File.Exists(_filePath))
            throw new FileNotFoundException(_filePath + " was not found.");
        this.filePath = _filePath;
    }

    public Board readBoardFromFile()
    {
        string[] lines = File.ReadAllLines(filePath);
        int rows = lines.GetLength(0), cols = 0;
        Cell[,] cells;

        Dictionary<string, CellType> cellCodes = new Dictionary<string, CellType>();
        cellCodes.Add("k", CellType.Start);
        cellCodes.Add("t", CellType.Treasure);
        cellCodes.Add("r", CellType.Empty);
        cellCodes.Add("x", CellType.Wall);

        if (rows == 0)
            throw new InvalidFileFormatException(filePath + " is empty");

        foreach (string c in lines[0].Split(' '))
        {
            if (c != "")
                cols++;
        }

        if (cols == 0)
            throw new InvalidFileFormatException(filePath + " is empty");

        cells = new Cell[rows, cols];

        for (int y = 0; y < rows; y++)
        {
            int colsInLine = 0;
            string[] codes = lines[y].Split(' ');

            for (int x = 0; x < cols; x++)
            {
                string code = codes[x].ToLower();

                if (code.Length == 0)
                    continue;
                if (colsInLine == cols)
                    throw new InvalidFileFormatException(filePath + " has inconsistent number of columns");

                try
                {
                    cells[x, y] = new Cell(x, y, cellCodes[code]);
                }
                catch (KeyNotFoundException)
                {
                    throw new InvalidFileFormatException(filePath + " has unknown cell type code (valid codes: K, T, R, X)"); ;
                }
            }
        }

        return new Board(cells);
    }
}