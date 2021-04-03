using Enigma.DAL.Writers.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace Enigma.DAL.Writers.Implementations
{
    public class UserListWriter : IWriter<KeyValuePair<long, string>>
    {
        public void Write(string path, KeyValuePair<long, string> data)
        {
            using (var binaryWriter = new BinaryWriter(File.Open(path, FileMode.Append)))
            {
                binaryWriter.Write(data.Key);
                binaryWriter.Write(data.Value);
            }
        }
    }
}
