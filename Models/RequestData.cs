using Models.dto;
using ProtoBuf;

namespace SimpleClient.Models;

[ProtoContract()]
public class RequestData<T> where T : ReqProtoBase
{
    [ProtoMember(1)]
    public T? Data { get; set; }
    [ProtoMember(2)]
    public required ERequestType Type { get; set; }
}
