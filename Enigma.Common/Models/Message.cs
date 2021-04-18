using System;

namespace Enigma.Common.Models
{
    public class Message : ICloneable
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        public DateTime Date { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
