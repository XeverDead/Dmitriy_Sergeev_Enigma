using Enigma.Common.Models;
using Enigma.Common.Settings;
using Enigma.DAL.Readers.Interfaces;
using Enigma.DAL.Writers.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Enigma.BLL.Services
{
    public class UserService
    {
        private readonly IReader<UserCredentials> userDataReader;
        private readonly IWriter<UserCredentials> userDataWriter;

        private readonly IReader<Dictionary<int, string>> userListReader;
        private readonly IWriter<KeyValuePair<int, string>> userListWriter;

        public UserService(IReader<UserCredentials> userDataReader, 
            IWriter<UserCredentials> userDataWriter, 
            IReader<Dictionary<int, string>> userListReader, 
            IWriter<KeyValuePair<int, string>> userListWriter)
        {
            this.userDataReader = userDataReader ?? throw new ArgumentNullException(nameof(userDataReader));
            this.userDataWriter = userDataWriter ?? throw new ArgumentNullException(nameof(userDataWriter));
            this.userListReader = userListReader ?? throw new ArgumentNullException(nameof(userListReader));
            this.userListWriter = userListWriter ?? throw new ArgumentNullException(nameof(userListWriter));
        }

        public bool TryAddUser(UserCredentials userData, out int userId)
        {
            userId = -1;

            var isLoginTaken = userListReader.Read(EnigmaSettings.UserListPath).ContainsValue(userData.Login);

            if (!isLoginTaken)
            {
                userId = GenerateId(userData);

                var idLoginPair = new KeyValuePair<int, string>(userId, userData.Login);

                userListWriter.Write(EnigmaSettings.UserListPath, idLoginPair);

                var userDirectoryPath = Path.Combine(EnigmaSettings.MainDirectory, userId.ToString());

                Directory.CreateDirectory(userDirectoryPath);

                var userDataPath = Path.Combine(userDirectoryPath, EnigmaSettings.UserDataFileName);

                userDataWriter.Write(userDataPath, userData);
            }

            return !isLoginTaken;
        }

        public bool CheckUserData(UserCredentials enteredData, out int userId)
        {
            var isDataValid = false;

            var userList = userListReader.Read(EnigmaSettings.UserListPath);

            userId = -1;

            if (userList.ContainsValue(enteredData.Login))
            {
                userId = userList.First((data) => data.Value == enteredData.Login).Key;
                var userDataPath = Path.Combine(EnigmaSettings.MainDirectory, userId.ToString(), EnigmaSettings.UserDataFileName);

                isDataValid = File.Exists(userDataPath);

                if (isDataValid)
                {
                    var userData = userDataReader.Read(userDataPath);

                    isDataValid = (userData.Login == enteredData.Login) && (userData.Password == enteredData.Password);
                }
            }

            return isDataValid;
        }

        public List<User> GetInterlocutors(int userId)
        {
            var interlocutorsList = new List<User>();

            var userDirectoryPath = Path.Combine(EnigmaSettings.MainDirectory, userId.ToString());

            var userList = userListReader.Read(EnigmaSettings.UserListPath);

            foreach (var directoryPath in Directory.GetDirectories(userDirectoryPath))
            {
                var directoryName = directoryPath[(directoryPath.LastIndexOf("\\") + 1)..];

                if (int.TryParse(directoryName, out int interlocutorId) && userList.ContainsKey(interlocutorId))
                {
                    var interlocutor = new User 
                    { 
                        Id = interlocutorId, 
                        Login = userList[interlocutorId] 
                    };

                    interlocutorsList.Add(interlocutor);
                }
            }

            return interlocutorsList;
        }

        public List<User> GetPossibleInterlocutorsByLoginPattern(int userId, string loginPattern)
        {
            var idLoginPairs = userListReader.Read(EnigmaSettings.UserListPath).Where((pair) => pair.Value.Contains(loginPattern, StringComparison.OrdinalIgnoreCase));

            var interlocutors = GetInterlocutors(userId);

            var users = new List<User>();

            foreach (var idLoginPair in idLoginPairs)
            {
                users.Add(new User { Id = idLoginPair.Key, Login = idLoginPair.Value });
            }

            for (var userIndex = 0; userIndex < users.Count; userIndex++)
            {
                for (var interlocutorIndex = 0; interlocutorIndex < interlocutors.Count; interlocutorIndex++)
                {
                    if ((users[userIndex].Id == interlocutors[interlocutorIndex].Id) && (users[userIndex].Login == interlocutors[interlocutorIndex].Login))
                    {
                        users.RemoveAt(userIndex);
                        userIndex--;
                    }
                }
            }

            return users;
        }

        private int GenerateId(UserCredentials userData)
        {
            var id = 1;

            var users = userListReader.Read(EnigmaSettings.UserListPath);

            if (users.Count > 0)
            {
                id = users.Keys.Max() + 1;
            }

            return id;
        }
    }
}
