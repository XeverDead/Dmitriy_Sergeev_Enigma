using Enigma.BLL.Services;
using Enigma.Common.Models;
using Enigma.UI.Pages.Interfaces;
using System;

namespace Enigma.UI.Pages.Implementations
{
    public class KeyCreationPage : IPage
    {
        private const string ToDialogCreationCode = "#b";

        private readonly DialogService dialogService;

        private readonly User user;
        private readonly User interlocutor;

        public KeyCreationPage(DialogService dialogService, User user, User interlocutor)
        {
            this.dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            this.user = user ?? throw new ArgumentNullException(nameof(user));
            this.interlocutor = interlocutor ?? throw new ArgumentNullException(nameof(interlocutor));
        }

        public Type NextPageType { get; private set; }

        public object[] NextPageArgs { get; private set; }

        public void Show()
        {
            Console.Title = $"Enigma - {user.Login} is creating key to start messaging with {interlocutor.Login}";

            var isKeyCreated = false;
            var key = -1;

            var isErrorOccurred = false;
            var errorMessage = string.Empty;

            while (!isKeyCreated)
            {
                if (isErrorOccurred)
                {
                    Console.Clear();
                    Console.WriteLine($"{errorMessage}. Press any key to continue");
                    Console.ReadKey();
                }

                Console.Clear();

                Console.WriteLine("Creating dialog with " + interlocutor.Login);
                Console.WriteLine("Enter key (or enter #b to choose other user)");

                var enteredData = Console.ReadLine();

                if (enteredData == ToDialogCreationCode)
                {
                    NextPageType = typeof(UserSearchPage);
                    NextPageArgs = new object[1] { user };

                    break;
                }
                else
                {
                    isKeyCreated = int.TryParse(enteredData, out key);

                    if (!isKeyCreated)
                    {
                        errorMessage = "Key must be integer number";
                    }

                    isErrorOccurred = !isKeyCreated;
                }
            }

            if (isKeyCreated)
            {
                dialogService.StartDialog(user.Id, interlocutor.Id, key);

                NextPageType = typeof(DialogPage);
                NextPageArgs = new object[2] { user, interlocutor };
            }
        }
    }
}
