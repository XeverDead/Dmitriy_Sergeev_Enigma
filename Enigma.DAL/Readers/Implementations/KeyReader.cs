using Enigma.DAL.Readers.Interfaces;
using System.IO;

namespace Enigma.DAL.Readers.Implementations
{
    public class KeyReader : IReader<int>
    {
        public int Read(string path)
        {
            using (var binaryReader = new BinaryReader(File.OpenRead(path)))
            {
                binaryReader.ReadInt32();
                var encryptionKey = binaryReader.ReadInt32();

                return encryptionKey ^ binaryReader.ReadInt32();
            }
        }
    }
}
