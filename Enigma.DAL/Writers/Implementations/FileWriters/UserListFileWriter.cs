using Enigma.DAL.Writers.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace Enigma.DAL.Writers.Implementations.FileWriters
{
    public class UserListFileWriter : IWriter<KeyValuePair<int, string>>
    {
        public void Write(string path, KeyValuePair<int, string> data)
        {
            using (var binaryWriter = new BinaryWriter(File.Open(path, FileMode.Append)))
            {
                binaryWriter.Write(data.Key);
                binaryWriter.Write(data.Value);
            }
        }
    }
}
