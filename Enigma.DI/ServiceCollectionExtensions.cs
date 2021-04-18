using Enigma.BLL.Encryptors.Implementations;
using Enigma.BLL.Encryptors.Interfaces;
using Enigma.BLL.Services;
using Enigma.Common.Models;
using Enigma.DAL.Readers.Implementations.FileReaders;
using Enigma.DAL.Readers.Interfaces;
using Enigma.DAL.Writers.Implementations.FileWriters;
using Enigma.DAL.Writers.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Enigma.DI
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEnigmaServices(this ServiceCollection services)
        {
            services.AddSingleton<IWriter<Message>, MessageFileWriter>();
            services.AddSingleton<IWriter<UserCredentials>, UserDataFileWriter>();
            services.AddSingleton<IWriter<KeyValuePair<int, string>>, UserListFileWriter>();

            services.AddSingleton<IReader<Message>, MessageFileReader>();
            services.AddSingleton<IReader<UserCredentials>, UserDataFileReader>();
            services.AddSingleton<IReader<Dictionary<int, string>>, UserListFileReader>();

            services.AddSingleton<MessageService>();
            services.AddSingleton<DialogService>();
            services.AddSingleton<UserService>();

            services.AddSingleton<IEncryptor, CaesarEncryptor>();
        }
    }
}
