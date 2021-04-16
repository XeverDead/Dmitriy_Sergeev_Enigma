using Enigma.BLL.Services;
using Enigma.Common.Models;
using Enigma.UI.Pages.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Enigma.UI.Pages.Implementations
{
    public class DialogPage : IPage
    {
        private const string ExitCode = "#e";

        private readonly DialogService dialogService;

        private readonly User sender;
        private readonly User receiver;

        public DialogPage(DialogService dialogService, User sender, User receiver)
        {
            this.dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));

            this.sender = sender ?? throw new ArgumentNullException(nameof(sender));
            this.receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
        }

        public Type NextPageType { get; private set; }

        public object[] NextPageArgs { get; private set; }

        public void Show()
        {
            var messages = dialogService.GetDialogMessages(sender.Id, receiver.Id);

            foreach (var message in messages)
            {
                if (message.SenderId == sender.Id)
                {
                    Console.Write(sender.Login + ": ");
                }
                else if(message.SenderId == receiver.Id)
                {
                    Console.Write(receiver.Login + ": ");
                }

                Console.WriteLine(message.Text);
            }

            var exitCodeWritten = false;

            var messageToSend = new Message
            {
                SenderId = sender.Id,
                ReceiverId = receiver.Id,
            };

            while (!exitCodeWritten)
            {
                Console.Write(sender.Login + ": ");

                var enteredText = Console.ReadLine();

                if (enteredText == ExitCode)
                {
                    exitCodeWritten = true;
                }
                else
                {
                    messageToSend.Text = enteredText;
                    messageToSend.Date = DateTime.Now;

                    dialogService.MessageService.SendMessage(messageToSend);
                }
            }

            NextPageType = typeof(DialogsPage);
            NextPageArgs = new object[] { sender };
        }
    }
}
