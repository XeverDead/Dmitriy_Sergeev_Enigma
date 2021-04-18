using Enigma.BLL.Encryptors.Implementations;
using Enigma.BLL.Encryptors.Interfaces;
using Enigma.BLL.Services;
using Enigma.Common.Models;
using Enigma.DAL.Readers.Implementations;
using Enigma.DAL.Readers.Interfaces;
using Enigma.DAL.Writers.Implementations;
using Enigma.DAL.Writers.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO;

namespace Enigma.DI
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEnigmaServices(this ServiceCollection services)
        {
            services.AddSingleton<IWriter<Message>, MessageWriter>();
            services.AddSingleton<IWriter<UserCredentials>, UserDataWriter>();
            services.AddSingleton<IWriter<KeyValuePair<int, string>>, UserListWriter>();
            services.AddSingleton<IWriter<int>, KeyWriter>();

            services.AddSingleton<IReader<Message>, MessageReader>();
            services.AddSingleton<IReader<UserCredentials>, UserDataReader>();
            services.AddSingleton<IReader<Dictionary<int, string>>, UserListReader>();
            services.AddSingleton<IReader<int>, KeyReader>();

            services.AddSingleton<MessageService>();
            services.AddSingleton<DialogService>();
            services.AddSingleton<UserService>();

            services.AddSingleton<FileSystemWatcher>();

            services.AddSingleton<IEncryptor, CaesarEncryptor>();
        }
    }
}
