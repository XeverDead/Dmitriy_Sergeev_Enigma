using Enigma.BLL.Services;
using Enigma.Common.Models;
using Enigma.UI.Pages.Interfaces;
using System;

namespace Enigma.UI.Pages.Implementations
{
    public class RegisterPage : IPage
    {
        private const string LoginCode = "#l";

        private readonly UserService userService;

        public RegisterPage(UserService userService)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public Type NextPageType { get; private set; }

        public object[] NextPageArgs { get; private set; }

        public void Show()
        {
            Console.Title = $"Enigma - Registration";

            var userData = new UserCredentials();

            var userId = -1;

            var hasUserRegistered = false;

            var isErrorOccurred = false;
            var errorMessage = string.Empty;

            do
            {
                if (isErrorOccurred)
                {
                    Console.Clear();
                    Console.WriteLine($"{errorMessage}. Press any key to continue\n");
                    Console.ReadKey();
                }

                Console.Clear();

                Console.WriteLine("Enter login (or enter #l to log in)");

                userData.Login = Console.ReadLine();

                if (userData.Login == LoginCode)
                {
                    NextPageType = typeof(LoginPage);
                    NextPageArgs = new object[0];

                    break;
                }

                Console.Clear();
                Console.WriteLine("Enter password (or enter #l to log in)");

                userData.Password = Console.ReadLine();

                if (userData.Password == LoginCode)
                {
                    NextPageType = typeof(LoginPage);
                    NextPageArgs = new object[0];

                    break;
                }

                hasUserRegistered = userService.TryAddUser(userData, out userId);

                if (!hasUserRegistered)
                {
                    errorMessage = "User with this login already exists";
                }

                isErrorOccurred = !hasUserRegistered;
            }
            while (!hasUserRegistered);

            if (hasUserRegistered)
            {
                var user = new User
                {
                    Id = userId,
                    Login = userData.Login
                };

                NextPageType = typeof(DialogsPage);
                NextPageArgs = new object[] { user };
            }
        }
    }
}
