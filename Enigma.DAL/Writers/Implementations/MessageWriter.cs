using Enigma.Common.Models;
using Enigma.DAL.Writers.Interfaces;
using System.IO;

namespace Enigma.DAL.Writers.Implementations
{
    public class MessageWriter : IWriter<Message>
    {
        public void Write(string path, Message data)
        {
            using (var binaryWriter = new BinaryWriter(File.OpenWrite(path)))
            {
                binaryWriter.Write(data.SenderId);
                binaryWriter.Write(data.ReceiverId);
                binaryWriter.Write(data.Date.Ticks);
                binaryWriter.Write(data.Text);
            }
        }
    }
}
