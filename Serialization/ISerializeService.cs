namespace Serialization;

public interface ISerializeService
{
    byte[] Serialize<T>(T obj) where T : class;

    T Deserialize<T>(byte[] data) where T : class;
}
