using Enigma.BLL.Services;
using Enigma.UI.Pages.Implementations;
using Enigma.UI.Pages.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Enigma.UI
{
    public class PagesServices
    {
        private readonly IServiceProvider serviceProvider;

        public PagesServices(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

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

        private Dictionary<Type, Type[]> pagesServices = new Dictionary<Type, Type[]>
        {
            [typeof(DialogPage)] = new Type[] { typeof(DialogService) },
            [typeof(DialogsPage)] = new Type[] { typeof(UserService) },
            [typeof(LoginPage)] = new Type[] { typeof(UserService) },
            [typeof(RegisterPage)] = new Type[] { typeof(UserService) }
        };
    }
}
