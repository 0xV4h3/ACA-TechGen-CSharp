using System.Text;
using Simulation.Models;

namespace Simulation.Utils;

public static class ByteArrayExtension
{
    public static Player? ToPlayer(this byte[] bytes)
    {
        if (bytes == null || bytes.Length == 0) 
            return null;

        using (var stream = new MemoryStream(bytes))
        using (var reader = new BinaryReader(stream, Encoding.UTF8))
        {
            int id = reader.ReadInt32();
            string name = reader.ReadString();
            int lvl = reader.ReadInt32();
                
            double posX = reader.ReadDouble();
            double posY = reader.ReadDouble();
                
            return new Player
            {
                Id = id,
                Name = name,
                Lvl = lvl,
                Coords = new Point(posX, posY)
            };
        }
    }
}