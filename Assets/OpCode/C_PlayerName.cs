using UnityEngine;

public class C_PlayerName : GeneriqueOpCode
{
    public string name;

    public C_PlayerName(string name = "")
    {
        opCode = EnetOpCode.OpCode.C_PlayerName;
        this.name = name;
    }

    public override void Serialize(ref byte[] byteArray)
    {
        NetworkCore.Serialize_str(ref byteArray, name);
    }

    public override void Unserialize(ref byte[] byteArray, int offset)
    {
        this.name = NetworkCore.Unserialize_str(ref byteArray, ref offset);
    }
}
