using Enigma.BLL.Encryptors.Interfaces;
using Enigma.Common.Models;
using Enigma.Common.Settings;
using Enigma.DAL.Readers.Interfaces;
using Enigma.DAL.Writers.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace Enigma.BLL.Services
{
    public class MessageService
    {
        private readonly IReader<Message> messageReader;
        private readonly IWriter<Message> messageWriter;

        private readonly IReader<Dictionary<long, string>> userListReader;

        private readonly IEncryptor messageEncryptor;

        public MessageService(IReader<Message> messageReader, 
            IWriter<Message> messageWriter, 
            IReader<Dictionary<long, string>> userListReader, 
            IEncryptor messageEncryptor)
        {
            this.messageReader = messageReader ?? throw new ArgumentNullException(nameof(messageReader));
            this.messageWriter = messageWriter ?? throw new ArgumentNullException(nameof(messageWriter));
            this.userListReader = userListReader ?? throw new ArgumentNullException(nameof(userListReader));
            this.messageEncryptor = messageEncryptor ?? throw new ArgumentNullException(nameof(messageEncryptor));
        }

        public Message ReadMessage(string path)
        {
            var message = messageReader.Read(path);

            message.Text = messageEncryptor.Decrypt(message.Text);

            return message;
        }

        public void SendMessage(Message message)
        {
            if (!CheckUserId(message.SenderId))
            {
                throw new ArgumentException("There no user with this sender id", nameof(message));
            }

            if (!CheckUserId(message.ReceiverId))
            {
                throw new ArgumentException("There no user with this receiver id", nameof(message));
            }

            var messageDirectory = Path.Combine(EnigmaSettings.MainDirectory, message.SenderId.ToString(), message.ReceiverId.ToString());

            if (!Directory.Exists(messageDirectory))
            {
                Directory.CreateDirectory(messageDirectory);
            }

            var messagePath = Path.Combine(messageDirectory, message.Date.Ticks.ToString() + $".{EnigmaSettings.MessageExtension}");

            var encryptedMessage = message.Clone() as Message;

            encryptedMessage.Text = messageEncryptor.Encrypt(encryptedMessage.Text);

            messageWriter.Write(messagePath, encryptedMessage);
        }

        private bool CheckUserId(long id)
        {
            return userListReader.Read(EnigmaSettings.UserListPath).ContainsKey(id);
        }
    }
}
