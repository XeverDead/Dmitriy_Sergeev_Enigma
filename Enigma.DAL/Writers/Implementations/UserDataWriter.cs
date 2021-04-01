using Enigma.Common.Models;
using Enigma.DAL.Writers.Interfaces;
using System.IO;

namespace Enigma.DAL.Writers.Implementations
{
    public class UserDataWriter : IWriter<UserCredentials>
    {
        public void Write(string path, UserCredentials data)
        {
            using (var binaryWriter = new BinaryWriter(File.OpenWrite(path)))
            {
                binaryWriter.Write(data.Login);
                binaryWriter.Write(data.Password);
                binaryWriter.Write(data.Nickname);
            }
        }
    }
}
