using Enigma.Common.Models;
using Enigma.Common.Settings;
using Enigma.DAL.Readers.Interfaces;
using Enigma.DAL.Writers.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace Enigma.BLL.Services
{
    public class DialogService
    {
        private readonly IReader<Dictionary<int, string>> userListReader;

        private readonly IReader<int> keyReader;
        private readonly IWriter<int> keyWriter;

        private readonly FileSystemWatcher systemWatcher;

        public int Key { get; private set; }

        public MessageService MessageService { get; }

        public event Action<Message> NewInterlocutorMessageWritten;

        public DialogService(MessageService messageService, 
            IReader<Dictionary<int, string>> userListReader,
            IReader<int> keyReader,
            IWriter<int> keyWriter,
            FileSystemWatcher systemWatcher)
        {
            MessageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
            this.userListReader = userListReader ?? throw new ArgumentNullException(nameof(userListReader));
            this.keyReader = keyReader ?? throw new ArgumentNullException(nameof(keyReader));
            this.keyWriter = keyWriter ?? throw new ArgumentNullException(nameof(keyWriter));
            this.systemWatcher = systemWatcher ?? throw new ArgumentNullException(nameof(systemWatcher));
        }

        public IEnumerable<Message> GetDialogMessages(int userId, int interlocutorId)
        {
            if (!CheckUserId(userId))
            {
                throw new ArgumentException("There no user with this id", nameof(userId));
            }

            if (!CheckUserId(interlocutorId))
            {
                throw new ArgumentException("There no user with this id", nameof(interlocutorId));
            }

            var userMessagesDirectoryPath = Path.Combine(EnigmaSettings.MainDirectory, userId.ToString(), interlocutorId.ToString());
            var interlocutorMessagesDirectoryPath = Path.Combine(EnigmaSettings.MainDirectory, interlocutorId.ToString(), userId.ToString());

            var messages = new List<Message>();

            var userKeyPath = Path.Combine(userMessagesDirectoryPath, EnigmaSettings.KeyFileName);
            var interlocutorKeyPath = Path.Combine(interlocutorMessagesDirectoryPath, EnigmaSettings.KeyFileName);

            var userKey = keyReader.Read(userKeyPath);
            var interlocutorKey = keyReader.Read(interlocutorKeyPath);

            if (userKey != interlocutorKey)
            {
                throw new ApplicationException("Message keys do not match");
            }
            else
            {
                Key = userKey;
            }

            messages.AddRange(GetUserToUserMessages(userMessagesDirectoryPath, userId, interlocutorId));

            if (userId != interlocutorId)
            {
                messages.AddRange(GetUserToUserMessages(interlocutorMessagesDirectoryPath, interlocutorId, userId));

                SetSystemWatcher(userId, interlocutorId);
            }

            messages.Sort(new Comparison<Message>(MessagesDateComparer));

            return messages;
        }

        public void StartDialog(int userId, int interlocutorId, int key)
        {
            if (!CheckUserId(userId))
            {
                throw new ArgumentException("There no user with this id", nameof(userId));
            }

            if (!CheckUserId(interlocutorId))
            {
                throw new ArgumentException("There no user with this id", nameof(interlocutorId));
            }

            var userMessagesDirectory = Path.Combine(EnigmaSettings.MainDirectory, userId.ToString(), interlocutorId.ToString());
            var interlocutorMessagesDirectory = Path.Combine(EnigmaSettings.MainDirectory, interlocutorId.ToString(), userId.ToString());

            Directory.CreateDirectory(userMessagesDirectory);
            Directory.CreateDirectory(interlocutorMessagesDirectory);

            keyWriter.Write(Path.Combine(userMessagesDirectory, EnigmaSettings.KeyFileName), key);
            keyWriter.Write(Path.Combine(interlocutorMessagesDirectory, EnigmaSettings.KeyFileName), key);

            Key = key;

            var message = new Message
            {
                SenderId = userId,
                ReceiverId = interlocutorId,
                Text = key.ToString(),
                Date = DateTime.Now
            };

            MessageService.SendMessage(message, key);
        }

        private bool CheckUserId(int userId)
        {
            return userListReader.Read(EnigmaSettings.UserListPath).ContainsKey(userId);
        }

        private IEnumerable<Message> GetUserToUserMessages(string directory, long senderId, long receiverId)
        {
            var messages = new List<Message>();

            var messagePaths = Directory.GetFiles(directory, $"*.{EnigmaSettings.MessageExtension}");

            foreach (var messagePath in messagePaths)
            {
                var message = MessageService.ReadMessage(messagePath, Key);

                if ((message.SenderId == senderId) && (message.ReceiverId == receiverId)) 
                {
                    messages.Add(message);
                }
            }

            return messages;
        }

        private void OnMessageCreated(object sender, FileSystemEventArgs e)
        {
            var isMessageRead = false;

            Message message = null;

            while (!isMessageRead)
            {
                try
                {
                    message = MessageService.ReadMessage(e.FullPath, Key);
                }
                catch(IOException)
                {
                    continue;
                }

                isMessageRead = true;
            }

            NewInterlocutorMessageWritten?.Invoke(message);
        }

        private void SetSystemWatcher(int userId, int interlocutorId)
        {
            systemWatcher.Path = Path.Combine(EnigmaSettings.MainDirectory, interlocutorId.ToString(), userId.ToString());
            systemWatcher.Filter = $"*.{EnigmaSettings.MessageExtension}";
            systemWatcher.NotifyFilter = NotifyFilters.FileName;
            systemWatcher.EnableRaisingEvents = true;

            systemWatcher.Created += OnMessageCreated;
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
