using System;

namespace Enigma.UI.Pages.Interfaces
{
    public interface IPage
    {
        Type NextPageType { get; }

        object[] NextPageArgs { get; }

        void Show();
    }
}
