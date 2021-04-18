using Enigma.DAL.Readers.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace Enigma.DAL.Readers.Implementations.FileReaders
{
    public class UserListFileReader : IReader<Dictionary<int, string>>
    {
        public Dictionary<int, string> Read(string path)
        {
            var userList = new Dictionary<int, string>();

            using (var binaryReader = new BinaryReader(File.OpenRead(path)))
            {
                while (binaryReader.PeekChar() != -1)
                {
                    var id = binaryReader.ReadInt32();
                    var login = binaryReader.ReadString();

                    userList.Add(id, login);
                }
            }

            return userList;
        }
    }
}
