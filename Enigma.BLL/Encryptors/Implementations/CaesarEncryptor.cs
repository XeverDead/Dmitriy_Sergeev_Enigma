using Enigma.BLL.Encryptors.Interfaces;
using System;
using System.Text;

namespace Enigma.BLL.Encryptors.Implementations
{
    public class CaesarEncryptor : IEncryptor
    {
        public int Key { private get; set; }

        public string Decrypt(string text)
        {
            return ModifyMessage(text, (symbol) => (char)(symbol - Key));
        }

        public string Encrypt(string text)
        {
            return ModifyMessage(text, (symbol) => (char)(symbol + Key));
        }

        private string ModifyMessage(string text, Func<char, char> symbolModificationRule)
        {
            var resultBuilder = new StringBuilder();

            foreach (var symbol in text)
            {
                resultBuilder.Append(symbolModificationRule(symbol));
            }

            return resultBuilder.ToString();
        }
    }
}
