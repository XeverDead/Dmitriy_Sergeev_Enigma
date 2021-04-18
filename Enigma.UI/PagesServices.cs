using Enigma.BLL.Services;
using Enigma.UI.Pages.Implementations;
using System;
using System.Collections.Generic;

namespace Enigma.UI
{
    public class PagesServices
    {
        public Type[] GetPageServices(Type pageType)
        {
            if (pagesServices.TryGetValue(pageType, out Type[] serviceTypes))
            {
                return serviceTypes;
            }
            else
            {
                return new Type[0];
            }
        }

        private readonly Dictionary<Type, Type[]> pagesServices = new Dictionary<Type, Type[]>
        {
            [typeof(DialogPage)] = new Type[] { typeof(DialogService) },
            [typeof(DialogsPage)] = new Type[] { typeof(UserService) },
            [typeof(LoginPage)] = new Type[] { typeof(UserService) },
            [typeof(RegisterPage)] = new Type[] { typeof(UserService) },
            [typeof(KeyCreationPage)] = new Type[] { typeof(DialogService) },
            [typeof(UserSearchPage)] = new Type[] { typeof(UserService) }
        };
    }
}
