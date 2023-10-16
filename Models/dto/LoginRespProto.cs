using ProtoBuf;

namespace Models.dto;

[ProtoContract]
public class LoginRespProto : RespProtoBase
{
    [ProtoMember(1)]
    public required bool IsOk { get; set; }
    [ProtoMember(2)]
    public string? ErrMsg { get; set; }

    [ProtoMember(4)]
    public int OutSeqNum { get; set; }
}
