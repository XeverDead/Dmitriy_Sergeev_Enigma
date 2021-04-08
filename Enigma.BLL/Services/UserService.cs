using Enigma.Common.Models;
using Enigma.Common.Settings;
using Enigma.DAL.Readers.Interfaces;
using Enigma.DAL.Writers.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Enigma.BLL.Services
{
    public class UserService
    {
        private readonly IReader<UserCredentials> _userdataReader;
        private readonly IWriter<UserCredentials> _userDataWriter;

        private readonly IReader<Dictionary<long, string>> _userListReader;
        private readonly IWriter<KeyValuePair<long, string>> _userListWriter;

        public UserService(IReader<UserCredentials> userdataReader, IWriter<UserCredentials> userDataWriter, IReader<Dictionary<long, string>> userListReader, IWriter<KeyValuePair<long, string>> userListWriter)
        {
            _userdataReader = userdataReader ?? throw new ArgumentNullException(nameof(userdataReader));
            _userDataWriter = userDataWriter ?? throw new ArgumentNullException(nameof(userDataWriter));
            _userListReader = userListReader ?? throw new ArgumentNullException(nameof(userListReader));
            _userListWriter = userListWriter ?? throw new ArgumentNullException(nameof(userListWriter));
        }

        public bool AddUser(UserCredentials userData)
        {
            var isLoginTaken = _userListReader.Read(EnigmaSettings.UserListPath).ContainsValue(userData.Login);

            if (!isLoginTaken)
            {
                var pair = new KeyValuePair<long, string>(userData.UserId, userData.Login);

                _userListWriter.Write(EnigmaSettings.UserListPath, pair);

                var userDirectoryPath = Path.Combine(EnigmaSettings.MainDirectory, userData.UserId.ToString());

                if (!Directory.Exists(userDirectoryPath))
                {
                    Directory.CreateDirectory(userDirectoryPath);
                }

                var userDataPath = Path.Combine(userDirectoryPath, EnigmaSettings.UserDataFileName);

                _userDataWriter.Write(userDataPath, userData);
            }

            return !isLoginTaken;
        }

        public bool CheckUserData(UserCredentials enteredData)
        {
            var userDataPath = Path.Combine(EnigmaSettings.MainDirectory, enteredData.UserId.ToString(), EnigmaSettings.UserDataFileName);

            var isDataValid = File.Exists(userDataPath);

            if(isDataValid)
            {
                var userData = _userdataReader.Read(userDataPath);

                if (!((userData.Login == enteredData.Login) && (userData.Password == enteredData.Password)))
                {
                    isDataValid = false;
                }
            }

            return isDataValid;
        }
    }
}
