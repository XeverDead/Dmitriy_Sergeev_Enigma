using Enigma.BLL.Services;
using Enigma.Common.Models;
using Enigma.UI.Pages.Interfaces;
using System;

namespace Enigma.UI.Pages.Implementations
{
    public class DialogsPage : IPage
    {
        private const string LogOutCode = "#e";
        private const string NewDialogCode = "#n";

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
            Console.Title = $"Enigma - {user.Login} looking at friend list";

            var interlocutors = userService.GetInterlocutors(user.Id);

            var chosenUserIndex = 1;

            var isUserChosen = false;

            var isErrorOccurred = false;
            var errorMessage = string.Empty;

            do
            {
                var userIndex = 1;

                var userInput = string.Empty;

                if (isErrorOccurred)
                {
                    Console.Clear();
                    Console.WriteLine($"{errorMessage}. Press any key to continue");
                    Console.ReadKey();
                }

                Console.Clear();

                Console.WriteLine("Your interlocutors:");

                foreach (var user in interlocutors)
                {
                    Console.WriteLine($"{userIndex++}. {user.Login}");
                }

                Console.WriteLine("\nEnter user number to enter dialog window (or enter #n to start new dialog, #e to log out)");

                userInput = Console.ReadLine();

                if (userInput == LogOutCode)
                {
                    NextPageType = typeof(LoginPage);
                    NextPageArgs = new object[0];
                    break;
                }
                else if (userInput == NewDialogCode)
                {
                    NextPageType = typeof(UserSearchPage);
                    NextPageArgs = new object[1] { user };

                    break;
                }

                isUserChosen = int.TryParse(userInput, out chosenUserIndex) && (chosenUserIndex > 0) && (chosenUserIndex < userIndex);

                if (!isUserChosen)
                {
                    errorMessage = "There is no user with this number";
                }

                isErrorOccurred = !isUserChosen;
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
