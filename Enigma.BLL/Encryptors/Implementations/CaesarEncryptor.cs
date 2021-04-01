using Enigma.BLL.Encryptors.Interfaces;
using System;
using System.Text;

namespace Enigma.BLL.Encryptors.Implementations
{
    public class CaesarEncryptor : IEncryptor
    {
        private readonly int _key;

        public CaesarEncryptor(int key)
        {
            _key = key;
        }

        public string Decrypt(string text)
        {
            return ModifyMessage(text, (symbol) => (char)(symbol - _key));
        }

        public string Encrypt(string text)
        {
            return ModifyMessage(text, (symbol) => (char)(symbol + _key));
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
