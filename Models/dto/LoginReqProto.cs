using ProtoBuf;

namespace Models.dto;

[ProtoContract]
public class LoginReqProto : ReqProtoBase
{
    [ProtoMember(1)]
    public required string User { get; set; }
    [ProtoMember(2)]
    public required string Password { get; set; }
    [ProtoMember(6)]
    public int InSeqNum { get; set; }
}
