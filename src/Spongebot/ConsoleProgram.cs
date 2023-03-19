using Spongebot.IO;
using Spongebot.Objects;

namespace Spongebot
{
    internal static class ConsoleProgram
    {
        static void Main()
        {
            FileIO configFile = new FileIO(@"C:\Users\Rinaldy Adin\source\repos\Tubes2_Spongebot\Spongebot\Test\board.txt");
            Board board = configFile.readBoardFromFile();
            board.print();
        }
    }
}
