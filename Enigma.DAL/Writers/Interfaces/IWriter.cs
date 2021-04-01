namespace Enigma.DAL.Writers.Interfaces
{
    public interface IWriter<T>
    {
        void Write(string path, T data);
    }
}
