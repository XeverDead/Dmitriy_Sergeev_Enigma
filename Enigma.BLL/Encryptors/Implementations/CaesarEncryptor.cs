using Enigma.BLL.Encryptors.Interfaces;
using System;
using System.Text;

namespace Enigma.BLL.Encryptors.Implementations
{
    public class CaesarEncryptor : IEncryptor
    {
        public string Decrypt(string text, int key)
        {
            return ModifyMessage(text, (symbol) => (char)(symbol - key));
        }

        public string Encrypt(string text, int key)
        {
            return ModifyMessage(text, (symbol) => (char)(symbol + key));
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
