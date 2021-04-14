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
        private readonly IReader<Message> _messageReader;
        private readonly IWriter<Message> _messageWriter;

        private readonly IReader<Dictionary<long, string>> _userListReader;

        public MessageService(IReader<Message> messageReader, IWriter<Message> messageWriter, IReader<Dictionary<long, string>> userListReader)
        {
            _messageReader = messageReader ?? throw new ArgumentNullException(nameof(messageReader));
            _messageWriter = messageWriter ?? throw new ArgumentNullException(nameof(messageWriter));
            _userListReader = userListReader ?? throw new ArgumentNullException(nameof(userListReader));
        }

        public IEnumerable<Message> GetDialogMessages(long user1Id, long user2Id)
        {
            CheckUserIds(user1Id, user2Id);

            var user1MessagesDirectory = Path.Combine(EnigmaSettings.MainDirectory, user1Id.ToString(), user2Id.ToString());
            var user2MessagesDirectory = Path.Combine(EnigmaSettings.MainDirectory, user2Id.ToString(), user1Id.ToString());

            var messages = new List<Message>();

            if (!Directory.Exists(user1MessagesDirectory))
            {
                Directory.CreateDirectory(user1MessagesDirectory);
            }

            if (!Directory.Exists(user2MessagesDirectory))
            {
                Directory.CreateDirectory(user2MessagesDirectory);
            }

            messages.AddRange(GetUserToUserMessages(user1MessagesDirectory, user1Id, user2Id));
            messages.AddRange(GetUserToUserMessages(user2MessagesDirectory, user2Id, user1Id));

            messages.Sort(new Comparison<Message>(MessagesDateComparer));

            return messages;
        }

        public void WriteMessage(Message message)
        {
            CheckUserIds(message.SenderId, message.ReceiverId);

            var messageDirectory = Path.Combine(EnigmaSettings.MainDirectory, message.SenderId.ToString(), message.ReceiverId.ToString());

            if (!Directory.Exists(messageDirectory))
            {
                Directory.CreateDirectory(messageDirectory);
            }

            var messagePath = Path.Combine(messageDirectory, message.Date.Ticks.ToString() + $".{EnigmaSettings.MessageExtension}");

            _messageWriter.Write(messagePath, message);
        }

        private void CheckUserIds(long user1Id, long user2Id)
        {
            if (!_userListReader.Read(EnigmaSettings.UserListPath).ContainsKey(user1Id))
            {
                throw new ArgumentException("There is no user with this id", nameof(user1Id));
            }

            if (!_userListReader.Read(EnigmaSettings.UserListPath).ContainsKey(user2Id))
            {
                throw new ArgumentException("There is no user with this id", nameof(user2Id));
            }
        }

        private IEnumerable<Message> GetUserToUserMessages(string directory, long senderId, long receiverId)
        {
            var messages = new List<Message>();

            var messagePaths = Directory.GetFiles(directory, $"*.{EnigmaSettings.MessageExtension}");

            foreach (var messagePath in messagePaths)
            {
                var message = _messageReader.Read(messagePath);

                if ((message.SenderId == senderId) && (message.ReceiverId == receiverId))
                {
                    messages.Add(message);
                }
            }

            return messages;
        }

        private int MessagesDateComparer(Message message1, Message message2)
        {
            if (message1.Date > message2.Date)
            {
                return 1;
            }
            else if(message1.Date < message2.Date)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
