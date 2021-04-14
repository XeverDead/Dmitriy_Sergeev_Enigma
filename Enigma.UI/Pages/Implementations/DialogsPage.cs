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
        private readonly UserService userService;

        private readonly User user;

        public DialogsPage(UserService userService, User user)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.user = user ?? throw new ArgumentNullException(nameof(user));
        }

        public void Show()
        {
            var interlocutors = userService.GetInterlocutors(user.Id);

            var userIndex = 1;

            var chosenUserIndex = 1;

            do
            {
                Console.Clear();

                Console.WriteLine("Your interlocutors:");

                foreach (var user in interlocutors)
                {
                    Console.WriteLine($"{userIndex}. {user.Login}");
                }

                Console.WriteLine("\nEnter user number to enter dialog window.");
            }
            while (!(int.TryParse(Console.ReadLine(), out chosenUserIndex) && (chosenUserIndex > 0) && (chosenUserIndex <= userIndex)));
        }
    }
}
