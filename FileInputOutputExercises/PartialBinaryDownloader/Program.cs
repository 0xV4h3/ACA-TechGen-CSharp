using System.Text;

namespace PartialBinaryDownloader;

class Program
{
    static void Main(string[] args)
    {
        string dir = Path.Combine(Environment.CurrentDirectory, "partial-download.bin");

        byte[] blockA = Encoding.UTF8.GetBytes("Geralt of Rivia : Geralt is a witcher, a magical mutant made for hunting and killing monsters");
        byte[] blockB = Encoding.UTF8.GetBytes("Yennefer of Vengerberg : Yennefer is a powerful sorceress, the true love of the witcher Geralt of Rivia, and a fierce, motherly figure to the young Ciri");

        using (var fs = new FileStream(dir, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
        {
            fs.Position = 0;
            fs.Write(blockA, 0, blockA.Length);

            fs.Seek(1024, SeekOrigin.Begin);
            fs.Write(blockB, 0, blockB.Length);

            fs.Flush();
        }

        bool isACorrect;
        bool isBCorrect;

        using (var fs = new FileStream(dir, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            byte[] readA = new byte[blockA.Length];
            fs.Position = 0;
            fs.Read(readA, 0, readA.Length);

            byte[] readB = new byte[blockB.Length];
            fs.Position = 1024;
            fs.Read(readB, 0, readB.Length);

            isACorrect = readA.SequenceEqual(blockA);
            isBCorrect = readB.SequenceEqual(blockB);
        }

        Console.WriteLine($"Block A correct: {isACorrect}");
        Console.WriteLine($"Block B correct: {isBCorrect}");
    }
}