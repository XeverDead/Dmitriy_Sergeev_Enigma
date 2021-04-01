using System;

namespace Enigma.Common.Models
{
    public class Message
    {
        public long Id { get; set; }

        public string Text { get; set; }

        public long SenderId { get; set; }

        public long ReceiverId { get; set; }

        public DateTime Date { get; set; }
    }
}
