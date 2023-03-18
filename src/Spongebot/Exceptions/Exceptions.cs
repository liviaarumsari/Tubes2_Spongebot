using System.IO;

namespace Spongebot.Exceptions;

class InvalidFileFormatException : IOException
{
    public InvalidFileFormatException()
        : base() { }

    public InvalidFileFormatException(string message)
        : base(message) { }
}