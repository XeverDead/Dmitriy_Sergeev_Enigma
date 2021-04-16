using Enigma.Common.Models;
using Enigma.DAL.Writers.Interfaces;
using System.IO;

namespace Enigma.DAL.Writers.Implementations.FileWriters
{
    public class UserDataFileWriter : IWriter<UserCredentials>
    {
        public void Write(string path, UserCredentials data)
        {
            using (var binaryWriter = new BinaryWriter(File.OpenWrite(path)))
            {
                binaryWriter.Write(data.Login.Length);

                foreach (var symbol in data.Login)
                {
                    binaryWriter.Write(symbol);
                }

                binaryWriter.Write(data.Password.Length);

                foreach (var symbol in data.Password)
                {
                    binaryWriter.Write(symbol);
                }
            }
        }
    }
}
