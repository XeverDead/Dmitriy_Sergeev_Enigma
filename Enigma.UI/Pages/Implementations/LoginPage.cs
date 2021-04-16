using System;
using Enigma.BLL.Services;
using Enigma.Common.Models;
using Enigma.UI.Pages.Interfaces;

namespace Enigma.UI.Pages.Implementations
{
    public class LoginPage : IPage
    {
        private const string ExitCode = "#e";
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
            var userData = new UserCredentials();

            var userId = -1;

            var hasUserLoggedIn = false;

            do
            {
                Console.Clear();
                Console.WriteLine("Enter login");

                userData.Login = Console.ReadLine();

                if (userData.Login == ExitCode)
                {
                    break;
                }
                else if (userData.Login == RegisterCode)
                {
                    NextPageType = typeof(RegisterPage);
                    NextPageArgs = new object[0];

                    break;
                }

                Console.Clear();
                Console.WriteLine("Enter password");

                userData.Password = Console.ReadLine();

                if (userData.Password == ExitCode)
                {
                    break;
                }
                else if (userData.Login == RegisterCode)
                {
                    NextPageType = typeof(RegisterPage);
                    NextPageArgs = new object[0];

                    break;
                }

                hasUserLoggedIn = userService.CheckUserData(userData, out userId);
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
