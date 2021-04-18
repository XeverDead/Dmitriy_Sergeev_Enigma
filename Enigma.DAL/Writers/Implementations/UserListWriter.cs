using Enigma.DAL.Writers.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Enigma.DAL.Writers.Implementations
{
    public class UserListWriter : IWriter<KeyValuePair<int, string>>
    {
        public void Write(string path, KeyValuePair<int, string> data)
        {
            var random = new Random();

            using (var binaryWriter = new BinaryWriter(File.Open(path, FileMode.Append)))
            {
                var encryptionKey = random.Next(10, 1000);

                binaryWriter.Write(data.Key ^ encryptionKey);
                binaryWriter.Write(encryptionKey);
                binaryWriter.Write(EncryptString(data.Value, encryptionKey));
            }
        }

        private string EncryptString(string data, int key)
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
