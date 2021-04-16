using Enigma.BLL.Services;
using Enigma.Common.Models;
using Enigma.UI.Pages.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Enigma.UI.Pages.Implementations
{
    public class DialogsPage : IPage
    {
        private const string ExitCode = "#e";

        private readonly UserService userService;

        private readonly User user;

        public DialogsPage(UserService userService, User user)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.user = user ?? throw new ArgumentNullException(nameof(user));
        }

        public Type NextPageType { get; private set; }

        public object[] NextPageArgs { get; private set; }

        public void Show()
        {
            var interlocutors = userService.GetInterlocutors(user.Id);

            var userIndex = 1;

            var chosenUserIndex = 1;

            var chosenUserIndexStr = string.Empty;

            var isUserChosen = false;

            do
            {
                Console.Clear();

                Console.WriteLine("Your interlocutors:");

                foreach (var user in interlocutors)
                {
                    Console.WriteLine($"{userIndex}. {user.Login}");
                    userIndex++;
                }

                Console.WriteLine("\nEnter user number to enter dialog window.");

                chosenUserIndexStr = Console.ReadLine();

                if (chosenUserIndexStr == ExitCode)
                {
                    NextPageType = typeof(LoginPage);
                    NextPageArgs = new object[0];
                    break;
                }

                isUserChosen = int.TryParse(chosenUserIndexStr, out chosenUserIndex) && (chosenUserIndex > 0) && (chosenUserIndex < userIndex);
            }
            while (!isUserChosen);

            if (isUserChosen)
            {
                NextPageType = typeof(DialogPage);
                NextPageArgs = new object[] { user, interlocutors[chosenUserIndex - 1] };
            }
        }
    }
}
