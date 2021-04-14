using Enigma.Common.Models;
using Enigma.DAL.Readers.Interfaces;
using System.IO;

namespace Enigma.DAL.Readers.Implementations
{
    public class UserDataReader : IReader<UserCredentials>
    {
        public UserCredentials Read(string path)
        {
            var credentials = new UserCredentials();

            using (var binaryReader = new BinaryReader(File.OpenRead(path)))
            {
                credentials.Login = binaryReader.ReadString();
                credentials.Password = binaryReader.ReadString();
            }

            return credentials;
        }
    }
}
