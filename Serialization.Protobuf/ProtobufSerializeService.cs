namespace Serialization.Protobuf;

public class ProtobufSerializeService : ISerializeService
{
    public byte[] Serialize<T>(T obj) where T : class
    {
        using MemoryStream stream = new();

        ProtoBuf.Serializer.Serialize(stream, obj);

        var results = new byte[stream.Length];
        stream.Position = 0;
        stream.Read(results, 0, results.Length);

        return results;
    }

    public T Deserialize<T>(byte[] data) where T : class
    {
        using MemoryStream stream = new();
        stream.Write(data, 0, data.Length);
        stream.Position = 0;

        var result = ProtoBuf.Serializer.Deserialize<T>(stream);
        return result;
    }
}
