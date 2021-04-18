using Enigma.DAL.Readers.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Enigma.DAL.Readers.Implementations
{
    public class UserListReader : IReader<Dictionary<int, string>>
    {
        public Dictionary<int, string> Read(string path)
        {
            var userList = new Dictionary<int, string>();

            using (var binaryReader = new BinaryReader(File.OpenRead(path)))
            {
                while (binaryReader.PeekChar() != -1)
                {
                    var id = binaryReader.ReadInt32();
                    var encrytionKey = binaryReader.ReadInt32();
                    var login = binaryReader.ReadString();

                    userList.Add(id ^ encrytionKey, DecryptString(login, encrytionKey));
                }
            }

            return userList;
        }

        private string DecryptString(string data, int key)
        {
            var resultBuilder = new StringBuilder();

            foreach (var character in data)
            {
                resultBuilder.Append((char)(character ^ key));
            }

            return resultBuilder.ToString();
        }
    }
}
