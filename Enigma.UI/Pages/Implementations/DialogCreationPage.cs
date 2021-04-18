using Enigma.Common.Models;
using Enigma.UI.Pages.Interfaces;
using System;
using System.Collections.Generic;

namespace Enigma.UI.Pages.Implementations
{
    public class DialogCreationPage : IPage
    {
        private const string ToSearchCode = "#b";

        private readonly User user;
        private readonly List<User> users;

        public Type NextPageType { get; private set; }

        public object[] NextPageArgs { get; private set; }

        public DialogCreationPage(User user, List<User> users)
        {
            this.user = user ?? throw new ArgumentNullException(nameof(user));
            this.users = users ?? throw new ArgumentNullException(nameof(users));
        }

        public void Show()
        {
            Console.Title = $"Enigma - {user.Login} is choosing a new friend";

            var isInputValid = false;

            var chosenUserIndex = 1;

            var isErrorOccurred = false;
            var errorMessage = string.Empty;

            while (!isInputValid)
            {
                if (isErrorOccurred)
                {
                    Console.Clear();
                    Console.WriteLine($"{errorMessage}. Press any key to continue\n");
                    Console.ReadKey();
                }

                Console.Clear();

                var userNum = 1;

                foreach (var user in users)
                {
                    Console.WriteLine($"{userNum++}. {user.Login}");
                }

                Console.WriteLine("Enter a number of a user, who you want to start dialog with (or enter #b to get back to search page)");

                var userInput = Console.ReadLine();

                if (userInput == ToSearchCode)
                {
                    NextPageType = typeof(UserSearchPage);
                    NextPageArgs = new object[1] { user };

                    break;
                }

                isInputValid = int.TryParse(userInput, out chosenUserIndex) && (chosenUserIndex > 0) && (chosenUserIndex < userNum);

                if (!isInputValid)
                {
                    errorMessage = "There is no user with this number";
                }

                isErrorOccurred = !isInputValid;
            }

            if (isInputValid)
            {
                NextPageType = typeof(KeyCreationPage);
                NextPageArgs = new object[2] { user, users[chosenUserIndex - 1] };
            }
        }
    }
}
