using Enigma.DAL.Readers.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace Enigma.DAL.Readers.Implementations
{
    public class UserListReader : IReader<Dictionary<long, string>>
    {
        public Dictionary<long, string> Read(string path)
        {
            var userList = new Dictionary<long, string>();

            using (var binaryReader = new BinaryReader(File.OpenRead(path)))
            {
                while (binaryReader.PeekChar() != -1)
                {
                    var id = binaryReader.ReadInt64();
                    var login = binaryReader.ReadString();

                    userList.Add(id, login);
                }
            }

            return userList;
        }
    }
}
