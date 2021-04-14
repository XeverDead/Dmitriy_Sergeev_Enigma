namespace Enigma.BLL.Encryptors.Interfaces
{
    public interface IEncryptor
    {
        public string Encrypt(string initialText);

        public string Decrypt(string initialText);
    }
}
