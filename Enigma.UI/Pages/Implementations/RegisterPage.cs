using Enigma.BLL.Services;
using Enigma.Common.Models;
using Enigma.UI.Pages.Interfaces;
using System;

namespace Enigma.UI.Pages.Implementations
{
    public class RegisterPage : IPage
    {
        private const string ExitCode = "#e";
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
            var userData = new UserCredentials();

            var userId = -1;

            var hasUserRegistered = false;

            do
            {
                Console.Clear();

                Console.WriteLine("Enter login");

                userData.Login = Console.ReadLine();

                if (userData.Login == ExitCode)
                {
                    break;
                }
                else if (userData.Password == LoginCode)
                {
                    NextPageType = typeof(LoginPage);
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
                else if (userData.Password == LoginCode)
                {
                    NextPageType = typeof(LoginPage);
                    NextPageArgs = new object[0];

                    break;
                }

                hasUserRegistered = userService.TryAddUser(userData, out userId);
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
