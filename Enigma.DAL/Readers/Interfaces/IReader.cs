namespace Enigma.DAL.Readers.Interfaces
{
    public interface IReader<T>
    {
        T Read(string path);
    }
}
