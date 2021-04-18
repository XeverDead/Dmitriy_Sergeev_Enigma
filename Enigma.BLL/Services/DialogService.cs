using Enigma.BLL.Encryptors.Interfaces;
using Enigma.Common.Models;
using Enigma.Common.Settings;
using Enigma.DAL.Readers.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Enigma.BLL.Services
{
    public class DialogService
    {
        private readonly IReader<Dictionary<long, string>> userListReader;

        public MessageService MessageService { get; }

        public DialogService(MessageService messageService, IReader<Dictionary<long, string>> userListReader)
        {
            MessageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
            this.userListReader = userListReader ?? throw new ArgumentNullException(nameof(userListReader));
        }

        public IEnumerable<Message> GetDialogMessages(long user1Id, long user2Id)
        {
            if (!CheckUserId(user1Id))
            {
                throw new ArgumentException("There no user with this id", nameof(user1Id));
            }

            if (!CheckUserId(user2Id))
            {
                throw new ArgumentException("There no user with this id", nameof(user2Id));
            }

            var user1MessagesDirectory = Path.Combine(EnigmaSettings.MainDirectory, user1Id.ToString(), user2Id.ToString());
            var user2MessagesDirectory = Path.Combine(EnigmaSettings.MainDirectory, user2Id.ToString(), user1Id.ToString());

            var messages = new List<Message>();

            Directory.CreateDirectory(user1MessagesDirectory);
            Directory.CreateDirectory(user2MessagesDirectory);

            messages.AddRange(GetUserToUserMessages(user1MessagesDirectory, user1Id, user2Id));
            messages.AddRange(GetUserToUserMessages(user2MessagesDirectory, user2Id, user1Id));

            messages.Sort(new Comparison<Message>(MessagesDateComparer));

            return messages;
        }

        public void StartDialog(long user1Id, long user2Id, int key)
        {
            if (!CheckUserId(user1Id))
            {
                throw new ArgumentException("There no user with this id", nameof(user1Id));
            }

            if (!CheckUserId(user2Id))
            {
                throw new ArgumentException("There no user with this id", nameof(user2Id));
            }

            var user1MessagesDirectory = Path.Combine(EnigmaSettings.MainDirectory, user1Id.ToString(), user2Id.ToString());
            var user2MessagesDirectory = Path.Combine(EnigmaSettings.MainDirectory, user2Id.ToString(), user1Id.ToString());

            Directory.CreateDirectory(user1MessagesDirectory);
            Directory.CreateDirectory(user2MessagesDirectory);

            // Save a key somehow
        }

        private bool CheckUserId(long userId)
        {
            return userListReader.Read(EnigmaSettings.UserListPath).ContainsKey(userId);
        }

        private IEnumerable<Message> GetUserToUserMessages(string directory, long senderId, long receiverId)
        {
            var messages = new List<Message>();

            var messagePaths = Directory.GetFiles(directory, $"*.{EnigmaSettings.MessageExtension}");

            foreach (var messagePath in messagePaths)
            {
                var message = MessageService.ReadMessage(messagePath);

                if ((message.SenderId == senderId) && (message.ReceiverId == receiverId)) 
                {
                    messages.Add(message);
                }
            }

            return messages;
        }

        private int MessagesDateComparer(Message message1, Message message2)
        {
            var result = 0;

            if (message1.Date > message2.Date)
            {
                result = 1;
            }
            else if (message1.Date < message2.Date)
            {
                result = -1;
            }

            return result;
        }
    }
}
