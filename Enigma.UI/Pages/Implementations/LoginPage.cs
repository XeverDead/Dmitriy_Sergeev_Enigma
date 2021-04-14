using System;
using Enigma.BLL.Services;
using Enigma.Common.Models;
using Enigma.UI.Pages.Interfaces;

namespace Enigma.UI.Pages.Implementations
{
    public class LoginPage : IPage
    {
        private readonly UserService userService;

        public LoginPage(UserService userService)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public void Show()
        {
            var userData = new UserCredentials();

            do
            {
                Console.Clear();
                Console.WriteLine("Enter login");

                userData.Login = Console.ReadLine();

                Console.Clear();
                Console.WriteLine("Enter password");

                userData.Password = Console.ReadLine();
            }
            while (!userService.CheckUserData(userData));
        }
    }
}
