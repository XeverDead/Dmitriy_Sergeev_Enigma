using Enigma.BLL.Services;
using Enigma.Common.Settings;
using Enigma.DI;
using Enigma.UI.Pages.Implementations;
using Enigma.UI.Pages.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Enigma.UI
{
    class Program
    {
        private static IPage currentPage;

        private static IServiceProvider serviceProvider;

        private static PagesServices pagesServices;

        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddEnigmaServices();
            serviceProvider = services.BuildServiceProvider();

            pagesServices = new PagesServices(serviceProvider);

            currentPage = new RegisterPage(serviceProvider.GetRequiredService<UserService>());

            Directory.CreateDirectory(EnigmaSettings.MainDirectory);

            if (!File.Exists(EnigmaSettings.UserListPath))
            {
                File.Create(EnigmaSettings.UserListPath);
            }

            StartApp();
        }

        static void StartApp()
        {
            var nextPageSet = false;

            do
            {
                currentPage.Show();

                if (!(currentPage.NextPageType is null))
                {
                    var args = new List<object>();

                    var pageServiceTypes = pagesServices.GetPageServices(currentPage.NextPageType);

                    foreach (var pageServiceType in pageServiceTypes)
                    {
                        args.Add(serviceProvider.GetRequiredService(pageServiceType));
                    }

                    args.AddRange(currentPage.NextPageArgs);

                    currentPage = Activator.CreateInstance(currentPage.NextPageType, args.ToArray()) as IPage;

                    nextPageSet = true;
                }
                else
                {
                    nextPageSet = false;
                }
            }
            while (nextPageSet);
        }
    }
}
