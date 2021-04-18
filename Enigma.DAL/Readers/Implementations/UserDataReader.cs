using Enigma.Common.Models;
using Enigma.DAL.Readers.Interfaces;
using System.IO;
using System.Text;

namespace Enigma.DAL.Readers.Implementations
{
    public class UserDataReader : IReader<UserCredentials>
    {
        public UserCredentials Read(string path)
        {
            var credentials = new UserCredentials();

            using (var binaryReader = new BinaryReader(File.OpenRead(path)))
            {
                binaryReader.ReadInt32();

                var loginKey = binaryReader.ReadInt32();
                var login = binaryReader.ReadString();
                var password = binaryReader.ReadString();
                var passwordKey = binaryReader.ReadInt32();

                credentials.Login = DecryptString(login, loginKey);
                credentials.Password = DecryptString(password, passwordKey);
            }

            return credentials;
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
