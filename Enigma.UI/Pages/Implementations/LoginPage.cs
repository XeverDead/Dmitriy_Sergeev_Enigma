using System;
using Enigma.BLL.Services;
using Enigma.Common.Models;
using Enigma.UI.Pages.Interfaces;

namespace Enigma.UI.Pages.Implementations
{
    public class LoginPage : IPage
    {
        private const string RegisterCode = "#r";

        private readonly UserService userService;

        public LoginPage(UserService userService)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public Type NextPageType { get; private set; }

        public object[] NextPageArgs { get; private set; }

        public void Show()
        {
            Console.Title = $"Enigma - Log in page";

            var userData = new UserCredentials();

            var userId = -1;

            var hasUserLoggedIn = false;

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

                Console.WriteLine("Enter login (or enter #r to register)");

                userData.Login = Console.ReadLine();

                if (userData.Login == RegisterCode)
                {
                    NextPageType = typeof(RegisterPage);
                    NextPageArgs = new object[0];

                    break;
                }

                Console.Clear();
                Console.WriteLine("Enter password (or enter #r to register)");

                userData.Password = Console.ReadLine();

                if (userData.Login == RegisterCode)
                {
                    NextPageType = typeof(RegisterPage);
                    NextPageArgs = new object[0];

                    break;
                }

                hasUserLoggedIn = userService.CheckUserData(userData, out userId);

                if (!hasUserLoggedIn)
                {
                    errorMessage = "Wrong login or password";
                }

                isErrorOccurred = !hasUserLoggedIn;
            }
            while (!hasUserLoggedIn);

            if (hasUserLoggedIn)
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
