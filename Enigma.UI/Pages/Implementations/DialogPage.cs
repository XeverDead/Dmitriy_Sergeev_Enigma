using Enigma.BLL.Services;
using Enigma.Common.Models;
using Enigma.UI.Pages.Interfaces;
using System;

namespace Enigma.UI.Pages.Implementations
{
    public class DialogPage : IPage
    {
        private const string ToDialogsCode = "#b";

        private readonly DialogService dialogService;

        private readonly User user;
        private readonly User interlocutor;

        public DialogPage(DialogService dialogService, User user, User interlocutor)
        {
            this.dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));

            this.user = user ?? throw new ArgumentNullException(nameof(user));
            this.interlocutor = interlocutor ?? throw new ArgumentNullException(nameof(interlocutor));

            dialogService.NewInterlocutorMessageWritten += WriteInterlocutorMessage;
        }

        public Type NextPageType { get; private set; }

        public object[] NextPageArgs { get; private set; }

        public void Show()
        {
            Console.Title = $"Enigma - Dialog between {user.Login} and {interlocutor.Login}";

            Console.Clear();
            Console.WriteLine("To get back to dialogs page enter #b. Press any key to continue");
            Console.ReadKey();

            Console.Clear();

            var messages = dialogService.GetDialogMessages(user.Id, interlocutor.Id);

            foreach (var message in messages)
            {
                if (message.SenderId == user.Id)
                {
                    Console.Write(user.Login + ": ");
                }
                else if(message.SenderId == interlocutor.Id)
                {
                    Console.Write(interlocutor.Login + ": ");
                }

                Console.WriteLine(message.Text);
            }

            var toDialogsCodeWritten = false;

            var messageToSend = new Message
            {
                SenderId = user.Id,
                ReceiverId = interlocutor.Id,
            };

            while (!toDialogsCodeWritten)
            {
               Console.Write(user.Login + ": ");

                var enteredText = Console.ReadLine();

                if (enteredText == ToDialogsCode)
                {
                    toDialogsCodeWritten = true;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(enteredText))
                    {
                        Console.CursorTop--;
                        ClearLine();
                        continue;
                    }

                    messageToSend.Text = enteredText;
                    messageToSend.Date = DateTime.Now;

                    dialogService.MessageService.SendMessage(messageToSend, dialogService.Key);
                }
            }

            NextPageType = typeof(DialogsPage);
            NextPageArgs = new object[] { user };
        }

        private void WriteInterlocutorMessage(Message obj)
        {
            ClearLine();

            Console.WriteLine($"{interlocutor.Login}: {obj.Text}");
            Console.Write($"{user.Login}: ");
        }

        private void ClearLine()
        {
            Console.Write(new string('\b', Console.CursorLeft));
        }
    }
}
