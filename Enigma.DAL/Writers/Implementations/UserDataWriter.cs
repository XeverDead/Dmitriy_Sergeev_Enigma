using Enigma.Common.Models;
using Enigma.DAL.Writers.Interfaces;
using System;
using System.IO;
using System.Text;

namespace Enigma.DAL.Writers.Implementations
{
    public class UserDataWriter : IWriter<UserCredentials>
    {
        public void Write(string path, UserCredentials data)
        {
            var random = new Random();

            using (var binaryWriter = new BinaryWriter(File.OpenWrite(path)))
            {
                var loginKey = random.Next(10, 1000);
                var passwordKey = random.Next(10, 1000);

                binaryWriter.Write(random.Next());
                binaryWriter.Write(loginKey);
                binaryWriter.Write(EncryptString(data.Login, loginKey));
                binaryWriter.Write(EncryptString(data.Password, passwordKey));
                binaryWriter.Write(passwordKey);
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
