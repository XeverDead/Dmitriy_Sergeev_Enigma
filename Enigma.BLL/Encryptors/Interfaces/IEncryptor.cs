namespace Enigma.BLL.Encryptors.Interfaces
{
    public interface IEncryptor
    {
        public string Encrypt(string initialText, int key);

        public string Decrypt(string initialText, int key);
    }
}
