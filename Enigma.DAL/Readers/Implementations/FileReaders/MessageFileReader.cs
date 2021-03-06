using Enigma.Common.Models;
using Enigma.DAL.Readers.Interfaces;
using System;
using System.IO;

namespace Enigma.DAL.Readers.Implementations.FileReaders
{
    public class MessageFileReader : IReader<Message>
    {
        public Message Read(string path)
        {
            var message = new Message();

            using (var binaryReader = new BinaryReader(File.OpenRead(path)))
            {
                message.SenderId = binaryReader.ReadInt64();
                message.ReceiverId = binaryReader.ReadInt64();
                message.Date = new DateTime(binaryReader.ReadInt64());
                message.Text = binaryReader.ReadString();
            }

            return message;
        }
    }
}
