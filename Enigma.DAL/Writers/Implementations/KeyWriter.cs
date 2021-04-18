using Enigma.DAL.Writers.Interfaces;
using System;
using System.IO;

namespace Enigma.DAL.Writers.Implementations
{
    public class KeyWriter : IWriter<int>
    {
        public void Write(string path, int data)
        {
            using (var binaryWriter = new BinaryWriter(File.OpenWrite(path)))
            {
                var random = new Random();

                var encryptionKey = random.Next();

                binaryWriter.Write(random.Next());
                binaryWriter.Write(encryptionKey);
                binaryWriter.Write(encryptionKey ^ data);
            }
        }
    }
}
