using Enigma.BLL.Services;
using Enigma.Common.Models;
using Enigma.UI.Pages.Interfaces;
using System;

namespace Enigma.UI.Pages.Implementations
{
    public class UserSearchPage : IPage
    {
        private const string ToDialogsCode = "#b";

        private readonly UserService userService;

        private readonly User user;

        public Type NextPageType { get; private set; }

        public object[] NextPageArgs { get; private set; }

        public UserSearchPage(UserService userService, User user)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.user = user ?? throw new ArgumentNullException(nameof(user));
        }

        public void Show()
        {
            Console.Title = $"Enigma - {user.Login} trying to find a new friend";

            Console.Clear();

            Console.WriteLine("Enter user login or part of it (or enter #b to get to dialogs page)");

            var loginPattern = Console.ReadLine();

            if (loginPattern == ToDialogsCode)
            {
                NextPageType = typeof(DialogsPage);
                NextPageArgs = new object[1] { user };

                return;
            }

            var users = userService.GetPossibleInterlocutorsByLoginPattern(user.Id, loginPattern);

            NextPageType = typeof(DialogCreationPage);
            NextPageArgs = new object[2] { user, users };
        }
    }
}
