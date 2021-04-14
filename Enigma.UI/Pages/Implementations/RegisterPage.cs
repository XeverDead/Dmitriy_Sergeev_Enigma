using Enigma.BLL.Services;
using Enigma.Common.Models;
using Enigma.UI.Pages.Interfaces;
using System;

namespace Enigma.UI.Pages.Implementations
{
    public class RegisterPage : IPage
    {
        private readonly UserService userService;

        public RegisterPage(UserService userService)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public void Show()
        {
            var userData = new UserCredentials();

            do
            {
                Console.WriteLine("Enter login");

                userData.Login = Console.ReadLine();

                Console.Clear();
                Console.WriteLine("Enter password");

                userData.Password = Console.ReadLine();
            }
            while (!userService.TryAddUser(userData));
        }
    }
}
